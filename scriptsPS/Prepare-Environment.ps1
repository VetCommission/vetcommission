[CmdletBinding()]
param(
    [switch]$SkipDotNet,
    [switch]$SkipFrontend,
    [switch]$SkipDocker,
    [switch]$SkipMigrations,
    [switch]$Force,
    [switch]$RefreshFrontendDependencies,
    [switch]$KeepBlockingProcesses
)

$ErrorActionPreference = 'Stop'
$repoRoot = Split-Path -Parent $PSScriptRoot
$environmentFile = Join-Path $repoRoot '.env'
$frontendPath = Join-Path $repoRoot 'frontend'
. (Join-Path $PSScriptRoot 'LocalEnvironment.ps1')

function Invoke-PreparationCommand {
    param([string]$Command, [string[]]$Arguments)
    & $Command @Arguments
    if ($LASTEXITCODE -ne 0) { throw "Preparacao falhou: $Command $($Arguments -join ' ') (codigo $LASTEXITCODE)." }
}

function Assert-PreparationCommand {
    param([string]$Name, [string]$Instructions)
    if (-not (Get-Command $Name -ErrorAction SilentlyContinue)) { throw "$Name nao encontrado. $Instructions" }
}

function Enable-PreparationNodeVersion {
    $version = (Get-Content -LiteralPath (Join-Path $repoRoot '.node-version') -Raw).Trim()
    $current = if (Get-Command node -ErrorAction SilentlyContinue) { (& node --version).TrimStart('v') } else { '' }
    if ($current -ne $version) {
        if (Get-Command nvm -ErrorAction SilentlyContinue) {
            Invoke-PreparationCommand 'nvm' @('install', $version)
            Invoke-PreparationCommand 'nvm' @('use', $version)
            $env:Path = "$([Environment]::GetEnvironmentVariable('Path', 'Machine'));$([Environment]::GetEnvironmentVariable('Path', 'User'))"
            $link = [Environment]::GetEnvironmentVariable('NVM_SYMLINK', 'User')
            if (-not $link) { $link = [Environment]::GetEnvironmentVariable('NVM_SYMLINK', 'Machine') }
            if ($link) { $env:Path = "$link;$env:Path" }
        }
        elseif (Get-Command fnm -ErrorAction SilentlyContinue) {
            fnm env --shell powershell | Out-String | Invoke-Expression
            Invoke-PreparationCommand 'fnm' @('install', $version)
            Invoke-PreparationCommand 'fnm' @('use', $version)
        }
        elseif (Get-Command volta -ErrorAction SilentlyContinue) {
            Invoke-PreparationCommand 'volta' @('install', "node@$version")
        }
    }
    Assert-PreparationCommand 'node' "Instale Node.js $version ou nvm/fnm/volta."
    Assert-PreparationCommand 'npm.cmd' 'Instale o npm junto com o Node.js.'
    if ((& node --version).TrimStart('v') -ne $version) { throw "Node.js $version e obrigatorio; ative essa versao e execute novamente." }
}

function Test-PreparationDocker {
    $previousPreference = $ErrorActionPreference
    $ErrorActionPreference = 'Continue'
    try {
        & docker info *> $null
        return $LASTEXITCODE -eq 0
    }
    finally { $ErrorActionPreference = $previousPreference }
}

