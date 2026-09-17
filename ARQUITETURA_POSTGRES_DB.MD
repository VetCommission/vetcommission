# Perfil de Banco de Dados — PostgreSQL

> Perfil ativo de persistencia do VetCommission. Este documento complementa `ARQUITETURA.MD` e nao substitui suas regras de aplicacao.

## Regra de Leitura

Antes de qualquer tarefa que envolva banco, persistencia, EF Core, Worker, concorrencia, testes de integracao, backup ou restore, o Codex deve ler:

1. `ARQUITETURA.MD`;
2. `ARQUITETURA_POSTGRES_DB.MD`;
3. os scripts existentes em `database/scripts`;
4. o requisito funcional da tarefa.

Em caso de divergencia, a implementacao deve parar ate que os documentos sejam corrigidos.

## Stack Ativa

- PostgreSQL 17;
- imagem local `postgres:17-alpine`;
- Docker Compose;
- Npgsql Entity Framework Core Provider;
- EFCore.NamingConventions;
- Microsoft.EntityFrameworkCore.Design;
- Microsoft.EntityFrameworkCore.Tools;
- Testcontainers for .NET com PostgreSQL real;
- PowerShell para automacao local;
- DBeaver para inspecao e diagnostico.

## Configuracao Local

O banco local executa em container, persiste dados em volume nomeado e expoe somente a porta local configurada.

Variaveis esperadas no `.env`:

```env
POSTGRES_DB=vetcommission
POSTGRES_USER=vetcommission
POSTGRES_PASSWORD=<senha-local>
POSTGRES_CONTAINER_NAME=vetcommission-postgres
```

Regras:

- `.env` nao e versionado;
- `.env.example` contem apenas valores seguros de exemplo;
- senha nao aparece em script, log, commit ou documentacao real;
- scripts PowerShell leem banco e usuario do `.env` da raiz;
- antes de alterar o banco, o executor exibe arquivo de ambiente, container e banco;
- a confirmacao obrigatoria e `ATUALIZAR <nome-do-banco>`;
- nao existe parametro de bypass para essa confirmacao.

Comandos locais:

```powershell
docker compose up -d postgres
docker compose ps
docker compose logs postgres
```

O container precisa estar `healthy` antes da API, Worker, scripts ou testes dependentes do banco.

## Schemas Fisicos

O PostgreSQL usa somente dois schemas de aplicacao:

### `core`

Fundacao reutilizavel por produtos derivados da arquitetura:

- `core.database_version`;
- tenant;
- identidade, perfis e permissoes;
- jobs persistentes;
- Outbox;
- notificacoes;
- auditoria tecnica.

### `business`

Objetos especificos do dominio do VetCommission:

- clinica;
- profissional;
- procedimento;
- regra de comissao;
- ciclo;
- calculo e resultado;
- contestacao.

Objetos da aplicacao nao sao criados em `public`. Scripts e consultas sempre qualificam o schema.

## Convencoes de Nomes

- schemas, tabelas, colunas, indices e constraints em ingles e `snake_case`;
- tabelas no singular;
- sem prefixos como `tb_`;
- primary key: `pk_<tabela>`;
- unique constraint: `uk_<tabela>_<colunas>`;
- foreign key: `fk_<origem>_<destino>`;
- indice: `ix_<tabela>_<colunas>`;
- a versao e o nome de cada script sao unicos.

Exemplos:

```text
core.database_version
core.tenant
business.clinic
business.commission_rule
pk_commission_rule
fk_clinic_tenant
ix_commission_rule_tenant_id_active
```

## Mapeamento de Tipos

| Conceito | PostgreSQL | .NET |
|---|---|---|
| Identificador | `uuid` | `Guid` |
| Instante global | `timestamptz` | `DateTime` UTC ou `DateTimeOffset` |
| Data sem horario | `date` | `DateOnly` |
| Booleano | `boolean` | `bool` |
| Monetario | `numeric(18,2)` ou precisao do dominio | `decimal` |
| Inteiro | `integer` / `bigint` | `int` / `long` |
| Texto limitado | `varchar(n)` | `string` |
| Texto longo | `text` | `string` |
| Semiestruturado | `jsonb` | tipo explicito ou JSON |

Regras:

- timestamps globais sao tratados em UTC;
- valores monetarios nunca usam ponto flutuante;
- strings possuem tamanho quando o dominio conhece o limite;
- `jsonb` nao substitui modelagem relacional normal;
- enums persistidos como inteiro exigem conversao explicita e valores estaveis;
- FKs e comportamento de exclusao sao sempre declarados;
- defaults importantes pertencem ao banco e aparecem no script;
- collation e case sensitivity sao consideradas em consultas textuais.

## Database First

Fluxo obrigatorio:

```text
novo script PostgreSQL
-> revisao
-> backup quando a mudanca for relevante
-> execucao pelo executor
-> validacao de constraints e indices
-> scaffold Npgsql
-> revisao do diff
-> mappers e repositorios
-> build e testes
```

Nao usar EF Core Migrations, `EnsureCreated` ou `EnsureDeleted` para evoluir o schema normal.

## Estrutura de Arquivos

```text
database/
  scripts/
    V001__create_database_version_table.sql
    V002__remove_legacy_public_database_version.sql
    V003__create_tenant_table.sql
scriptsPS/
  ExecutarScriptsDB.ps1
  Scaffold-EF.ps1
  Backup-VetCommission.ps1
  Restore-VetCommission.ps1
  Unblock-PowerShellScripts.ps1
docker-compose.yml
.env.example
```

Todos os `.ps1` ficam em `scriptsPS`. `database/scripts` recebe somente SQL evolutivo.

## Convencao de Scripts

Formato obrigatorio:

```text
VNNN__descricao_da_alteracao.sql
```

Regras:

- `V` identifica versao;
- `NNN` e sequencial com tres digitos, iniciado em `001`;
- `__` separa versao e descricao;
- descricao em ingles, minuscula e `snake_case`;
- nao ha salto, reutilizacao ou renumeracao de versao compartilhada;
- script aplicado nunca e alterado;
- correcao usa nova versao;
- scripts nao declaram `BEGIN`, `COMMIT` ou `ROLLBACK`;
- o executor controla a transacao da alteracao e do registro da versao;
- operacao nao transacional exige decisao arquitetural explicita e script dedicado;
- mudanca destrutiva exige backup, justificativa e validacao de dados.

## Controle de Versoes

Fonte de historico:

```text
core.database_version
```

O executor registra:

- versao;
- descricao;
- nome do arquivo;
- SHA-256 normalizado;
- usuario do banco;
- data UTC de aplicacao.

Comportamento:

- script registrado com o mesmo hash e ignorado;
- script registrado com hash diferente interrompe a execucao;
- scripts pendentes executam em ordem crescente;
- falha no SQL desfaz a alteracao e o registro;
- V001 cria `core`, `business` e `core.database_version`;
- estados inesperados nao sao corrigidos silenciosamente;
- reparos de dados ou schema usam nova versao explicita.

Execucao:

```powershell
.\scriptsPS\ExecutarScriptsDB.ps1
```

O operador precisa conferir o destino e digitar exatamente:

```text
ATUALIZAR vetcommission
```

## Scaffold do EF Core

Pacotes:

```text
Npgsql.EntityFrameworkCore.PostgreSQL
EFCore.NamingConventions
Microsoft.EntityFrameworkCore.Design
Microsoft.EntityFrameworkCore.Tools
```

Comando equivalente:

```powershell
dotnet ef dbcontext scaffold `
  "Host=localhost;Port=5432;Database=vetcommission;Username=vetcommission;Password=<senha-local>" `
  Npgsql.EntityFrameworkCore.PostgreSQL `
  --project src\<NomeProjeto>.Infrastructure `
  --context <NomeProjeto>DbContext `
  --context-dir Persistence\Generated `
  --output-dir Persistence\Generated\Entities `
  --schema core `
  --schema business `
  --no-onconfiguring `
  --force
```

O comando real e encapsulado por `scriptsPS/Scaffold-EF.ps1`, que le configuracao local sem registrar senha no codigo ou log.

Regras:

- gerar somente `core` e `business`;
- arquivos gerados ficam em `Infrastructure/Persistence/Generated`;
- usar `--no-onconfiguring`;
- nunca editar arquivo gerado para incluir regra de negocio;
- extensoes usam partials ou arquivos separados;
- revisar integralmente o diff depois do `--force`;
- remocao inesperada interrompe a tarefa;
- configuracao adicional nao deve contradizer constraints do banco.

Registro do provider:

```csharp
services.AddDbContext<<NomeProjeto>DbContext>(options =>
    options
        .UseNpgsql(connectionString, npgsql =>
            npgsql.EnableRetryOnFailure())
        .UseSnakeCaseNamingConvention());
```

Retry do provider atende falhas transitorias de conexao. Ele nao autoriza repetir automaticamente operacoes de negocio nao idempotentes.

## Multitenancy

- tabelas isoladas possuem `tenant_id uuid NOT NULL`;
- FKs devem impedir vinculos entre tenants quando aplicavel;
- indices de consultas multi-tenant normalmente iniciam por `tenant_id`;
- todas as queries de negocio filtram tenant explicitamente;
- constraints de unicidade incluem `tenant_id` quando a unicidade e por tenant;
- testes comprovam que um tenant nao acessa dados de outro;
- Row-Level Security pode ser adotado como defesa adicional, nunca como substituto da validacao da aplicacao;
- ativar RLS exige script, politica, testes e definicao segura do contexto da conexao.

