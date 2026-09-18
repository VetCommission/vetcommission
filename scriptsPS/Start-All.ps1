[CmdletBinding()]
param()

$ErrorActionPreference = 'Stop'
$repoRoot = Split-Path -Parent $PSScriptRoot
$containerName = 'vetcommission-postgres'

if (-not (Get-Command docker -ErrorAction SilentlyContinue)) {
    throw 'Docker não encontrado no PATH.'
}

$requiredFiles = @(
    'Start-Api.ps1',
    'Start-Worker.ps1',
    'Start-Frontend.ps1'
)

foreach ($file in $requiredFiles) {
    $scriptPath = Join-Path $PSScriptRoot $file
    if (-not (Test-Path -LiteralPath $scriptPath -PathType Leaf)) {
        throw "Script obrigatório não encontrado: $scriptPath"
    }
}

Push-Location $repoRoot
try {
    docker compose up -d postgres
    if ($LASTEXITCODE -ne 0) {
        throw 'Não foi possível iniciar o PostgreSQL pelo Docker Compose.'
    }

    $databaseHealthy = $false
    for ($attempt = 1; $attempt -le 12; $attempt++) {
        $health = docker inspect --format '{{.State.Health.Status}}' $containerName 2>$null
        if ($LASTEXITCODE -eq 0 -and $health -eq 'healthy') {
            $databaseHealthy = $true
            break
        }

        Start-Sleep -Seconds 5
    }

    if (-not $databaseHealthy) {
        throw "O PostgreSQL não ficou saudável no tempo esperado. Verifique: docker logs $containerName"
    }

    $powershellPath = (Get-Process -Id $PID).Path
    foreach ($scriptName in $requiredFiles) {
        $scriptPath = Join-Path $PSScriptRoot $scriptName
        $arguments = @(
            '-NoProfile',
            '-ExecutionPolicy', 'Bypass',
            '-File', "`"$scriptPath`""
        )

        Start-Process -FilePath $powershellPath -ArgumentList $arguments -WindowStyle Hidden
    }

    Write-Host 'PostgreSQL saudável. API, Worker e frontend foram iniciados em segundo plano.'
    Write-Host 'API: http://localhost:5077 | Frontend: http://localhost:3000'
}
finally {
    Pop-Location
}