Push-Location $repoRoot
try {
    if (-not $SkipDocker) {
        Write-Host '==> Preparando Docker'
        Assert-PreparationCommand 'docker' 'Instale o Docker Desktop com suporte a containers Linux.'
        if (-not (Test-PreparationDocker)) {
            $desktop = Join-Path $env:ProgramFiles 'Docker/Docker/Docker Desktop.exe'
            if (Test-Path -LiteralPath $desktop) {
                Write-Host 'Iniciando Docker Desktop e aguardando o engine...'
                Start-Process -FilePath $desktop -WindowStyle Hidden | Out-Null
                for ($attempt = 1; $attempt -le 30; $attempt++) {
                    if (Test-PreparationDocker) { break }
                    Start-Sleep -Seconds 2
                }
            }
            if (-not (Test-PreparationDocker)) { throw 'Docker nao esta acessivel. Abra o Docker Desktop e aguarde o engine; use -SkipDocker para preparar somente dependencias locais.' }
        }
        # Nao inventar credenciais novas para um volume que pode conter dados.
        if (-not (Test-Path -LiteralPath $environmentFile)) {
            $volumes = & docker volume ls --filter 'name=^vetcommission-postgres-data$' --format '{{.Name}}'
            if ($LASTEXITCODE -ne 0) { throw 'Nao foi possivel consultar o volume do PostgreSQL.' }
            if ($volumes -contains 'vetcommission-postgres-data') {
                throw 'Existe um volume PostgreSQL e o .env esta ausente. Recupere o .env com as credenciais originais antes de continuar; os dados foram preservados.'
            }
        }
    }

    Write-Host '==> Preparando configuracao local'
    Initialize-LocalEnvironmentFile -Path $environmentFile -TemplatePath (Join-Path $repoRoot '.env.example')
    $values = Get-LocalEnvironmentValues -Path $environmentFile
    Set-LocalDatabaseConnection -Values $values

    if (-not $SkipDotNet) {
        Write-Host '==> Restaurando .NET e ferramentas locais'
        Assert-PreparationCommand 'dotnet' 'Instale o SDK .NET 10 indicado no global.json.'
        $sdkVersion = & dotnet --version
        if ($LASTEXITCODE -ne 0 -or $sdkVersion -notmatch '^10\.') { throw 'SDK .NET 10 compativel com global.json nao esta disponivel.' }
        Invoke-PreparationCommand 'dotnet' @('restore', (Join-Path $repoRoot 'VetCommission.sln'))
        Invoke-PreparationCommand 'dotnet' @('tool', 'restore')
    }

    if (-not $SkipFrontend) {
        Write-Host '==> Preparando dependencias do frontend'
        Enable-PreparationNodeVersion
        Push-Location $frontendPath
        try {
            # Resolve divergencias do lockfile sem executar scripts de instalacao.
            Invoke-PreparationCommand 'npm.cmd' @('install', '--package-lock-only', '--ignore-scripts', '--no-audit', '--no-fund')
            $lockHash = (Get-FileHash -LiteralPath 'package-lock.json' -Algorithm SHA256).Hash
            $stampPath = Join-Path $repoRoot '.tmp/frontend-dependencies.sha256'
            $stamp = if (Test-Path -LiteralPath $stampPath) { (Get-Content -LiteralPath $stampPath -Raw).Trim() } else { '' }
            $ready = (Test-Path -LiteralPath 'node_modules/.bin/next.cmd') -and (Test-Path -LiteralPath 'node_modules/.bin/eslint.cmd')
            if ($RefreshFrontendDependencies -or -not $ready -or $stamp -ne $lockHash) {
                $connections = @(Get-NetTCPConnection -LocalPort 3000 -State Listen -ErrorAction SilentlyContinue)
                if ($connections.Count -gt 0 -and $KeepBlockingProcesses) { throw 'Frontend em execucao na porta 3000. Pare-o antes de instalar dependencias.' }
                foreach ($processId in @($connections | Select-Object -ExpandProperty OwningProcess -Unique)) {
                    if ($processId -ne $PID) { Stop-Process -Id $processId -Force }
                }
                Invoke-PreparationCommand 'npm.cmd' @('ci', '--no-audit', '--no-fund')
                New-Item -ItemType Directory -Path (Split-Path -Parent $stampPath) -Force | Out-Null
                Set-Content -LiteralPath $stampPath -Value $lockHash
            }
            else { Write-Host 'Dependencias ja correspondem ao lockfile; npm ci dispensado.' }
        }
        finally { Pop-Location }
    }

    if (-not $SkipDocker) {
        Write-Host '==> Preparando PostgreSQL local'
        $container = if ($values['POSTGRES_CONTAINER_NAME']) { $values['POSTGRES_CONTAINER_NAME'] } else { 'vetcommission-postgres' }
        Start-LocalPostgresContainer -Values $values -EnvironmentFile $environmentFile
        if (-not $SkipMigrations) {
            & (Join-Path $PSScriptRoot 'ExecutarScriptsDB.ps1') -EnvironmentFile $environmentFile -ContainerName $container -Force:$Force
            if (-not $?) { throw 'Falha ao aplicar scripts de banco.' }
        }
    }

    Write-Host 'Ambiente local preparado. Use .\scriptsPS\Start-All.ps1 para iniciar o app.' -ForegroundColor Green
}
finally { Pop-Location }
