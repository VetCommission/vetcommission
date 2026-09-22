# Plano de CI/CD, testes e performance do frontend

## Objetivo

Criar uma frente isolada para automatizar a qualidade do frontend Next.js, os testes dos fluxos críticos e a detecção de regressões de bundle e performance. Esta frente não altera a decisão atual de manter o token em `localStorage`.

## Escopo

- Instalação determinística com Node.js `24.17.0` e `frontend/package-lock.json`.
- Lint, formatação e type-check.
- Testes unitários e de componentes com Vitest, Testing Library e MSW.
- Testes E2E com Playwright em ambiente isolado.
- Build de produção.
- Bundle budget por rota.
- Lighthouse/performance em ambiente de preview ou staging.
- Checks obrigatórios de Pull Request.

## Fases de execução

### Fase 1 — Linha de base

- Registrar o tamanho atual do bundle por rota.
- Registrar LCP, INP e CLS das rotas críticas.
- Identificar waterfalls e requests duplicadas.
- Inventariar Client Components e dependências pesadas.
- Definir os budgets somente após a medição inicial.

### Fase 2 — Infraestrutura de testes

- Configurar Vitest, Testing Library, `jest-dom`, `user-event` e MSW.
- Criar renderizador com MUI, providers e `QueryClient` isolado por teste.
- Adicionar scripts `test`, `test:watch`, `test:coverage` e `type-check`.
- Cobrir autenticação, tenant, guards, query keys, formulários e erros HTTP.

### Fase 3 — Fluxos E2E

- Login válido e inválido.
- Logout e expiração de sessão.
- Acesso direto sem permissão.
- Troca de tenant sem vazamento visual.
- Tentativa de acesso cross-tenant.
- Cadastro, edição e alteração de status de profissional.
- Erros de validação e falha de rede.

### Fase 4 — Workflow do GitHub Actions

O workflow deve executar, nesta ordem:

```text
npm ci
npm run lint
npm run format:check
npm run type-check
npm test -- --run
npm run build
```

Os testes E2E devem ficar em job separado quando dependerem de API, banco ou fixtures efêmeras.

Requisitos do workflow:

- `permissions: contents: read`.
- Node fixado em `24.17.0`.
- Cache baseado no `frontend/package-lock.json`.
- Nenhum segredo exposto em logs ou para Pull Requests não confiáveis.
- Nenhum `continue-on-error` nos gates.
- Falha do workflow quando qualquer etapa falhar.

### Fase 5 — Performance automatizada

- Adicionar análise de bundle em job próprio.
- Comparar o bundle com o budget versionado.
- Falhar o job quando houver regressão acima do limite acordado.
- Executar Lighthouse somente em ambiente controlado de preview/staging.
- Guardar relatórios como artefatos apenas quando necessário.
- Não aplicar lazy loading, virtualização ou memoização sem evidência da medição.

## Checks obrigatórios antes do merge

- `frontend-lint`.
- `frontend-format`.
- `frontend-typecheck`.
- `frontend-unit`.
- `frontend-build`.
- `frontend-e2e` para mudanças que afetem autenticação, autorização, tenant ou fluxos críticos.
- `frontend-performance` quando o workflow de budget estiver estabilizado.

## Critérios de aceite

- O pipeline parte de workspace limpo usando instalação determinística.
- Nenhum teste depende de API real ou produção.
- Os testes de tenant não exibem dados de outra clínica.
- O build e os gates falham quando houver regressão.
- Os budgets de performance são baseados em medições reais.
- Os checks essenciais estão configurados como obrigatórios na proteção da branch.

## Fora do escopo desta frente

- Migração do token para cookie `HttpOnly`.
- Alterações de produto em tabelas de pesquisa ou toast.
- Otimizações especulativas sem medição.
- Execução contra produção.
