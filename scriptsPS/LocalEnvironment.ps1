# Funcoes compartilhadas; carregar com dot-sourcing.
function Initialize-LocalEnvironmentFile {
    param([string]$Path, [string]$TemplatePath)

    if (Test-Path -LiteralPath $Path -PathType Leaf) {
        Write-Host '.env existente preservado.'
        return
    }
    if (-not (Test-Path -LiteralPath $TemplatePath -PathType Leaf)) {
        throw "Modelo de ambiente nao encontrado: $TemplatePath"
    }
    $bytes = New-Object byte[] 32
    $random = [Security.Cryptography.RandomNumberGenerator]::Create()
    try { $random.GetBytes($bytes) } finally { $random.Dispose() }
    $password = ([BitConverter]::ToString($bytes)).Replace('-', '').ToLowerInvariant()
    $content = (Get-Content -LiteralPath $TemplatePath -Raw).Replace('defina_uma_senha_local', $password)
    # CreateNew impede sobrescrever um arquivo criado simultaneamente.
    $stream = [IO.File]::Open($Path, [IO.FileMode]::CreateNew, [IO.FileAccess]::Write)
    try {
        $encoded = (New-Object Text.UTF8Encoding($false)).GetBytes($content)
        $stream.Write($encoded, 0, $encoded.Length)
    }
    finally { $stream.Dispose() }
    Write-Host '.env criado a partir de .env.example, com senha local gerada.'
}

function Get-LocalEnvironmentValues {
    param([string]$Path)
    if (-not (Test-Path -LiteralPath $Path -PathType Leaf)) { throw "Arquivo .env nao encontrado: $Path" }
    $values = @{}
    foreach ($line in Get-Content -LiteralPath $Path) {
        $content = $line.Trim()
        if (-not $content -or $content.StartsWith('#')) { continue }
        if ($content.StartsWith('export ')) { $content = $content.Substring(7).Trim() }
        $parts = $content -split '=', 2
        if ($parts.Count -ne 2 -or -not $parts[0].Trim()) { throw "Linha invalida no arquivo .env: $Path" }
        $value = $parts[1].Trim()
        if ($value.StartsWith('"')) {
            if ($value -notmatch '^"(?<value>(?:\\.|[^"\\])*)"\s*(?:#.*)?$') { throw "Valor entre aspas invalido no .env: $Path" }
            $value = [regex]::Replace($Matches.value, '\\([\\"nrt$])', {
                param($match)
                switch ($match.Groups[1].Value) {
                    'n' { "`n" }
                    'r' { "`r" }
                    't' { "`t" }
                    default { $match.Groups[1].Value }
                }
            })
        }
        elseif ($value.StartsWith("'")) {
            if ($value -notmatch "^'(?<value>(?:\\.|[^'\\])*)'\s*(?:#.*)?$") { throw "Valor entre aspas invalido no .env: $Path" }
            $value = $Matches.value.Replace("\'", "'")
        }
        else {
            $value = ($value -replace '\s+#.*$', '').TrimEnd()
        }
        $values[$parts[0].Trim()] = $value
    }
    foreach ($name in @('POSTGRES_DB', 'POSTGRES_USER', 'POSTGRES_PASSWORD')) {
        if (-not $values.ContainsKey($name) -or [string]::IsNullOrWhiteSpace($values[$name])) {
            throw "Variavel $name ausente ou vazia em '$Path'."
        }
    }
    return $values
}

function Set-LocalDatabaseConnection {
    param([hashtable]$Values)
    $builder = New-Object System.Data.Common.DbConnectionStringBuilder
    $builder['Host'] = if ($Values['POSTGRES_HOST']) { $Values['POSTGRES_HOST'] } else { 'localhost' }
    $port = if ($Values['POSTGRES_PORT']) { $Values['POSTGRES_PORT'] } else { '5432' }
    $parsedPort = 0
    if (-not [int]::TryParse($port, [ref]$parsedPort) -or $parsedPort -lt 1 -or $parsedPort -gt 65535) {
        throw 'POSTGRES_PORT deve ser um numero entre 1 e 65535.'
    }
    $builder['Port'] = $port
    $builder['Database'] = $Values['POSTGRES_DB']
    $builder['Username'] = $Values['POSTGRES_USER']
    $builder['Password'] = $Values['POSTGRES_PASSWORD']
    $env:ConnectionStrings__DefaultConnection = $builder.ConnectionString
}

function Start-LocalPostgresContainer {
    param([hashtable]$Values, [string]$EnvironmentFile)
    $previousValues = @{}
    try {
        # O Compose prioriza o terminal; garantir que ele use o mesmo .env da API.
        foreach ($key in @('POSTGRES_DB', 'POSTGRES_USER', 'POSTGRES_PASSWORD', 'POSTGRES_PORT', 'POSTGRES_CONTAINER_NAME')) {
            $previousValues[$key] = [Environment]::GetEnvironmentVariable($key, 'Process')
            [Environment]::SetEnvironmentVariable($key, $Values[$key], 'Process')
        }
        & docker compose --env-file $EnvironmentFile up -d --wait --wait-timeout 120 postgres
        if ($LASTEXITCODE -ne 0) { throw 'PostgreSQL nao iniciou saudavel. Verifique o Docker, a porta configurada e os logs do container.' }
    }
    finally {
        foreach ($key in $previousValues.Keys) { [Environment]::SetEnvironmentVariable($key, $previousValues[$key], 'Process') }
    }
}
