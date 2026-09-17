# Scripts PowerShell do PostgreSQL

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

