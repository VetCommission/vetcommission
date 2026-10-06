[CmdletBinding()]
param()

$ErrorActionPreference = 'Stop'
$repoRoot = Split-Path -Parent (Split-Path -Parent $PSScriptRoot)
$fixtureRoot = Join-Path $repoRoot ".tmp/prepare-tests-$([Guid]::NewGuid().ToString('N'))"
$fixtureScripts = Join-Path $fixtureRoot 'scriptsPS'
New-Item -ItemType Directory -Path $fixtureScripts -Force | Out-Null
Copy-Item -LiteralPath (Join-Path $repoRoot '.env.example') -Destination $fixtureRoot
foreach ($name in @('Prepare-Environment.ps1', 'LocalEnvironment.ps1')) {
    $source = Join-Path $repoRoot "scriptsPS/$name"
    if (Test-Path -LiteralPath $source) { Copy-Item -LiteralPath $source -Destination $fixtureScripts }
}
$prepare = Join-Path $fixtureScripts 'Prepare-Environment.ps1'
$environmentPath = Join-Path $fixtureRoot '.env'
$originalConnection = $env:ConnectionStrings__DefaultConnection

function Assert-True([bool]$Condition, [string]$Message) {
    if (-not $Condition) { throw $Message }
}

try {
    Assert-True (Test-Path -LiteralPath $prepare) 'A preparacao do ambiente ainda nao existe.'
    & $prepare -SkipDotNet -SkipFrontend -SkipDocker
    Assert-True (Test-Path -LiteralPath $environmentPath) 'Preparacao nao criou o .env.'
    $created = Get-Content -LiteralPath $environmentPath -Raw
    Assert-True ($created -notmatch 'defina_uma_senha_local') 'Senha modelo foi mantida.'
    Assert-True ($created -match 'POSTGRES_PASSWORD=[a-f0-9]{64}') 'Senha local nao foi gerada.'
    Assert-True (-not [string]::IsNullOrWhiteSpace($env:ConnectionStrings__DefaultConnection)) 'Conexao nao foi exportada.'

    # A segunda execucao precisa preservar inclusive comentarios e credenciais.
    $custom = "# preservar este arquivo`nPOSTGRES_DB=existing_db`nPOSTGRES_USER=existing_user`nPOSTGRES_PASSWORD='senha;com=delimitadores'`nPOSTGRES_HOST=127.0.0.1`nPOSTGRES_PORT=5544`n"
    [IO.File]::WriteAllText($environmentPath, $custom)
    & $prepare -SkipDotNet -SkipFrontend -SkipDocker
    Assert-True ((Get-Content -LiteralPath $environmentPath -Raw) -ceq $custom) 'Preparacao sobrescreveu o .env existente.'
    $connection = New-Object System.Data.Common.DbConnectionStringBuilder
    $connection.set_ConnectionString($env:ConnectionStrings__DefaultConnection)
    Assert-True ($connection.get_Item('Password') -ceq 'senha;com=delimitadores') 'Senha foi interpretada como opcoes da conexao.'
    Assert-True ($connection.get_Item('Port') -eq '5544') 'Porta configurada foi ignorada.'
    Assert-True ($connection.get_Item('Database') -ceq 'existing_db') 'Banco existente foi ignorado.'

    $commented = @'
POSTGRES_DB=existing_db # banco local
POSTGRES_USER='existing_user' # usuario
POSTGRES_PASSWORD="abc\\def\"ghi" # senha com escapes
'@
    [IO.File]::WriteAllText($environmentPath, $commented)
    & $prepare -SkipDotNet -SkipFrontend -SkipDocker
    $connection.set_ConnectionString($env:ConnectionStrings__DefaultConnection)
    Assert-True ($connection.get_Item('Database') -ceq 'existing_db') 'Comentario foi incluido no nome do banco.'
    Assert-True ($connection.get_Item('Username') -ceq 'existing_user') 'Comentario apos aspas foi incluido no usuario.'
    Assert-True ($connection.get_Item('Password') -ceq 'abc\def"ghi') 'Escapes de valor entre aspas nao foram decodificados.'

    [IO.File]::WriteAllText($environmentPath, "POSTGRES_DB=db`nPOSTGRES_USER=user`nPOSTGRES_PASSWORD=`n")
    $rejected = $false
    try { & $prepare -SkipDotNet -SkipFrontend -SkipDocker } catch { $rejected = $_.Exception.Message -match 'POSTGRES_PASSWORD' }
    Assert-True $rejected 'Senha vazia nao foi rejeitada.'
    Assert-True ((Get-Content -LiteralPath $environmentPath -Raw) -match 'POSTGRES_PASSWORD=\s*$') 'Arquivo invalido foi alterado silenciosamente.'
    Write-Host 'PASS: criacao, preservacao, conexao com delimitadores e rejeicao de senha vazia.'
}
finally {
    $env:ConnectionStrings__DefaultConnection = $originalConnection
    # O fixture fica em .tmp para inspecao, sem remover caminhos computados.
}
