[CmdletBinding()]
param()

$ErrorActionPreference = 'Stop'
$repoRoot = Split-Path -Parent $PSScriptRoot
$projectPath = Join-Path $repoRoot 'src\VetCommission.WebApi\VetCommission.WebApi.csproj'

if (-not (Get-Command dotnet -ErrorAction SilentlyContinue)) {
    throw 'SDK .NET não encontrado no PATH.'
}

if (-not (Test-Path -LiteralPath $projectPath -PathType Leaf)) {
    throw "Projeto da API não encontrado: $projectPath"
}

Push-Location $repoRoot
try {
    dotnet run --project $projectPath --urls 'http://localhost:5077'
    if ($LASTEXITCODE -ne 0) {
        throw "A API foi encerrada com código $LASTEXITCODE."
    }
}
finally {
    Pop-Location
}
