# Padrão de plano de implementação

Todo plano detalhado do VetCommission deve ser executável sem depender de interpretação implícita.

## Cabeçalho obrigatório

- Objetivo verificável.
- Arquitetura resumida.
- Stack afetada.
- Requisitos e especificações de origem.
- Pré-requisitos aprovados.
- Fora do escopo.
- Restrições globais aplicáveis.

## Estrutura obrigatória de cada tarefa

1. **Arquivos:** caminhos exatos a criar, modificar e testar.
2. **Interfaces:** contratos consumidos e produzidos.
3. **Teste que falha:** comportamento específico e falha esperada.
4. **Implementação mínima:** somente o necessário para o teste.
5. **Verificação:** comando exato e resultado esperado.
6. **Revisão:** diff, isolamento de tenant, segurança e UX quando aplicáveis.
7. **Commit:** conjunto coeso de arquivos e mensagem vinculada à issue.

## Fluxo obrigatório para persistência

```text
script VNNN imutável
→ revisão de schemas, constraints, índices e tenant
→ aplicação pelo executor
→ segunda execução idempotente
→ validação em banco vazio
→ scaffold Npgsql
→ revisão integral do diff
→ mapper/repositório
→ build e testes com PostgreSQL real
```

## Definition of Done por fatia

- Regra de negócio e permissões cobertas.
- Nenhum dado de outro tenant ou profissional fica acessível.
- DTOs e enums C#/TypeScript sincronizados.
- Loading, vazio, sem resultado, erro, sem permissão e sucesso aplicados conforme o contexto.
- Desktop e mobile aplicáveis verificados.
- Testes unitários, integração e jornada definidos no plano executados.
- `dotnet build`, `dotnet test`, lint e build frontend executados quando os projetos existirem.
- Nenhum segredo versionado.
- Documentação e issue atualizadas.

## Proibições

- Não usar `TODO`, `TBD` ou “implementar depois” em plano aprovado.
- Não criar EF Migrations nem usar `EnsureCreated`/`EnsureDeleted` para evolução normal.
- Não colocar regra de negócio em controller, componente React ou modelo scaffoldado.
- Não adiar auditoria, testes ou estados essenciais de UX para uma fase final.
- Não avançar automaticamente para outra fatia.
