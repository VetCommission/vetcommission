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

$environment = @{}
foreach ($line in Get-Content -LiteralPath $environmentFile) {
    $trimmedLine = $line.Trim()
    if ([string]::IsNullOrWhiteSpace($trimmedLine) -or $trimmedLine.StartsWith('#')) {
        continue
    }

    $separatorIndex = $trimmedLine.IndexOf('=')
    if ($separatorIndex -lt 1) {
        continue
    }

    $name = $trimmedLine.Substring(0, $separatorIndex).Trim()
    $value = $trimmedLine.Substring($separatorIndex + 1).Trim().Trim('"').Trim("'")
    $environment[$name] = $value
}

foreach ($requiredName in @('POSTGRES_DB', 'POSTGRES_USER', 'POSTGRES_PASSWORD')) {
    if (-not $environment.ContainsKey($requiredName) -or [string]::IsNullOrWhiteSpace($environment[$requiredName])) {
        throw "Variavel $requiredName nao encontrada no arquivo .env."
    }
}

$databaseHost = if ($environment.ContainsKey('POSTGRES_HOST') -and $environment['POSTGRES_HOST']) { $environment['POSTGRES_HOST'] } else { 'localhost' }
$databasePort = if ($environment.ContainsKey('POSTGRES_PORT') -and $environment['POSTGRES_PORT']) { $environment['POSTGRES_PORT'] } else { '5432' }
$env:ConnectionStrings__DefaultConnection = "Host=$databaseHost;Port=$databasePort;Database=$($environment['POSTGRES_DB']);Username=$($environment['POSTGRES_USER']);Password=$($environment['POSTGRES_PASSWORD']);"

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