## Worker, Jobs e Concorrencia

Na primeira versao, o PostgreSQL pode atuar como fila persistente.

Requisitos:

- reserva atomica do job;
- estado pendente, processando, concluido ou falho;
- `tenant_id`, tentativas, proxima tentativa e diagnostico;
- idempotencia por chave de negocio;
- timeout e recuperacao de jobs abandonados;
- Outbox na mesma transacao do resultado de negocio;
- Slack processado depois do commit e sem desfazer o calculo.

Para consumidores concorrentes, usar uma estrategia revisada com bloqueio de linha, normalmente:

```sql
FOR UPDATE SKIP LOCKED
```

A query real deve ser validada com concorrencia e plano de execucao. Nao manter transacao aberta durante chamadas externas ou calculos demorados.

## Repositorios e Consultas

- leitura sem alteracao usa `AsNoTracking()`;
- repositorio nao expoe `IQueryable` para `Application`;
- filtros incluem tenant quando aplicavel;
- paginação tem ordenacao deterministica;
- includes sao usados somente quando necessarios;
- SQL manual usa parametros;
- operacoes compostas usam transacao coerente com `IUnitOfWork`;
- comandos idempotentes documentam sua chave de idempotencia.

## Indices e Performance

- todo indice tem nome explicito;
- indice nasce de consulta e regra reais, nao de suposicao;
- FKs usadas em filtros ou joins sao avaliadas para indexacao;
- indices multi-tenant consideram `tenant_id` como primeira coluna;
- evitar indices redundantes ou de baixa seletividade sem evidencia;
- fluxos criticos sao analisados com:

```sql
EXPLAIN (ANALYZE, BUFFERS)
```

`ANALYZE` executa a consulta. Em comandos de escrita ou ambiente sensivel, usar plano seguro e dados controlados.

## Testes de Integracao

- usar PostgreSQL real com Testcontainers for .NET;
- aplicar todos os scripts em banco vazio antes da suite;
- nao usar EF Core InMemory para SQL, constraints, concorrencia ou isolamento;
- testar PK, FK, UK, defaults e comportamento de exclusao;
- testar vazamento entre tenants;
- testar concorrencia do Worker;
- testar que scripts aplicados sao ignorados;
- testar que hash divergente interrompe o executor;
- testar restauracao periodicamente fora do banco principal.

## Backup e Restore

Desenvolvimento local:

```powershell
.\scriptsPS\Backup-VetCommission.ps1
.\scriptsPS\Restore-VetCommission.ps1 -DatabaseName vetcommission_restore_test
```

Regras:

- backup usa `pg_dump` em formato custom;
- validar o arquivo com `pg_restore --list`;
- gerar SHA-256 do backup;
- restore encerra conexoes somente do banco de destino;
- restore remove e recria somente o banco confirmado;
- nome do banco de restore deve ser explicito;
- confirmacao mostra o banco que sera substituido;
- primeiro teste de restore usa banco separado;
- backup local nao substitui retencao externa, criptografia e monitoramento de producao.

## DBeaver

O DBeaver e ferramenta de inspecao, consulta e diagnostico. Alteracao estrutural manual pelo DBeaver nao substitui script versionado.

Usos permitidos:

- consultar dados;
- inspecionar schemas, constraints e indices;
- executar diagnosticos;
- validar scripts durante a tarefa;
- analisar planos de execucao.

Mudanca confirmada deve existir em `database/scripts` e ser aplicada pelo executor.

## Criterios de Conclusao de Tarefa de Banco

- novo script possui proxima versao valida;
- backup foi gerado quando necessario;
- executor exibiu e confirmou o banco correto;
- todos os scripts pendentes foram aplicados;
- segunda execucao ignorou scripts aplicados;
- `core.database_version` contem versao e hash;
- objetos foram criados em `core` ou `business`;
- constraints e indices foram verificados;
- scaffold foi executado depois do banco;
- diff gerado foi revisado;
- build e testes passaram;
- nenhum segredo foi versionado;
- documentacao foi atualizada quando surgiu regra global nova.

## O Que Pertence Somente a Este Perfil

- PostgreSQL e Npgsql;
- tipos e sintaxe PostgreSQL;
- Docker da imagem PostgreSQL;
- `pg_dump` e `pg_restore`;
- `EXPLAIN (ANALYZE, BUFFERS)`;
- `FOR UPDATE SKIP LOCKED`;
- Row-Level Security do PostgreSQL;
- comandos de scaffold e Testcontainers PostgreSQL.

Camadas, casos de uso, frontend, autorizacao, Worker e regras de negocio permanecem em `ARQUITETURA.MD`.
