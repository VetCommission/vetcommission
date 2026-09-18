[CmdletBinding()]
param(
    [string]$EnvironmentFile,
    [string]$ContainerName
)

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

$executeDatabaseScriptsPath = Join-Path $PSScriptRoot 'ExecutarScriptsDB.ps1'
$scaffoldEfPath = Join-Path $PSScriptRoot 'Scaffold-EF.ps1'

if (-not (Test-Path -LiteralPath $executeDatabaseScriptsPath -PathType Leaf)) {
    throw "Script nao encontrado: $executeDatabaseScriptsPath"
}

if (-not (Test-Path -LiteralPath $scaffoldEfPath -PathType Leaf)) {
    throw "Script nao encontrado: $scaffoldEfPath"
}

$databaseArguments = @()
if (-not [string]::IsNullOrWhiteSpace($EnvironmentFile)) {
    $databaseArguments += @('-EnvironmentFile', $EnvironmentFile)
}
if (-not [string]::IsNullOrWhiteSpace($ContainerName)) {
    $databaseArguments += @('-ContainerName', $ContainerName)
}

$scaffoldArguments = @()
if (-not [string]::IsNullOrWhiteSpace($EnvironmentFile)) {
    $scaffoldArguments += @('-EnvironmentFile', $EnvironmentFile)
}

Write-Host 'Sincronizando banco de dados...' -ForegroundColor Cyan
& $executeDatabaseScriptsPath @databaseArguments
if ($LASTEXITCODE -ne 0) {
    throw "Sincronizacao do banco falhou com codigo $LASTEXITCODE."
}

Write-Host ''
Write-Host 'Executando scaffold EF Core...' -ForegroundColor Cyan
& $scaffoldEfPath @scaffoldArguments
if ($LASTEXITCODE -ne 0) {
    throw "Scaffold EF Core falhou com codigo $LASTEXITCODE."
}

Write-Host ''
Write-Host 'Banco sincronizado e scaffold EF Core concluido.' -ForegroundColor Green
