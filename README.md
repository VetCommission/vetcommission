# VetCommission

SaaS multi-tenant para controlar produção, cálculo, conferência, contestação e fechamento de comissões de profissionais de clínicas veterinárias.

## Comece por aqui

1. Leia [ARQUITETURA.MD](ARQUITETURA.MD), contrato global da aplicação.
2. Leia [ARQUITETURA_POSTGRES_DB.MD](ARQUITETURA_POSTGRES_DB.MD), perfil ativo de persistência.
3. Consulte o [pacote de requisitos do MVP](Requisitos/VETCOM_MVP_UI_UX_V1/README.md).
4. Consulte o [índice da documentação](docs/README.md).
5. Para planejar ou executar o MVP, use o [roadmap de implementação](docs/planos/mvp/README.md).

## Estado atual

O repositório contém os contratos de arquitetura, requisitos de produto/UI/UX, a fundação local do PostgreSQL, a solução .NET 10 e o frontend Next.js. A fundação não antecipa entidades, tabelas ou telas funcionais do domínio.

## Infraestrutura local existente

- PostgreSQL 17 em Docker Compose.
- Schemas `core` e `business`.
- Controle de scripts em `core.database_version`.
- Executor, backup e restore em `scriptsPS/`.
- Scripts evolutivos imutáveis em `database/scripts/`.
- WebApi em `http://localhost:5077`, com health check em `/health`.
- Worker separado, sem jobs de negócio nesta etapa.
- Frontend Next.js em `http://localhost:3000`.

Consulte [scriptsPS/README.md](scriptsPS/README.md) para os comandos disponíveis.

## Acesso local ao sistema

Com PostgreSQL, API e frontend em execucao, abra:

```text
http://localhost:3000/login
```

O usuario inicial previsto pelo script de banco e:

```text
E-mail: admin@vetcommission.local
```

No primeiro MVP nao existe autocadastro publico nem recuperacao automatizada de senha. O usuario precisa estar previamente cadastrado, ativo e vinculado a um tenant.

### Senha inicial de desenvolvimento

Para facilitar os testes locais, o script `database/scripts/V004__provision_development_admin_password.sql` define a senha `qwas` para o administrador inicial e grava somente o hash PBKDF2 compativel com `Pbkdf2PasswordHasher`.

Essa senha e exclusiva do ambiente local e nao deve ser usada em homologacao ou producao. Em ambientes compartilhados, substitua o hash por uma credencial provisionada de forma segura.

Depois do provisionamento:

1. Inicie os servicos com `scriptsPS/01-Restart-Backend-Frontend.ps1`.
2. Acesse `http://localhost:3000/login`.
3. Informe `admin@vetcommission.local` e a senha provisionada.
4. Apos autenticar, acesse a area administrativa em `http://localhost:3000/app`.

O backend expoe os endpoints de sessao em `POST http://localhost:5077/api/auth/login` e `GET http://localhost:5077/api/auth/me`.

## Processo de desenvolvimento

Cada fatia do MVP deve seguir, quando houver persistência:

```text
requisito aprovado
→ teste/regra
→ script PostgreSQL
→ aplicação e validação do script
→ scaffold do EF Core
→ backend
→ frontend
→ testes e revisão de UX
```

Não implemente uma fase inteira a partir apenas do roadmap. Cada entrega exige um plano detalhado aprovado, com arquivos, interfaces, testes e comandos de verificação.
