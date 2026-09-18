[CmdletBinding()]
param()

$ErrorActionPreference = 'Stop'
$repoRoot = Split-Path -Parent $PSScriptRoot
$frontendPath = Join-Path $repoRoot 'frontend'
$requiredNodeVersion = 'v24.17.0'

if (-not (Get-Command node -ErrorAction SilentlyContinue)) {
    throw 'Node.js não encontrado no PATH.'
}

if (-not (Get-Command npm.cmd -ErrorAction SilentlyContinue)) {
    throw 'npm não encontrado no PATH.'
}

$currentNodeVersion = node --version
if ($currentNodeVersion -ne $requiredNodeVersion) {
    throw "Node.js $requiredNodeVersion é obrigatório. Versão atual: $currentNodeVersion."
}

if (-not (Test-Path -LiteralPath (Join-Path $frontendPath 'package.json') -PathType Leaf)) {
    throw "Frontend não encontrado: $frontendPath"
}

Push-Location $frontendPath
try {
    if (-not (Test-Path -LiteralPath 'node_modules' -PathType Container)) {
        npm.cmd ci
        if ($LASTEXITCODE -ne 0) {
            throw "npm ci falhou com código $LASTEXITCODE."
        }
    }

    npm.cmd run dev -- --hostname 0.0.0.0 --port 3000
    if ($LASTEXITCODE -ne 0) {
        throw "O frontend foi encerrado com código $LASTEXITCODE."
    }
}
finally {
    Pop-Location
}
