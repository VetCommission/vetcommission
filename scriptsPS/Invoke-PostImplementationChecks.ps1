[CmdletBinding()]
param(
    [switch]$SkipDotNet,
    [switch]$SkipFrontend,
    [switch]$SkipDocker,
    [switch]$SkipRuntime,
    [switch]$SkipGitChecks,
    [switch]$KeepRuntimeProcesses
)

$ErrorActionPreference = 'Stop'
$repoRoot = Split-Path -Parent $PSScriptRoot
$solutionPath = Join-Path $repoRoot 'VetCommission.sln'
$frontendPath = Join-Path $repoRoot 'frontend'
$apiProjectPath = Join-Path $repoRoot 'src\VetCommission.WebApi\VetCommission.WebApi.csproj'
$workerProjectPath = Join-Path $repoRoot 'src\VetCommission.Worker\VetCommission.Worker.csproj'
$requiredNodeVersion = 'v24.17.0'
$requiredNodeVersionWithoutPrefix = $requiredNodeVersion.TrimStart('v')
$postgresContainerName = 'vetcommission-postgres'
$apiUrl = 'http://localhost:5077'
$frontendUrl = 'http://localhost:3000'
$logsPath = Join-Path $repoRoot '.tmp\post-implementation-checks'
$startedProcesses = New-Object System.Collections.Generic.List[System.Diagnostics.Process]

function Write-Section {
    param([Parameter(Mandatory)][string]$Message)

    Write-Host ''
    Write-Host "==> $Message" -ForegroundColor Cyan
}

function Assert-Command {
    param([Parameter(Mandatory)][string]$Name)

    if (-not (Get-Command $Name -ErrorAction SilentlyContinue)) {
        throw "Comando obrigatorio nao encontrado no PATH: $Name"
    }
}

function Assert-File {
    param([Parameter(Mandatory)][string]$Path)

    if (-not (Test-Path -LiteralPath $Path -PathType Leaf)) {
        throw "Arquivo obrigatorio nao encontrado: $Path"
    }
}

function Assert-Directory {
    param([Parameter(Mandatory)][string]$Path)

    if (-not (Test-Path -LiteralPath $Path -PathType Container)) {
        throw "Diretorio obrigatorio nao encontrado: $Path"
    }
}

function Invoke-NativeCommand {
    param(
        [Parameter(Mandatory)][string]$Command,
        [Parameter()][string[]]$Arguments = @(),
        [Parameter()][string]$WorkingDirectory = $repoRoot
    )

    Push-Location $WorkingDirectory
    try {
        & $Command @Arguments
        if ($LASTEXITCODE -ne 0) {
            $argumentText = $Arguments -join ' '
            throw "Comando falhou com codigo ${LASTEXITCODE}: $Command $argumentText"
        }
    }
    finally {
        Pop-Location
    }
}

function Update-NodePath {
    $machinePath = [Environment]::GetEnvironmentVariable('Path', 'Machine')
    $userPath = [Environment]::GetEnvironmentVariable('Path', 'User')
    $env:Path = "$machinePath;$userPath"

    $nvmSymlink = $env:NVM_SYMLINK
    if (-not $nvmSymlink) {
        $nvmSymlink = [Environment]::GetEnvironmentVariable('NVM_SYMLINK', 'User')
    }
    if (-not $nvmSymlink) {
        $nvmSymlink = [Environment]::GetEnvironmentVariable('NVM_SYMLINK', 'Machine')
    }

    if ($nvmSymlink -and (Test-Path -LiteralPath $nvmSymlink -PathType Container)) {
        $env:Path = "$nvmSymlink;$env:Path"
    }
}

function Get-CurrentNodeVersion {
    if (-not (Get-Command node -ErrorAction SilentlyContinue)) {
        return $null
    }

    return node --version
}

