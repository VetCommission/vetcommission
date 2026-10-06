[CmdletBinding()]
param()

$ErrorActionPreference = 'Stop'
$repoRoot = Split-Path -Parent $PSScriptRoot
$projectPath = Join-Path $repoRoot 'src\VetCommission.WebApi\VetCommission.WebApi.csproj'
$environmentFile = Join-Path $repoRoot '.env'

if (-not (Get-Command dotnet -ErrorAction SilentlyContinue)) {
    throw 'SDK .NET não encontrado no PATH.'
}

if (-not (Test-Path -LiteralPath $projectPath -PathType Leaf)) {
    throw "Projeto da API não encontrado: $projectPath"
}

if (-not (Test-Path -LiteralPath $environmentFile -PathType Leaf)) {
    throw "Arquivo .env nao encontrado: $environmentFile"
}

. (Join-Path $PSScriptRoot 'LocalEnvironment.ps1')
Set-LocalDatabaseConnection -Values (Get-LocalEnvironmentValues -Path $environmentFile)

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
