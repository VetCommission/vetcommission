# Histórico — adoção dos schemas `core` e `business`

Este documento registra a atualização que introduziu os schemas `core` e `business`, o versionamento do banco e os scripts PowerShell relacionados. Ele é histórico; as regras vigentes estão em `ARQUITETURA.MD` e `ARQUITETURA_POSTGRES_DB.MD`.

## Arquivos envolvidos

- `ARQUITETURA.MD`;
- `database/scripts/V001__create_database_version_table.sql`;
- `database/scripts/V002__remove_legacy_public_database_version.sql`;
- `scriptsPS/ExecutarScriptsDB.ps1`;
- `docs/PADRAO_SCHEMAS.md`.

## Operação registrada

O V001 cria `core`, `business` e `core.database_version`. O V002 remove somente a tabela legada `public.database_version`, caso exista.

O executor exige confirmação explícita do banco e registra versão, arquivo, checksum, usuário e instante de aplicação. Scripts compartilhados não são alterados; correções usam uma nova versão.

## Validação histórica

```sql
SELECT version, script_name, checksum_sha256, applied_at_utc, applied_by
FROM core.database_version
ORDER BY id;
```

```sql
SELECT table_schema, table_name
FROM information_schema.tables
WHERE table_schema IN ('core', 'business', 'public')
ORDER BY table_schema, table_name;
```
