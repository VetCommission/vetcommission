# Scripts PowerShell do VetCommission

Execute os scripts a partir da raiz do repositório. Pré-requisitos:

- .NET SDK 10;
- Node.js `24.17.0`;
- Docker Desktop;
- arquivo `.env` local baseado em `.env.example`.

## Iniciar os componentes

Em terminais separados:

```powershell
.\scriptsPS\Start-Api.ps1
.\scriptsPS\Start-Worker.ps1
.\scriptsPS\Start-Frontend.ps1
```

Para iniciar PostgreSQL, API, Worker e frontend de uma vez:

```powershell
.\scriptsPS\Start-All.ps1
```

O `Start-All.ps1` aguarda o health check do PostgreSQL antes de iniciar os demais componentes. A API usa `http://localhost:5077` e o frontend usa `http://localhost:3000`.

## Validar o frontend

```powershell
.\scriptsPS\Build-Frontend.ps1
```

O script executa `npm ci`, lint e build usando a versão Node definida em `.node-version`.

## Diagnóstico rápido

```powershell
docker compose ps
docker logs vetcommission-postgres
dotnet --version
node --version
```

Os scripts falham cedo quando um executável, projeto ou versão obrigatória não está disponível. Nenhum segredo é armazenado neles.

## Banco de dados

Execute os scripts a partir da raiz do repositorio, com o Docker Desktop aberto e o container `vetcommission-postgres` em execucao.

## Gerar backup

```powershell
.\scriptsPS\Backup-VetCommission.ps1
```

O script cria na pasta `backup`:

- `vetcommission_yyyyMMdd_HHmmss.backup`
- `vetcommission_yyyyMMdd_HHmmss.backup.sha256`

O arquivo `.backup` contem a estrutura e os dados. O `.sha256` permite detectar alteracoes ou corrupcao.

## Restaurar o backup mais recente

```powershell
.\scriptsPS\Restore-VetCommission.ps1
```

O restore seleciona automaticamente o `.backup` mais recente e exige a confirmacao:

```text
RESTAURAR vetcommission
```

O banco atual e removido e recriado. Conexoes abertas no DBeaver serao encerradas.

## Restaurar um arquivo especifico

```powershell
.\scriptsPS\Restore-VetCommission.ps1 -BackupFile ".\backup\vetcommission_20260917_120000.backup"
```

## Automacao sem confirmacao interativa

Use `-Force` apenas em automacoes controladas:

```powershell
.\scriptsPS\Restore-VetCommission.ps1 -Force
```

## Preparacao do repositorio

Mantenha os backups fora do Git adicionando ao `.gitignore`:

```gitignore
backup/
```

