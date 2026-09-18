[CmdletBinding()]
param()

$ErrorActionPreference = 'Stop'
$repoRoot = Split-Path -Parent $PSScriptRoot
$projectPath = Join-Path $repoRoot 'src\VetCommission.Worker\VetCommission.Worker.csproj'

if (-not (Get-Command dotnet -ErrorAction SilentlyContinue)) {
    throw 'SDK .NET não encontrado no PATH.'
}

if (-not (Test-Path -LiteralPath $projectPath -PathType Leaf)) {
    throw "Projeto do Worker não encontrado: $projectPath"
}

Push-Location $repoRoot
try {
    dotnet run --project $projectPath
    if ($LASTEXITCODE -ne 0) {
        throw "O Worker foi encerrado com código $LASTEXITCODE."
    }
}
finally {
    Pop-Location
}
