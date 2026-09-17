# Atualizacao para `core` e `business`

Este pacote deve ser extraido na raiz do repositorio VetCommission, preservando a estrutura de pastas.

## Arquivos substituidos ou adicionados

- `ARQUITETURA.MD`;
- `database/scripts/V001__create_database_version_table.sql`;
- `database/scripts/V002__remove_legacy_public_database_version.sql`;
- `scriptsPS/ExecutarScriptsDB.ps1`;
- `docs/PADRAO_SCHEMAS.md`.

## Antes de executar

1. Confirme que esta na branch da VET-19.
2. Gere um backup:

```powershell
.\scriptsPS\Backup-VetCommission.ps1
```

3. Confira no `.env`:

```env
POSTGRES_DB=vetcommission
POSTGRES_USER=vetcommission
POSTGRES_CONTAINER_NAME=vetcommission-postgres
```

4. Desbloqueie o executor, se necessario:

```powershell
Unblock-File .\scriptsPS\ExecutarScriptsDB.ps1
```

## Aplicacao

Execute:

```powershell
.\scriptsPS\ExecutarScriptsDB.ps1
```

Confira o destino exibido e digite exatamente:

```text
ATUALIZAR vetcommission
```

O V001 cria `core`, `business` e `core.database_version`. O V002 remove somente a tabela legada `public.database_version`, caso exista.

## Validacao

Execute novamente o executor. O resultado deve indicar que V001 e V002 ja foram aplicados.

No DBeaver, atualize a conexao e confirme:

```sql
SELECT version,
       script_name,
       checksum_sha256,
       applied_at_utc,
       applied_by
FROM core.database_version
ORDER BY id;
```

Confirme tambem que nao existe tabela de aplicacao em `public`:

```sql
SELECT table_schema,
       table_name
FROM information_schema.tables
WHERE table_schema IN ('core', 'business', 'public')
ORDER BY table_schema, table_name;
```

## Git

Depois da validacao:

```powershell
git add ARQUITETURA.MD database scriptsPS docs
git status
```

Revise os arquivos antes do commit. Scripts aplicados nao podem ser alterados depois que forem compartilhados.
