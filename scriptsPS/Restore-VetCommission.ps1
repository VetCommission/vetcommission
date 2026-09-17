[CmdletBinding()]
param(
    [string]$BackupFile,
    [string]$ContainerName = "vetcommission-postgres",
    [string]$DatabaseName = "vetcommission",
    [string]$DatabaseUser = "vetcommission",
    [switch]$Force
)

Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"

function Assert-NativeCommandSucceeded {
    param([Parameter(Mandatory)][string]$Operation)

    if ($LASTEXITCODE -ne 0) {
        throw "$Operation falhou com o codigo de saida $LASTEXITCODE."
    }
}

function Assert-ValidIdentifier {
    param(
        [Parameter(Mandatory)][string]$Value,
        [Parameter(Mandatory)][string]$Name
    )

    if ($Value -notmatch '^[A-Za-z_][A-Za-z0-9_]*$') {
        throw "$Name invalido: '$Value'. Use apenas letras, numeros e sublinhado."
    }
}

Assert-ValidIdentifier -Value $DatabaseName -Name "Nome do banco"
Assert-ValidIdentifier -Value $DatabaseUser -Name "Usuario do banco"

if (-not (Get-Command docker -ErrorAction SilentlyContinue)) {
    throw "Docker nao foi encontrado no PATH. Abra o Docker Desktop e tente novamente."
}

$containerIsRunning = & docker inspect --format '{{.State.Running}}' $ContainerName 2>$null
Assert-NativeCommandSucceeded -Operation "Consulta do container '$ContainerName'"

if (($containerIsRunning | Out-String).Trim() -ne "true") {
    throw "O container '$ContainerName' nao esta em execucao. Use: docker compose up -d"
}

$projectRoot = [System.IO.Path]::GetFullPath((Join-Path $PSScriptRoot ".."))
$backupDirectory = Join-Path $projectRoot "backup"

if ([string]::IsNullOrWhiteSpace($BackupFile)) {
    $selectedBackup = Get-ChildItem -LiteralPath $backupDirectory -Filter "*.backup" -File -ErrorAction SilentlyContinue |
        Sort-Object LastWriteTime -Descending |
        Select-Object -First 1

    if (-not $selectedBackup) {
        throw "Nenhum arquivo .backup foi encontrado em '$backupDirectory'."
    }
}
else {
    $resolvedBackup = Resolve-Path -LiteralPath $BackupFile -ErrorAction Stop
    $selectedBackup = Get-Item -LiteralPath $resolvedBackup.Path
}

if ($selectedBackup.Extension -ne ".backup") {
    throw "O arquivo selecionado precisa possuir a extensao .backup."
}

$checksumPath = "$($selectedBackup.FullName).sha256"
if (Test-Path -LiteralPath $checksumPath) {
    $expectedHash = (Get-Content -LiteralPath $checksumPath -Raw).Trim().Split(' ')[0]
    $actualHash = (Get-FileHash -LiteralPath $selectedBackup.FullName -Algorithm SHA256).Hash

    if ($expectedHash -ne $actualHash) {
        throw "O checksum SHA-256 nao confere. O backup pode estar corrompido ou alterado."
    }

    Write-Host "Checksum SHA-256 validado." -ForegroundColor Green
}
else {
    Write-Warning "Arquivo de checksum nao encontrado. O formato ainda sera validado pelo pg_restore."
}

$containerBackupPath = "/tmp/$($selectedBackup.Name)"

try {
    & docker cp $selectedBackup.FullName "${ContainerName}:$containerBackupPath"
    Assert-NativeCommandSucceeded -Operation "Copia do backup para o container"

    $null = & docker exec $ContainerName pg_restore --list $containerBackupPath
    Assert-NativeCommandSucceeded -Operation "Validacao do formato do backup"

    Write-Host "Backup selecionado: $($selectedBackup.FullName)" -ForegroundColor Cyan
    Write-Warning "O banco '$DatabaseName' sera removido e recriado. Conexoes abertas, inclusive do DBeaver, serao encerradas."

    if (-not $Force) {
        $confirmation = Read-Host "Para continuar, digite exatamente: RESTAURAR $DatabaseName"
        if ($confirmation -cne "RESTAURAR $DatabaseName") {
            Write-Host "Restauracao cancelada. Nenhuma alteracao foi feita no banco." -ForegroundColor Yellow
            exit 0
        }
    }

    Write-Host "Encerrando conexoes com '$DatabaseName'..." -ForegroundColor Cyan
    $terminateConnectionsSql = "SELECT pg_terminate_backend(pid) FROM pg_stat_activity WHERE datname = '$DatabaseName' AND pid <> pg_backend_pid();"
    $null = & docker exec $ContainerName psql `
        --username=$DatabaseUser `
        --dbname=postgres `
        --set=ON_ERROR_STOP=1 `
        --command=$terminateConnectionsSql
    Assert-NativeCommandSucceeded -Operation "Encerramento das conexoes"

    & docker exec $ContainerName dropdb `
        --username=$DatabaseUser `
        --if-exists `
        $DatabaseName
    Assert-NativeCommandSucceeded -Operation "Remocao do banco de destino"

    & docker exec $ContainerName createdb `
        --username=$DatabaseUser `
        --owner=$DatabaseUser `
        $DatabaseName
    Assert-NativeCommandSucceeded -Operation "Criacao do banco de destino"

    Write-Host "Restaurando '$DatabaseName'..." -ForegroundColor Cyan
    & docker exec $ContainerName pg_restore `
        --username=$DatabaseUser `
        --dbname=$DatabaseName `
        --no-owner `
        --no-privileges `
        --exit-on-error `
        $containerBackupPath
    Assert-NativeCommandSucceeded -Operation "Restauracao do banco"

    $restoredDatabase = & docker exec $ContainerName psql `
        --username=$DatabaseUser `
        --dbname=$DatabaseName `
        --tuples-only `
        --no-align `
        --command="SELECT current_database();"
    Assert-NativeCommandSucceeded -Operation "Validacao do banco restaurado"

    if (($restoredDatabase | Out-String).Trim() -ne $DatabaseName) {
        throw "A validacao final nao retornou o banco esperado '$DatabaseName'."
    }

    Write-Host "Restauracao concluida com sucesso." -ForegroundColor Green
    Write-Host "Banco  : $DatabaseName"
    Write-Host "Backup : $($selectedBackup.FullName)"
    Write-Host "Atualize a conexao no DBeaver antes de continuar."
}
finally {
    & docker exec $ContainerName rm -f $containerBackupPath 2>$null | Out-Null
}

