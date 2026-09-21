[CmdletBinding()]
param(
    [switch]$StopDatabase
)

$ErrorActionPreference = 'Stop'
$repoRoot = Split-Path -Parent $PSScriptRoot

function Stop-ProcessesUsingTcpPort {
    param(
        [int]$Port,
        [string]$ServiceName
    )

    $connections = @(Get-NetTCPConnection -LocalPort $Port -State Listen -ErrorAction SilentlyContinue)
    $processIds = @($connections | Select-Object -ExpandProperty OwningProcess -Unique)

    foreach ($processId in $processIds) {
        if ($processId -eq $PID) { continue }

        $process = Get-Process -Id $processId -ErrorAction SilentlyContinue
        if ($process) {
            Write-Host "Encerrando $ServiceName na porta $Port (PID $processId)..."
            Stop-Process -Id $processId -Force
        }
    }
}

function Stop-WorkerProcesses {
    $workerProcesses = @(Get-CimInstance Win32_Process -Filter "Name = 'dotnet.exe'" -ErrorAction SilentlyContinue |
        Where-Object { $_.CommandLine -and $_.CommandLine -match 'VetCommission\.Worker|Start-Worker\.ps1' })

    foreach ($worker in $workerProcesses) {
        Write-Host "Encerrando Worker (PID $($worker.ProcessId))..."
        Stop-Process -Id $worker.ProcessId -Force
    }
}

Write-Host '==> Parando API'
Stop-ProcessesUsingTcpPort 5077 'API'

Write-Host '==> Parando frontend'
Stop-ProcessesUsingTcpPort 3000 'frontend'

Write-Host '==> Parando Worker'
Stop-WorkerProcesses

if ($StopDatabase) {
    if (-not (Get-Command docker -ErrorAction SilentlyContinue)) {
        throw 'Docker nao encontrado no PATH.'
    }

    Write-Host '==> Parando PostgreSQL'
    Push-Location $repoRoot
    try {
        docker compose stop postgres
        if ($LASTEXITCODE -ne 0) {
            throw 'Nao foi possivel parar o PostgreSQL pelo Docker Compose.'
        }
    }
    finally {
        Pop-Location
    }
}

Write-Host 'Servicos do VetCommission encerrados.'
if (-not $StopDatabase) {
    Write-Host 'PostgreSQL preservado. Use -StopDatabase para parar o banco tambem.'
}

