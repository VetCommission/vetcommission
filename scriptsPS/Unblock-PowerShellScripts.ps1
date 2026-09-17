[CmdletBinding()]
param()

Set-StrictMode -Version Latest
$ErrorActionPreference = "Stop"

$scriptsDirectory = [System.IO.Path]::GetFullPath($PSScriptRoot)

if (-not (Test-Path -LiteralPath $scriptsDirectory -PathType Container)) {
    throw "A pasta do utilitario nao foi encontrada: $scriptsDirectory"
}

$scripts = @(
    Get-ChildItem `
        -LiteralPath $scriptsDirectory `
        -Filter "*.ps1" `
        -File `
        -Recurse |
        Sort-Object FullName
)

if ($scripts.Count -eq 0) {
    Write-Host "Nenhum arquivo .ps1 foi encontrado em '$scriptsDirectory'." -ForegroundColor Yellow
    exit 0
}

Write-Host "Pasta analisada: $scriptsDirectory" -ForegroundColor Cyan
Write-Host "Scripts encontrados: $($scripts.Count)" -ForegroundColor Cyan

$successCount = 0
$failureCount = 0

foreach ($script in $scripts) {
    try {
        Unblock-File -LiteralPath $script.FullName -ErrorAction Stop
        Write-Host "Desbloqueado: $($script.Name)" -ForegroundColor Green
        $successCount++
    }
    catch {
        Write-Warning "Falha ao desbloquear '$($script.FullName)': $($_.Exception.Message)"
        $failureCount++
    }
}

Write-Host ""
Write-Host "Processamento concluido." -ForegroundColor Cyan
Write-Host "Sucesso: $successCount" -ForegroundColor Green

if ($failureCount -gt 0) {
    Write-Host "Falhas : $failureCount" -ForegroundColor Red
    exit 1
}

Write-Host "Todos os scripts PowerShell da pasta do utilitario foram desbloqueados." -ForegroundColor Green
