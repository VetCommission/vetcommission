# Padrao de schemas

O VetCommission usa somente dois schemas de aplicacao:

## `core`

Objetos que podem ser reutilizados praticamente sem alteracao em outro produto:

- versionamento do banco;
- tenant;
- usuarios, perfis e permissoes;
- outbox e jobs;
- notificacoes;
- auditoria tecnica.

Exemplos: `core.database_version`, `core.tenant`, `core.user`, `core.outbox_message`.

## `business`

Objetos que existem por causa do dominio do VetCommission:

- clinicas;
- profissionais;
- procedimentos;
- regras de comissao;
- ciclos e calculos;
- resultados e contestacoes.

Exemplos: `business.clinic`, `business.professional`, `business.commission_rule`.

## Regra de decisao

Pergunte: este objeto poderia ser reaproveitado praticamente sem alteracao em outro sistema criado com esta arquitetura?

- Sim: `core`.
- Nao: `business`.

## Nomenclatura

- tabelas no singular;
- nomes em ingles e `snake_case`;
- sem prefixos como `tb_`;
- objetos sempre qualificados pelo schema;
- PK: `pk_<tabela>`;
- UK: `uk_<tabela>_<colunas>`;
- FK: `fk_<origem>_<destino>`;
- indice: `ix_<tabela>_<colunas>`.

Nenhuma tabela da aplicacao deve ser criada em `public`.
