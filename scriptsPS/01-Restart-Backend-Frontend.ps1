[CmdletBinding()]
param(
    [switch]$SkipDatabase
)

$ErrorActionPreference = 'Stop'
$repoRoot = Split-Path -Parent $PSScriptRoot
$apiPort = 5077
$frontendPort = 3000
$apiScript = Join-Path $PSScriptRoot 'Start-Api.ps1'
$frontendScript = Join-Path $PSScriptRoot 'Start-Frontend.ps1'

function Stop-ProcessesUsingTcpPort {
    param(
        [int]$Port,
        [string]$ServiceName
    )

    $connections = @(Get-NetTCPConnection -LocalPort $Port -State Listen -ErrorAction SilentlyContinue)
    $processIds = @($connections | Select-Object -ExpandProperty OwningProcess -Unique)

    foreach ($processId in $processIds) {
        if ($processId -eq $PID) {
            continue
        }

        $process = Get-Process -Id $processId -ErrorAction SilentlyContinue
        if ($process) {
            Write-Host "Encerrando $ServiceName na porta $Port (PID $processId)..."
            Stop-Process -Id $processId -Force -ErrorAction Stop
        }
    }
}

foreach ($requiredFile in @($apiScript, $frontendScript)) {
    if (-not (Test-Path -LiteralPath $requiredFile -PathType Leaf)) {
        throw "Script obrigatorio nao encontrado: $requiredFile"
    }
}

Write-Host '==> Parando servicos atuais'
Stop-ProcessesUsingTcpPort $apiPort 'API'
Stop-ProcessesUsingTcpPort $frontendPort 'frontend'

if (-not $SkipDatabase) {
    if (-not (Get-Command docker -ErrorAction SilentlyContinue)) {
        throw 'Docker nao encontrado no PATH. Use -SkipDatabase se o PostgreSQL ja estiver sendo executado fora deste script.'
    }

    Write-Host '==> Garantindo PostgreSQL local'
    Push-Location $repoRoot
    try {
        docker compose up -d postgres
        if ($LASTEXITCODE -ne 0) {
            throw 'Nao foi possivel iniciar o PostgreSQL pelo Docker Compose.'
        }
    }
    finally {
        Pop-Location
    }
}

$powershellPath = (Get-Process -Id $PID).Path
$childArguments = @('-NoProfile', '-ExecutionPolicy', 'Bypass', '-File')

Write-Host '==> Iniciando API em http://localhost:5077'
Start-Process -FilePath $powershellPath -ArgumentList ($childArguments + "`"$apiScript`"") -WorkingDirectory $repoRoot -WindowStyle Hidden

Write-Host '==> Iniciando frontend em http://localhost:3000'
Start-Process -FilePath $powershellPath -ArgumentList ($childArguments + "`"$frontendScript`"") -WorkingDirectory $repoRoot -WindowStyle Hidden

Write-Host 'API e frontend iniciados em segundo plano.'
Write-Host 'Use Ctrl+C apenas nesta janela nao encerra os processos filhos.'
Write-Host 'Para reiniciar, execute este script novamente.'

