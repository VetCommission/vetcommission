[CmdletBinding()]
param(
    [string]$EnvironmentFile
)

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

function Get-DotEnvValues {
    param([Parameter(Mandatory)][string]$Path)

    if (-not (Test-Path -LiteralPath $Path -PathType Leaf)) {
        throw "Arquivo de ambiente nao encontrado: $Path"
    }

    $values = @{}

    foreach ($line in Get-Content -LiteralPath $Path) {
        $content = $line.Trim()

        if ([string]::IsNullOrWhiteSpace($content) -or $content.StartsWith('#')) {
            continue
        }

        if ($content.StartsWith('export ')) {
            $content = $content.Substring(7).Trim()
        }

        $parts = $content -split '=', 2
        if ($parts.Count -ne 2) {
            throw "Linha invalida no arquivo de ambiente: $line"
        }

        $key = $parts[0].Trim()
        $value = $parts[1].Trim()

        if ([string]::IsNullOrWhiteSpace($key)) {
            throw 'Variavel sem nome encontrada no arquivo de ambiente.'
        }

        if ($value.Length -ge 2) {
            $hasDoubleQuotes = $value.StartsWith('"') -and $value.EndsWith('"')
            $hasSingleQuotes = $value.StartsWith("'") -and $value.EndsWith("'")

            if ($hasDoubleQuotes -or $hasSingleQuotes) {
                $value = $value.Substring(1, $value.Length - 2)
            }
        }

        $values[$key] = $value
    }

    return $values
}

function Get-RequiredEnvironmentValue {
    param(
        [Parameter(Mandatory)][hashtable]$Values,
        [Parameter(Mandatory)][string]$Name,
        [Parameter(Mandatory)][string]$Source
    )

    if (-not $Values.ContainsKey($Name)) {
        throw "A variavel $Name nao foi encontrada em '$Source'."
    }

    $value = [string]$Values[$Name]
    if ([string]::IsNullOrWhiteSpace($value)) {
        throw "A variavel $Name esta vazia em '$Source'."
    }

    return $value
}

function Assert-NativeCommandSucceeded {
    param([Parameter(Mandatory)][string]$Operation)

    if ($LASTEXITCODE -ne 0) {
        throw "$Operation falhou com o codigo de saida $LASTEXITCODE."
    }
}

$repoRoot = [System.IO.Path]::GetFullPath((Join-Path $PSScriptRoot '..'))
$infrastructureProject = Join-Path $repoRoot 'src\VetCommission.Infrastructure\VetCommission.Infrastructure.csproj'

if ([string]::IsNullOrWhiteSpace($EnvironmentFile)) {
    $EnvironmentFile = Join-Path $repoRoot '.env'
}
else {
    $EnvironmentFile = [System.IO.Path]::GetFullPath($EnvironmentFile)
}

if (-not (Get-Command dotnet -ErrorAction SilentlyContinue)) {
    throw 'SDK .NET nao encontrado no PATH.'
}

if (-not (Test-Path -LiteralPath $infrastructureProject -PathType Leaf)) {
    throw "Projeto de Infrastructure nao encontrado: $infrastructureProject"
}

$environment = Get-DotEnvValues -Path $EnvironmentFile
$databaseName = Get-RequiredEnvironmentValue -Values $environment -Name 'POSTGRES_DB' -Source $EnvironmentFile
$databaseUser = Get-RequiredEnvironmentValue -Values $environment -Name 'POSTGRES_USER' -Source $EnvironmentFile
$databasePassword = Get-RequiredEnvironmentValue -Values $environment -Name 'POSTGRES_PASSWORD' -Source $EnvironmentFile

$databaseHost = 'localhost'
$databasePort = '5432'

if ($environment.ContainsKey('POSTGRES_HOST') -and -not [string]::IsNullOrWhiteSpace([string]$environment['POSTGRES_HOST'])) {
    $databaseHost = [string]$environment['POSTGRES_HOST']
}

if ($environment.ContainsKey('POSTGRES_PORT') -and -not [string]::IsNullOrWhiteSpace([string]$environment['POSTGRES_PORT'])) {
    $databasePort = [string]$environment['POSTGRES_PORT']
}

$connectionString = "Host=$databaseHost;Port=$databasePort;Database=$databaseName;Username=$databaseUser;Password=$databasePassword"

Write-Host 'Scaffold EF Core' -ForegroundColor Magenta
Write-Host "Ambiente : $EnvironmentFile" -ForegroundColor DarkGray
Write-Host "Banco    : $databaseName" -ForegroundColor DarkGray
Write-Host "Schemas  : core, business" -ForegroundColor DarkGray
Write-Host 'Senha    : <oculta>' -ForegroundColor DarkGray

Push-Location $repoRoot
try {
    dotnet tool restore
    Assert-NativeCommandSucceeded -Operation 'Restore das ferramentas .NET locais'

    dotnet tool run dotnet-ef dbcontext scaffold `
        $connectionString `
        Npgsql.EntityFrameworkCore.PostgreSQL `
        --project $infrastructureProject `
        --context VetCommissionDbContext `
        --context-dir Persistence\Generated `
        --output-dir Persistence\Generated\Entities `
        --schema core `
        --schema business `
        --no-onconfiguring `
        --force

    Assert-NativeCommandSucceeded -Operation 'Scaffold EF Core'
}
finally {
    Pop-Location
}

Write-Host 'Scaffold EF Core concluido com sucesso.' -ForegroundColor Green
