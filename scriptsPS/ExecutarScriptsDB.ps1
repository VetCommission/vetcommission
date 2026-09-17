[CmdletBinding()]
param(
    [string]$EnvironmentFile,
    [string]$ContainerName
)

Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"
$ScriptVersion = "3.0"

function Assert-NativeCommandSucceeded {
    param([Parameter(Mandatory)][string]$Operation)

    if ($LASTEXITCODE -ne 0) {
        throw "$Operation falhou com o codigo de saida $LASTEXITCODE."
    }
}

function ConvertTo-SqlLiteral {
    param([Parameter(Mandatory)][string]$Value)

    return $Value.Replace("'", "''")
}

function Get-DotEnvValues {
    param([Parameter(Mandatory)][string]$Path)

    if (-not (Test-Path -LiteralPath $Path -PathType Leaf)) {
        throw "Arquivo de ambiente nao encontrado: $Path"
    }

    $values = @{}

    foreach ($line in Get-Content -LiteralPath $Path) {
        $content = $line.Trim()

        if ([string]::IsNullOrWhiteSpace($content) -or $content.StartsWith("#")) {
            continue
        }

        if ($content.StartsWith("export ")) {
            $content = $content.Substring(7).Trim()
        }

        $parts = $content -split "=", 2

        if ($parts.Count -ne 2) {
            throw "Linha invalida no arquivo de ambiente: $line"
        }

        $key = $parts[0].Trim()
        $value = $parts[1].Trim()

        if ([string]::IsNullOrWhiteSpace($key)) {
            throw "Variavel sem nome encontrada no arquivo de ambiente."
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

function Get-NormalizedSha256 {
    param([Parameter(Mandatory)][string]$Content)

    $normalizedContent = $Content -replace "`r`n?", "`n"
    $encoding = New-Object System.Text.UTF8Encoding($false)
    $bytes = $encoding.GetBytes($normalizedContent)
    $sha256 = [System.Security.Cryptography.SHA256]::Create()

    try {
        $hashBytes = $sha256.ComputeHash($bytes)
        return ([System.BitConverter]::ToString($hashBytes)).Replace("-", "").ToLowerInvariant()
    }
    finally {
        $sha256.Dispose()
    }
}

$projectRoot = [System.IO.Path]::GetFullPath((Join-Path $PSScriptRoot ".."))
$scriptsDirectory = Join-Path $projectRoot "database\scripts"

if ([string]::IsNullOrWhiteSpace($EnvironmentFile)) {
    $EnvironmentFile = Join-Path $projectRoot ".env"
}
else {
    $EnvironmentFile = [System.IO.Path]::GetFullPath($EnvironmentFile)
}

$environment = Get-DotEnvValues -Path $EnvironmentFile
$DatabaseName = Get-RequiredEnvironmentValue -Values $environment -Name "POSTGRES_DB" -Source $EnvironmentFile
$DatabaseUser = Get-RequiredEnvironmentValue -Values $environment -Name "POSTGRES_USER" -Source $EnvironmentFile

if ([string]::IsNullOrWhiteSpace($ContainerName)) {
    if ($environment.ContainsKey("POSTGRES_CONTAINER_NAME") -and
        -not [string]::IsNullOrWhiteSpace([string]$environment["POSTGRES_CONTAINER_NAME"])) {
        $ContainerName = [string]$environment["POSTGRES_CONTAINER_NAME"]
    }
    else {
        $ContainerName = "vetcommission-postgres"
    }
}

if (-not (Get-Command docker -ErrorAction SilentlyContinue)) {
    throw "Docker nao foi encontrado no PATH. Abra o Docker Desktop e tente novamente."
}

$containerIsRunning = & docker inspect --format '{{.State.Running}}' $ContainerName 2>$null
Assert-NativeCommandSucceeded -Operation "Consulta do container '$ContainerName'"

if (($containerIsRunning | Out-String).Trim() -ne "true") {
    throw "O container '$ContainerName' nao esta em execucao. Use: docker compose up -d"
}

if (-not (Test-Path -LiteralPath $scriptsDirectory -PathType Container)) {
    throw "A pasta de scripts SQL nao foi encontrada: $scriptsDirectory"
}

$scripts = @(
    Get-ChildItem -LiteralPath $scriptsDirectory -Filter "*.sql" -File |
        Sort-Object Name
)

if ($scripts.Count -eq 0) {
    Write-Host "Nenhum script SQL foi encontrado em '$scriptsDirectory'." -ForegroundColor Yellow
    exit 0
}

$expectedNumber = 1

foreach ($script in $scripts) {
    if ($script.Name -notmatch '^V(?<number>\d{3})__(?<description>[a-z0-9_]+)\.sql$') {
        throw "Nome de script invalido: '$($script.Name)'. Use VNNN__descricao.sql."
    }

    $scriptNumber = [int]$Matches.number

    if ($scriptNumber -ne $expectedNumber) {
        $expectedVersion = "V$($expectedNumber.ToString('000'))"
        throw "Sequencia invalida. Esperado $expectedVersion, encontrado V$($Matches.number)."
    }

    $expectedNumber++
}

Write-Host "Executor : ExecutarScriptsDB.ps1 (versao $ScriptVersion)" -ForegroundColor Magenta
Write-Host "Ambiente : $EnvironmentFile" -ForegroundColor DarkGray
Write-Host "Container: $ContainerName" -ForegroundColor DarkGray
Write-Host "Banco    : $DatabaseName" -ForegroundColor DarkGray

Write-Warning "Os scripts pendentes alterarao o banco '$DatabaseName' no container '$ContainerName'."
$expectedConfirmation = "ATUALIZAR $DatabaseName"
$confirmation = Read-Host "Para continuar, digite exatamente: $expectedConfirmation"

if ($confirmation -cne $expectedConfirmation) {
    Write-Host "Execucao cancelada. Nenhuma alteracao foi feita no banco." -ForegroundColor Yellow
    exit 0
}

foreach ($script in $scripts) {
    if ($script.Name -notmatch '^V(?<number>\d{3})__(?<description>[a-z0-9_]+)\.sql$') {
        throw "Nome de script invalido: '$($script.Name)'."
    }

    $version = "V$($Matches.number)"
    $description = $Matches.description.Replace("_", " ")
    $scriptContent = Get-Content -LiteralPath $script.FullName -Raw

    if ($scriptContent -match '(?im)^\s*(BEGIN|START\s+TRANSACTION|COMMIT|ROLLBACK)\s*;') {
        throw "O script '$($script.Name)' contem controle de transacao. BEGIN e COMMIT sao responsabilidade do executor."
    }

    $checksum = Get-NormalizedSha256 -Content $scriptContent

    $historyExistsResult = & docker exec $ContainerName psql `
        --username=$DatabaseUser `
        --dbname=$DatabaseName `
        --tuples-only `
        --no-align `
        --set=ON_ERROR_STOP=1 `
        --command="SELECT to_regclass('core.database_version') IS NOT NULL;"

    Assert-NativeCommandSucceeded -Operation "Consulta da tabela database_version"
    $historyExists = (($historyExistsResult | Out-String).Trim() -eq "t")

    if ($historyExists) {
        $registeredChecksumResult = & docker exec $ContainerName psql `
            --username=$DatabaseUser `
            --dbname=$DatabaseName `
            --tuples-only `
            --no-align `
            --set=ON_ERROR_STOP=1 `
            --command="SELECT checksum_sha256 FROM core.database_version WHERE version = '$version';"

        Assert-NativeCommandSucceeded -Operation "Consulta da versao $version"
        $registeredChecksum = ($registeredChecksumResult | Out-String).Trim().ToLowerInvariant()

        if (-not [string]::IsNullOrWhiteSpace($registeredChecksum)) {
            if ($registeredChecksum -ne $checksum) {
                throw "O script $version ja foi aplicado, mas seu hash foi alterado. Crie uma nova versao corretiva."
            }

            Write-Host "$version ja aplicado. Ignorando." -ForegroundColor DarkGray
            continue
        }
    }
    elseif ($version -ne "V001") {
        throw "A tabela core.database_version nao existe. O primeiro script deve ser V001."
    }

    Write-Host "Aplicando $version - $description..." -ForegroundColor Cyan

    $safeVersion = ConvertTo-SqlLiteral -Value $version
    $safeDescription = ConvertTo-SqlLiteral -Value $description
    $safeScriptName = ConvertTo-SqlLiteral -Value $script.Name
    $safeChecksum = ConvertTo-SqlLiteral -Value $checksum

    $transactionScript = @"
\set ON_ERROR_STOP on

BEGIN;

$scriptContent

INSERT INTO core.database_version
(
    version,
    description,
    script_name,
    checksum_sha256
)
VALUES
(
    '$safeVersion',
    '$safeDescription',
    '$safeScriptName',
    '$safeChecksum'
);

COMMIT;
"@

    $temporaryFile = Join-Path `
        ([System.IO.Path]::GetTempPath()) `
        "vetcommission-$version-$([Guid]::NewGuid().ToString('N')).sql"

    $containerFile = "/tmp/vetcommission-$version-$([Guid]::NewGuid().ToString('N')).sql"

    try {
        $utf8WithoutBom = New-Object System.Text.UTF8Encoding($false)
        [System.IO.File]::WriteAllText($temporaryFile, $transactionScript, $utf8WithoutBom)

        & docker cp $temporaryFile "${ContainerName}:$containerFile"
        Assert-NativeCommandSucceeded -Operation "Copia do script $version para o container"

        & docker exec $ContainerName psql `
            --username=$DatabaseUser `
            --dbname=$DatabaseName `
            --set=ON_ERROR_STOP=1 `
            --file=$containerFile

        Assert-NativeCommandSucceeded -Operation "Execucao do script $version"
        Write-Host "$version aplicado com sucesso." -ForegroundColor Green
    }
    finally {
        if (Test-Path -LiteralPath $temporaryFile) {
            Remove-Item -LiteralPath $temporaryFile -Force
        }

        & docker exec $ContainerName rm -f $containerFile 2>$null | Out-Null
    }
}

Write-Host "Banco de dados atualizado com sucesso." -ForegroundColor Green
