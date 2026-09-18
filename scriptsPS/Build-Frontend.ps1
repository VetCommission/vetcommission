[CmdletBinding()]
param()

$ErrorActionPreference = 'Stop'
$repoRoot = Split-Path -Parent $PSScriptRoot
$frontendPath = Join-Path $repoRoot 'frontend'
$requiredNodeVersion = 'v24.17.0'
$requiredNodeVersionWithoutPrefix = $requiredNodeVersion.TrimStart('v')

if (-not (Get-Command node -ErrorAction SilentlyContinue)) {
    throw 'Node.js não encontrado no PATH.'
}

if (-not (Get-Command npm.cmd -ErrorAction SilentlyContinue)) {
    throw 'npm não encontrado no PATH.'
}

function Update-NodePath {
    $machinePath = [Environment]::GetEnvironmentVariable('Path', 'Machine')
    $userPath = [Environment]::GetEnvironmentVariable('Path', 'User')
    $env:Path = "$machinePath;$userPath"

    $nvmSymlink = $env:NVM_SYMLINK
    if (-not $nvmSymlink) {
        $nvmSymlink = [Environment]::GetEnvironmentVariable('NVM_SYMLINK', 'User')
    }
    if (-not $nvmSymlink) {
        $nvmSymlink = [Environment]::GetEnvironmentVariable('NVM_SYMLINK', 'Machine')
    }

    if ($nvmSymlink -and (Test-Path -LiteralPath $nvmSymlink -PathType Container)) {
        $env:Path = "$nvmSymlink;$env:Path"
    }
}

function Get-CurrentNodeVersion {
    if (-not (Get-Command node -ErrorAction SilentlyContinue)) {
        return $null
    }

    return node --version
}

$currentNodeVersion = Get-CurrentNodeVersion
if ($currentNodeVersion -ne $requiredNodeVersion) {
    Write-Host "Node.js atual: $currentNodeVersion. Tentando ativar $requiredNodeVersion..."

    $fnmCommand = Get-Command fnm -ErrorAction SilentlyContinue
    $nvmCommand = Get-Command nvm -ErrorAction SilentlyContinue
    $voltaCommand = Get-Command volta -ErrorAction SilentlyContinue

    if ($fnmCommand) {
        Push-Location $frontendPath
        try {
            fnm env --use-on-cd | Out-String | Invoke-Expression
            fnm use $requiredNodeVersionWithoutPrefix
            if ($LASTEXITCODE -ne 0) {
                throw "fnm não conseguiu ativar Node.js $requiredNodeVersion."
            }
        }
        finally {
            Pop-Location
        }
    }
    elseif ($nvmCommand) {
        nvm use $requiredNodeVersionWithoutPrefix
        Update-NodePath
        $currentNodeVersion = Get-CurrentNodeVersion
        if ($currentNodeVersion -ne $requiredNodeVersion) {
            Write-Host "Node.js $requiredNodeVersion não está instalado no nvm. Instalando..."
            nvm install $requiredNodeVersionWithoutPrefix
            if ($LASTEXITCODE -ne 0) {
                throw "nvm não conseguiu instalar Node.js $requiredNodeVersion."
            }

            nvm use $requiredNodeVersionWithoutPrefix
            if ($LASTEXITCODE -ne 0) {
                throw "nvm instalou, mas não conseguiu ativar Node.js $requiredNodeVersion."
            }

            Update-NodePath
        }
    }
    elseif ($voltaCommand) {
        volta install "node@$requiredNodeVersionWithoutPrefix"
        if ($LASTEXITCODE -ne 0) {
            throw "volta não conseguiu instalar/ativar Node.js $requiredNodeVersion."
        }
    }
    else {
        throw "Node.js $requiredNodeVersion é obrigatório. Versão atual: $currentNodeVersion. Instale a versão correta com nvm install $requiredNodeVersionWithoutPrefix ou instale fnm/volta."
    }

    $currentNodeVersion = Get-CurrentNodeVersion
    if ($currentNodeVersion -ne $requiredNodeVersion) {
        throw "Node.js $requiredNodeVersion é obrigatório. Versão atual após ativação: $currentNodeVersion."
    }
}

Push-Location $frontendPath
try {
    npm.cmd ci
    if ($LASTEXITCODE -ne 0) {
        throw "npm ci falhou com código $LASTEXITCODE."
    }

    npm.cmd run lint
    if ($LASTEXITCODE -ne 0) {
        throw "O lint falhou com código $LASTEXITCODE."
    }

    npm.cmd run build
    if ($LASTEXITCODE -ne 0) {
        throw "O build falhou com código $LASTEXITCODE."
    }
}
finally {
    Pop-Location
}