function Use-RequiredNodeVersion {
    Assert-Command 'node'
    Assert-Command 'npm.cmd'

    $currentNodeVersion = Get-CurrentNodeVersion
    if ($currentNodeVersion -eq $requiredNodeVersion) {
        return
    }

    Write-Host "Node.js atual: $currentNodeVersion. Tentando ativar $requiredNodeVersion..."

    $fnmCommand = Get-Command fnm -ErrorAction SilentlyContinue
    $nvmCommand = Get-Command nvm -ErrorAction SilentlyContinue
    $voltaCommand = Get-Command volta -ErrorAction SilentlyContinue

    if ($fnmCommand) {
        Push-Location $frontendPath
        try {
            fnm env --use-on-cd | Out-String | Invoke-Expression
            fnm use $requiredNodeVersionWithoutPrefix
            if ($LASTEXITCODE -ne 0) {
                throw "fnm nao conseguiu ativar Node.js $requiredNodeVersion."
            }
        }
        finally {
            Pop-Location
        }
    }
    elseif ($nvmCommand) {
        nvm use $requiredNodeVersionWithoutPrefix
        Update-NodePath
        $currentNodeVersion = Get-CurrentNodeVersion
        if ($currentNodeVersion -ne $requiredNodeVersion) {
            Write-Host "Node.js $requiredNodeVersion nao esta instalado no nvm. Instalando..."
            nvm install $requiredNodeVersionWithoutPrefix
            if ($LASTEXITCODE -ne 0) {
                throw "nvm nao conseguiu instalar Node.js $requiredNodeVersion."
            }

            nvm use $requiredNodeVersionWithoutPrefix
            if ($LASTEXITCODE -ne 0) {
                throw "nvm instalou, mas nao conseguiu ativar Node.js $requiredNodeVersion."
            }

            Update-NodePath
        }
    }
    elseif ($voltaCommand) {
        volta install "node@$requiredNodeVersionWithoutPrefix"
        if ($LASTEXITCODE -ne 0) {
            throw "volta nao conseguiu instalar/ativar Node.js $requiredNodeVersion."
        }
    }
    else {
        throw "Node.js $requiredNodeVersion e obrigatorio. Versao atual: $currentNodeVersion. Instale a versao correta com nvm install $requiredNodeVersionWithoutPrefix ou instale fnm/volta."
    }

    $currentNodeVersion = Get-CurrentNodeVersion
    if ($currentNodeVersion -ne $requiredNodeVersion) {
        throw "Node.js $requiredNodeVersion e obrigatorio. Versao atual apos ativacao: $currentNodeVersion."
    }
}

function Wait-HttpOk {
    param(
        [Parameter(Mandatory)][string]$Url,
        [Parameter()][int]$Attempts = 30,
        [Parameter()][int]$DelaySeconds = 2
    )

    for ($attempt = 1; $attempt -le $Attempts; $attempt++) {
        try {
            $response = Invoke-WebRequest -Uri $Url -UseBasicParsing -TimeoutSec 5
            if ($response.StatusCode -ge 200 -and $response.StatusCode -lt 500) {
                Write-Host "URL respondeu: $Url [$($response.StatusCode)]"
                return
            }
        }
        catch {
            if ($attempt -eq $Attempts) {
                throw "URL nao respondeu no tempo esperado: $Url"
            }
        }

        Start-Sleep -Seconds $DelaySeconds
    }
}

function Assert-DockerDaemon {
    Assert-Command 'docker'

    $previousErrorActionPreference = $ErrorActionPreference
    $ErrorActionPreference = 'Continue'
    try {
        docker info > $null 2> $null
        $dockerExitCode = $LASTEXITCODE
    }
    finally {
        $ErrorActionPreference = $previousErrorActionPreference
    }

    if ($dockerExitCode -ne 0) {
        throw 'Docker esta instalado, mas o daemon nao esta acessivel. Abra o Docker Desktop, aguarde ele ficar em execucao e rode o script novamente. Para validar sem runtime/Docker, use -SkipRuntime ou -SkipDocker.'
    }
}

function Write-ProcessLogTail {
    param(
        [Parameter(Mandatory)][string]$Name,
        [Parameter(Mandatory)][string]$LogPath
    )

    if (-not (Test-Path -LiteralPath $LogPath -PathType Leaf)) {
        return
    }

    Write-Host ''
    Write-Host "Ultimas linhas do log de $Name ($LogPath):" -ForegroundColor Yellow
    Get-Content -LiteralPath $LogPath -Tail 80
}

