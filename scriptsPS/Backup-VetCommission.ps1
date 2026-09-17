[CmdletBinding()]
param(
    [string]$ContainerName = "vetcommission-postgres",
    [string]$DatabaseName = "vetcommission",
    [string]$DatabaseUser = "vetcommission"
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
$null = New-Item -ItemType Directory -Force -Path $backupDirectory

$timestamp = Get-Date -Format "yyyyMMdd_HHmmss"
$backupFileName = "${DatabaseName}_${timestamp}.backup"
$containerBackupPath = "/tmp/$backupFileName"
$localBackupPath = Join-Path $backupDirectory $backupFileName
$checksumPath = "$localBackupPath.sha256"

Write-Host "Gerando backup de '$DatabaseName'..." -ForegroundColor Cyan

try {
    & docker exec $ContainerName pg_dump `
        --username=$DatabaseUser `
        --dbname=$DatabaseName `
        --format=custom `
        --no-owner `
        --no-privileges `
        --file=$containerBackupPath
    Assert-NativeCommandSucceeded -Operation "Backup do banco '$DatabaseName'"

    # Le o catalogo do arquivo para detectar backups invalidos antes da copia.
    $null = & docker exec $ContainerName pg_restore --list $containerBackupPath
    Assert-NativeCommandSucceeded -Operation "Validacao do backup"

    & docker cp "${ContainerName}:$containerBackupPath" $localBackupPath
    Assert-NativeCommandSucceeded -Operation "Copia do backup para o Windows"

    $backupFile = Get-Item -LiteralPath $localBackupPath
    if ($backupFile.Length -le 0) {
        throw "O arquivo de backup foi criado vazio."
    }

    $hash = Get-FileHash -LiteralPath $localBackupPath -Algorithm SHA256
    Set-Content -LiteralPath $checksumPath -Value $hash.Hash -Encoding ASCII

    Write-Host "Backup concluido com sucesso." -ForegroundColor Green
    Write-Host "Arquivo : $localBackupPath"
    Write-Host "Tamanho : $($backupFile.Length) bytes"
    Write-Host "SHA-256 : $($hash.Hash)"
}
finally {
    & docker exec $ContainerName rm -f $containerBackupPath 2>$null | Out-Null
}

