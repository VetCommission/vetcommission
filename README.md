# VetCommission

SaaS multi-tenant para controlar produção, cálculo, conferência, contestação e fechamento de comissões de profissionais de clínicas veterinárias.

## Comece por aqui

1. Leia [ARQUITETURA.MD](ARQUITETURA.MD), contrato global da aplicação.
2. Leia [ARQUITETURA_POSTGRES_DB.MD](ARQUITETURA_POSTGRES_DB.MD), perfil ativo de persistência.
3. Consulte o [pacote de requisitos do MVP](Requisitos/VETCOM_MVP_UI_UX_V1/README.md).
4. Consulte o [índice da documentação](docs/README.md).
5. Para planejar ou executar o MVP, use o [roadmap de implementação](docs/planos/mvp/README.md).

## Estado atual

O repositório contém os contratos de arquitetura, requisitos de produto/UI/UX e a fundação local do PostgreSQL. Os projetos .NET e Next.js ainda serão criados incrementalmente depois da aprovação das decisões arquiteturais bloqueantes.

## Infraestrutura local existente

- PostgreSQL 17 em Docker Compose.
- Schemas `core` e `business`.
- Controle de scripts em `core.database_version`.
- Executor, backup e restore em `scriptsPS/`.
- Scripts evolutivos imutáveis em `database/scripts/`.

Consulte [scriptsPS/README.md](scriptsPS/README.md) para os comandos disponíveis.

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