function Convert-ProcessArgument {
    param([Parameter(Mandatory)][string]$Argument)

    if ($Argument -notmatch '[\s"]') {
        return $Argument
    }

    $escapedArgument = $Argument.Replace('"', '\"')
    return "`"$escapedArgument`""
}

function Start-CheckedProcess {
    param(
        [Parameter(Mandatory)][string]$FilePath,
        [Parameter(Mandatory)][string[]]$ArgumentList,
        [Parameter(Mandatory)][string]$WorkingDirectory,
        [Parameter(Mandatory)][string]$Name
    )

    if (-not (Test-Path -LiteralPath $logsPath -PathType Container)) {
        New-Item -ItemType Directory -Path $logsPath -Force | Out-Null
    }

    $safeName = $Name.ToLowerInvariant()
    $timestamp = Get-Date -Format 'yyyyMMdd-HHmmss'
    $stdoutPath = Join-Path $logsPath "$safeName-$timestamp.out.log"
    $stderrPath = Join-Path $logsPath "$safeName-$timestamp.err.log"
    $escapedArgumentList = $ArgumentList | ForEach-Object { Convert-ProcessArgument $_ }

    $process = Start-Process `
        -FilePath $FilePath `
        -ArgumentList $escapedArgumentList `
        -WorkingDirectory $WorkingDirectory `
        -WindowStyle Hidden `
        -RedirectStandardOutput $stdoutPath `
        -RedirectStandardError $stderrPath `
        -PassThru

    $startedProcesses.Add($process)
    Start-Sleep -Seconds 3

    if ($process.HasExited) {
        $process.Refresh()
        Write-ProcessLogTail $Name $stdoutPath
        Write-ProcessLogTail $Name $stderrPath
        throw "$Name encerrou antes do esperado com codigo $($process.ExitCode)."
    }

    Write-Host "$Name iniciado em segundo plano. PID: $($process.Id). Logs: $stdoutPath | $stderrPath"
    return $process
}

function Stop-StartedProcesses {
    if ($KeepRuntimeProcesses) {
        Write-Host 'Processos de runtime mantidos ativos conforme parametro -KeepRuntimeProcesses.'
        return
    }

    foreach ($process in $startedProcesses) {
        if ($null -ne $process -and -not $process.HasExited) {
            Stop-Process -Id $process.Id -Force
            Write-Host "Processo encerrado. PID: $($process.Id)"
        }
    }
}

try {
    Write-Section 'Validando estrutura minima'
    Assert-File $solutionPath
    Assert-File $apiProjectPath
    Assert-File $workerProjectPath
    Assert-Directory $frontendPath

    if (-not $SkipDotNet) {
        Write-Section 'Validando backend .NET'
        Assert-Command 'dotnet'
        Invoke-NativeCommand 'dotnet' @('restore', $solutionPath)
        Invoke-NativeCommand 'dotnet' @('build', $solutionPath, '--no-restore')
        Invoke-NativeCommand 'dotnet' @('test', $solutionPath, '--no-build')
    }

    if (-not $SkipFrontend) {
        Write-Section 'Validando frontend'
        Use-RequiredNodeVersion

        Invoke-NativeCommand 'npm.cmd' @('ci') $frontendPath
        Invoke-NativeCommand 'npm.cmd' @('run', 'lint') $frontendPath
        Invoke-NativeCommand 'npm.cmd' @('run', 'build') $frontendPath
    }

    if (-not $SkipDocker -and -not $SkipRuntime) {
        Write-Section 'Validando PostgreSQL local'
        Assert-DockerDaemon

        Invoke-NativeCommand 'docker' @('compose', 'up', '-d', 'postgres')

        $databaseHealthy = $false
        for ($attempt = 1; $attempt -le 12; $attempt++) {
            $health = docker inspect --format '{{.State.Health.Status}}' $postgresContainerName 2>$null
            if ($LASTEXITCODE -eq 0 -and $health -eq 'healthy') {
                $databaseHealthy = $true
                break
            }

            Start-Sleep -Seconds 5
        }

        if (-not $databaseHealthy) {
            throw "PostgreSQL nao ficou saudavel no tempo esperado. Verifique: docker logs $postgresContainerName"
        }

        Write-Host 'PostgreSQL saudavel.'
    }

    if (-not $SkipRuntime) {
        Write-Section 'Validando runtime da API'
        Assert-Command 'dotnet'
        Start-CheckedProcess 'dotnet' @('run', '--project', $apiProjectPath, '--urls', $apiUrl) $repoRoot 'WebApi'
        Wait-HttpOk "$apiUrl/health"

        Write-Section 'Validando runtime do Worker'
        Start-CheckedProcess 'dotnet' @('run', '--project', $workerProjectPath) $repoRoot 'Worker'
        Start-Sleep -Seconds 5

        Write-Section 'Validando runtime do frontend'
        Use-RequiredNodeVersion
        Start-CheckedProcess 'npm.cmd' @('run', 'dev', '--', '--hostname', '127.0.0.1', '--port', '3000') $frontendPath 'Frontend'
        Wait-HttpOk $frontendUrl
    }

    if (-not $SkipGitChecks) {
        Write-Section 'Validando diff e status Git'
        Assert-Command 'git'
        Invoke-NativeCommand 'git' @('diff', '--check')
        Invoke-NativeCommand 'git' @('status', '--short')
    }

    Write-Section 'Processo pos-implementacao concluido'
    Write-Host 'Revise os resultados acima e registre as evidencias na issue/PR.'
}
finally {
    Stop-StartedProcesses
}
