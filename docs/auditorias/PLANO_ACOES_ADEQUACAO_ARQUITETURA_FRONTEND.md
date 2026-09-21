# Plano de Implementação — Adequação da Arquitetura Frontend Vetcom

> **Para execução pelo Codex:** executar este plano tarefa por tarefa, usando testes antes da implementação. Não iniciar uma fase enquanto os critérios de saída da fase anterior não forem atendidos. Manter checkboxes atualizados no próprio arquivo.

**Objetivo:** tratar integralmente os achados `FRONT-001` a `FRONT-016` da auditoria e deixar o frontend Next.js aderente ao padrão Feature-Based Architecture, App Router e abordagem Server-First, com segurança multi-tenant, contratos padronizados de listagem, testes e integração contínua. Este plano não cria novas funcionalidades de negócio; ele corrige e padroniza a base arquitetural existente.

**Arquitetura-alvo:** frontend organizado por funcionalidades, com páginas finas, Server Components por padrão e Client Components restritos às ilhas interativas. A API deve validar autorização por usuário, tenant e recurso; o estado remoto deve ser segmentado por tenant e identidade; componentes transversais devem possuir interfaces próprias e não expor bibliotecas diretamente às features.

**Stack:** Next.js 16.3.5, React 19.2.8, TypeScript 5.9.3, MUI 9.4.0, TanStack Query 5.103.1, Axios, React Hook Form, Zod, Vitest, Testing Library, MSW e Playwright.

**Fonte:** `docs/auditorias/AUDITORIA_FRONTEND_NEXTJS.md`.

## Itens explicitamente fora do escopo

- Não criar ou avaliar um componente genérico de tabela de pesquisa, `AppDataTable<T>`, Material React Table, MUI X Data Grid ou TanStack Table neste plano.
- Não evoluir, substituir ou padronizar o `ToastProvider`, nem criar fachada de toast ou central persistente de notificações.
- Quando uma tarefa exigir feedback ao usuário, usar estados acessíveis e locais da própria tela ou formulário. A escolha de uma solução transversal de notificações fica para uma decisão futura, fora desta execução.

Essas exclusões não deixam achados da auditoria sem tratamento: tabela genérica e toast eram expansões propostas, não correções obrigatórias de `FRONT-001` a `FRONT-016`.

### Paginação permanece no escopo arquitetural

A exclusão do componente visual de tabela não exclui a paginação. Todas as APIs de listagem devem adotar um contrato paginado comum, pois paginação, filtros e ordenação no servidor são responsabilidades arquiteturais dos contratos HTTP e da camada de aplicação. Este plano deve padronizar os endpoints já existentes, sem criar novas telas, novos filtros de negócio ou uma nova biblioteca visual de tabela.

## Restrições globais

- Preservar a arquitetura Clean Architecture existente no backend.
- Não confiar em `tenantId`, `userId`, perfil ou recurso enviados pelo navegador.
- Toda consulta e escrita tenant-scoped deve validar `(usuário autenticado, tenant ativo, recurso exigido)`.
- Manter TypeScript em modo `strict` e não introduzir `any`, `@ts-ignore` ou supressões genéricas.
- Usar App Router e Server Components por padrão.
- Não instalar versões incompatíveis com React 19, Next.js 16 ou MUI 9.
- Não migrar banco sem script evolutivo e sem aplicar o processo DB-First adotado pelo projeto.
- Não expor tokens, conexões, chaves ou conteúdo de `.env` em logs, testes ou relatórios.
- Não testar contra produção.
- Cada tarefa deve resultar em commit pequeno, revisável e com testes verdes.
- Alterações de autenticação, autorização e tenant devem receber revisão específica de segurança.
- APIs de listagem devem paginar no servidor, aplicar limites seguros de tamanho de página e usar ordenação determinística.
- Não carregar coleções potencialmente ilimitadas para paginar ou filtrar somente no navegador.

## Fases e bloqueios

| Fase | Conteúdo | Condição para avançar |
| --- | --- | --- |
| 0 | Reprodução e proteção inicial | Testes demonstram o comportamento atual e impedem regressão silenciosa |
| 1 | Autorização multi-tenant | Acesso cruzado retorna `403`/`404` e testes de integração passam |
| 2 | Cache, sessão e tenant | Nenhum dado sobrevive indevidamente à troca de tenant, logout ou novo login |
| 3 | Testes e CI | Pull request não passa com lint, tipos, testes ou build quebrados |
| 4 | App Router, UX e erros | Boundaries, 404, loading e páginas Server-First funcionando |
| 5 | Features, contratos de listagem e formulários | Dependências por domínio, APIs paginadas e formulários padronizados |
| 6 | Performance, acessibilidade e segurança do navegador | Requisições canceláveis, filtros controlados e auditoria básica aprovada |

## Matriz de cobertura dos achados

| Achado | Tratamento principal |
| --- | --- |
| FRONT-001 | Tarefas 1 e 2 |
| FRONT-002 | Tarefa 3 |
| FRONT-003 | Tarefa 4 |
| FRONT-004 | Tarefa 5 |
| FRONT-005 | Tarefas 1, 3, 4, 6 e 7 |
| FRONT-006 | Tarefa 8 |
| FRONT-007 | Tarefa 9 |
| FRONT-008 | Tarefa 10 |
| FRONT-009 | Tarefa 11 |
| FRONT-010 | Tarefa 13 |
| FRONT-011 | Tarefa 14 |
| FRONT-012 | Tarefa 15 |
| FRONT-013 | Tarefa 12 |
| FRONT-014 | Tarefa 4 |
| FRONT-015 | Tarefas 10 e 16 |
| FRONT-016 | Tarefa 17 |

---

## Fase 0 — Criar a rede mínima de segurança

### Tarefa 1 — Comprovar o isolamento multi-tenant por teste de integração

**Arquivos:**

- Criar testes na suíte de integração da API, seguindo o padrão existente.
- Modificar somente infraestrutura de testes necessária para criar dois tenants, dois usuários e permissões distintas.

**Cenários obrigatórios:**

- [x] Usuário do tenant A acessa recurso do tenant A e recebe sucesso.
- [x] Usuário do tenant A envia `X-Tenant-Id` do tenant B e recebe `403` ou `404`.
- [x] Usuário com recurso no tenant A, mas sem o mesmo recurso no tenant B, não acessa o tenant B.
- [x] ID de profissional do tenant B não pode ser lido, alterado, ativado ou inativado pelo tenant A.
- [x] `tenantId` ou `userId` adulterado no payload não muda o escopo da operação.
- [x] Ausência de tenant em endpoint tenant-scoped é rejeitada de forma padronizada.

**Execução:**

- [x] Escrever primeiro os testes negativos.
- [x] Executar os testes e registrar quais falham antes da correção.
- [x] Não flexibilizar asserts para acomodar o comportamento inseguro.
- [x] Confirmar que nenhuma resposta revela se um registro de outro tenant existe.

**Critério de aceite:** existe evidência automatizada e reproduzível do risco descrito no `FRONT-001`; após a Tarefa 2, todos os cenários passam.

## Fase 1 — Corrigir autorização multi-tenant

### Tarefa 2 — Corrigir autorização por usuário, tenant e recurso

**Arquivos de referência da auditoria:**

- `src/VetCommission.WebApi/Auth/HttpTenantContext.cs`
- `src/VetCommission.WebApi/Auth/JwtTokenService.cs`
- `src/VetCommission.WebApi/Auth/AccessResourceAuthorizationHandler.cs`
- `src/VetCommission.Application/DependencyInjection.cs`
- `src/VetCommission.Infrastructure/Professionals/ProfessionalRepository.cs`

**Interface de autorização esperada:**

```csharp
Task<bool> PossuiAcessoAsync(
    Guid usuarioId,
    Guid tenantId,
    string recurso,
    CancellationToken cancellationToken);
```

**Passos:**

- [x] Mapear todos os endpoints tenant-scoped e as policies utilizadas.
- [x] Fazer a resolução do tenant rejeitar valor ausente, inválido ou sem vínculo ativo.
- [x] Fazer o handler validar o recurso dentro do tenant atual, sem usar união global de recursos.
- [x] Evitar que claims agregadas entre tenants concedam acesso cruzado.
- [x] Manter filtros por `TenantId` nos repositórios como segunda camada de proteção.
- [x] Garantir que comandos não aceitem tenant arbitrário do corpo como fonte de autoridade.
- [x] Executar testes unitários de autorização e os testes de integração da Tarefa 1.
- [x] Documentar a regra em arquivo arquitetural de segurança/multi-tenancy.

**Critério de aceite:** nenhum endpoint tenant-scoped autoriza somente por recurso global; todos os testes cruzados passam; `FRONT-001` encerrado.

---

## Fase 2 — Isolar cache, identidade e sessão

**Pré-requisito de execução:** antes de alterar cache ou sessão, executar a parte de configuração da Tarefa 6 necessária para escrever e rodar os testes das Tarefas 3 e 4. A cobertura completa dos demais componentes continua na Fase 3.

### Tarefa 3 — Tornar query keys obrigatoriamente tenant-scoped

**Arquivos de referência:**

- `frontend/src/features/professionals/professionalQueryKeys.ts`
- `frontend/src/features/professionals/ProfessionalForm.tsx`
- `frontend/src/features/professional-roles/ProfessionalRolesList.tsx`
- `frontend/src/features/professional-specialties/ProfessionalSpecialtiesList.tsx`
- `frontend/src/features/auth/TenantProvider.tsx`

**Convenção obrigatória:**

```ts
export const tenantKeys = {
  root: (tenantId: string) => ["tenant", tenantId] as const,
};

export const professionalKeys = {
  all: (tenantId: string) =>
    [...tenantKeys.root(tenantId), "professionals"] as const,
  list: (tenantId: string, filters: ProfessionalFilters) =>
    [...professionalKeys.all(tenantId), "list", filters] as const,
  detail: (tenantId: string, professionalId: string) =>
    [...professionalKeys.all(tenantId), "detail", professionalId] as const,
};
```

**Passos:**

- [ ] Criar factories tipadas para todas as query keys tenant-scoped.
- [ ] Eliminar arrays literais dispersos nas features.
- [ ] Incluir `tenantId` em listas, detalhes, combos e dados mestres.
- [ ] Definir `enabled: Boolean(activeTenantId)` em queries que exigem tenant.
- [ ] Na troca, cancelar queries do tenant anterior antes de mudar a identidade ativa.
- [ ] Remover queries sensíveis do tenant anterior.
- [ ] Testar troca A → B enquanto existe request de A em andamento.
- [ ] Testar retorno B → A sem exibição transitória de dados incorretos.

**Critério de aceite:** nenhuma query tenant-scoped possui key sem tenant; testes provam ausência de cache cruzado; `FRONT-002` encerrado.

### Tarefa 4 — Unificar logout, expiração e troca de identidade

**Arquivo principal:** `frontend/src/features/auth/AuthProvider.tsx`.

**Comportamento esperado:**

```ts
async function clearAuthenticatedSession(): Promise<void> {
  await queryClient.cancelQueries();
  queryClient.clear();
  clearApiAuthentication();
  clearStoredSession();
  setAccessToken(null);
  setCurrentUser(null);
  setActiveTenantId(null);
}
```

Adaptar nomes à implementação real, mantendo uma única transição idempotente.

**Passos:**

- [ ] Centralizar logout manual, `401`, falha de `/auth/me` e expiração.
- [ ] Cancelar requests antes de limpar o cache.
- [ ] Limpar todo cache sensível, não apenas `['auth']`.
- [ ] Atualizar estados React e storages na mesma transição.
- [ ] Impedir loop de interceptors em caso de `401` durante logout.
- [ ] Testar logout seguido de login de outro usuário na mesma aba.
- [ ] Testar falha de `/auth/me` deixando `accessToken === null`.
- [ ] Testar chamada repetida da limpeza sem lançar erro.

**Critério de aceite:** nenhuma informação da sessão anterior é renderizada após logout, expiração ou novo login; `FRONT-003` e `FRONT-014` encerrados.

### Tarefa 5 — Definir e executar a migração da autenticação segura

**Gate obrigatório de decisão:** antes de alterar a autenticação, mapear a topologia de produção — domínios do frontend e da API, proxy reverso, HTTPS, CORS, estratégia de deploy e suporte a cookies. Produzir uma ADR comparando, no mínimo, cookie `HttpOnly` emitido pela API, BFF no Next.js e manutenção temporária do Bearer token com mitigadores. O Codex deve interromper esta tarefa após a ADR e solicitar aprovação da alternativa escolhida. Não implementar cookie ou BFF sem essa aprovação.

**Requisitos:**

- [ ] Access/refresh token não deve ser legível por JavaScript.
- [ ] Cookie deve usar `HttpOnly`, `Secure` em produção e `SameSite` adequado.
- [ ] Definir proteção CSRF compatível com a topologia escolhida.
- [ ] Definir renovação, expiração, revogação e logout server-side.
- [ ] Remover token do `localStorage` e migração silenciosa de valores antigos.
- [ ] Garantir que logs não registrem cookies ou tokens.
- [ ] Testar login, refresh, expiração, logout e request forjada entre origens.
- [ ] Atualizar documentação de autenticação e deploy.
- [ ] Registrar plano de reversão e compatibilidade durante a transição.

**Critério de aceite da decisão:** ADR aprovada com topologia, alternativa, riscos, CSRF, refresh, revogação, logout e rollback definidos.

**Critério de aceite da implementação:** nenhum token reutilizável está disponível em `localStorage`/`sessionStorage`; testes de login, renovação, expiração, logout e CSRF passam; `FRONT-004` encerrado.

---

## Fase 3 — Testes automatizados e integração contínua

### Tarefa 6 — Configurar testes unitários e de componentes

**Dependências propostas:** Vitest, Testing Library, `@testing-library/jest-dom`, `@testing-library/user-event`, ambiente DOM compatível e MSW.

**Passos:**

- [ ] Confirmar compatibilidade das versões com Next.js 16 e React 19.
- [ ] Adicionar scripts `test`, `test:watch`, `test:coverage` e `type-check`.
- [ ] Criar setup global de testes sem acoplar asserts a classes internas do MUI.
- [ ] Criar utilitário de renderização com MUI, QueryClient e providers mínimos.
- [ ] Usar um `QueryClient` novo por teste e desabilitar retry nos testes.
- [ ] Configurar MSW para sucesso, validação, `401`, `403`, `404`, conflito e falha de rede.
- [ ] Cobrir `AuthProvider`, `TenantProvider`, guards, query keys e formulários.
- [ ] Garantir ausência de handlers MSW não utilizados e requests não mockadas.

**Critério de aceite:** testes rodam de maneira determinística, sem API real, e cobrem os riscos de sessão/cache; parte do `FRONT-005` encerrada.

### Tarefa 7 — Configurar Playwright para fluxos críticos

**Fluxos mínimos:**

- [ ] Login válido e inválido.
- [ ] Expiração e logout.
- [ ] Acesso direto por URL sem permissão.
- [ ] Troca de tenant sem vazamento visual.
- [ ] Tentativa de acessar profissional de outro tenant.
- [ ] Cadastro e edição de profissional.
- [ ] Ativação/inativação com confirmação.
- [ ] Erros de validação e falha de rede.

**Requisitos:**

- [ ] Usar ambiente e banco efêmeros ou fixtures isoladas.
- [ ] Nunca executar contra produção.
- [ ] Preservar trace, screenshot e vídeo apenas em falha.
- [ ] Usar locators por papel, label ou texto acessível.
- [ ] Evitar waits fixos; usar expectativas com auto-wait.

**Critério de aceite:** fluxos críticos executam localmente e no CI; `FRONT-005` encerrado para o escopo atual.

### Tarefa 8 — Criar GitHub Actions e checks obrigatórios

**Arquivo:** `.github/workflows/ci.yml` ou workflows separados com nomes estáveis.

**Pipeline mínimo:**

```text
npm ci
npm run lint
npm run type-check
npm test -- --run
npm run build
npm run test:e2e
dotnet test (incluindo isolamento multi-tenant)
```

**Passos:**

- [ ] Fixar Node 24.17.0 conforme `.node-version` e `package.json`.
- [ ] Usar cache baseado em `frontend/package-lock.json`.
- [ ] Definir `permissions: contents: read` por padrão.
- [ ] Não disponibilizar segredos a PRs não confiáveis.
- [ ] Não usar `continue-on-error` nos gates.
- [ ] Separar E2E quando exigir serviços, mantendo dependências explícitas.
- [ ] Configurar checks obrigatórios na proteção da branch.

**Checks obrigatórios:**

- `frontend / lint`
- `frontend / type-check`
- `frontend / unit-and-component-tests`
- `frontend / build`
- `frontend / e2e-critical`
- `backend / tenant-isolation-integration`

**Critério de aceite:** PR com qualquer validação quebrada não pode ser integrado; `FRONT-006` encerrado.

---

## Fase 4 — App Router, Server-First e estados de interface

### Tarefa 9 — Criar boundaries de loading, erro e não encontrado

**Arquivos esperados:**

- `frontend/src/app/loading.tsx`
- `frontend/src/app/error.tsx`
- `frontend/src/app/global-error.tsx`
- `frontend/src/app/not-found.tsx`
- Boundaries específicos nos segmentos administrativos quando necessário.

**Passos:**

- [ ] Criar skeletons coerentes com as telas, sem layout shift expressivo.
- [ ] Criar erro recuperável com botão de nova tentativa.
- [ ] Criar erro global sem revelar detalhes técnicos.
- [ ] Criar página 404 com navegação segura.
- [ ] Fazer detalhe inexistente usar `notFound()` ou estado equivalente correto.
- [ ] Integrar telemetria sem dados pessoais ou tokens.
- [ ] Testar loading, exceção de renderização, falha de API e ID inexistente.

**Critério de aceite:** nenhuma rota crítica termina vazia ou somente com fallback genérico; `FRONT-007` encerrado.

### Tarefa 10 — Reduzir fronteiras de Client Components

**Páginas indicadas pela auditoria:** profissionais, novo profissional, funções/cargos, nova função/cargo, especialidades e nova especialidade.

**Passos:**

- [ ] Remover `"use client"` das páginas que apenas compõem componentes.
- [ ] Manter hooks, formulários e eventos dentro das features clientes.
- [ ] Revisar `AuthStates.tsx` e `theme.ts` sem forçar importação cliente desnecessária.
- [ ] Avaliar a rota `[id]` como página servidor com ilha cliente apenas onde houver interação.
- [ ] Substituir `router.push` por `Link` em navegação declarativa.
- [ ] Executar build e testes para detectar imports server/client inválidos.

**Critério de aceite:** páginas de composição são Server Components; interatividade continua funcional; `FRONT-008` e parte do `FRONT-015` encerrados.

---

## Fase 5 — Organização por feature, contratos de listagem e formulários

### Tarefa 11 — Padronizar APIs paginadas de listagem, cancelamento, filtros e URL

**Arquivos de referência:** `professionalsApi.ts` e `ProfessionalsList.tsx`.

**Interface esperada:**

```ts
export async function getProfessionals(
  tenantId: string,
  query: ProfessionalListQuery,
  signal?: AbortSignal,
): Promise<PaginatedResponse<Professional>>;
```

**Contrato arquitetural comum:**

```ts
export type ListQuery<TFilters = Record<string, never>> = {
  page: number;
  pageSize: number;
  search?: string;
  sortBy?: string;
  sortDirection?: "asc" | "desc";
  filters?: TFilters;
};

export type PaginatedResponse<T> = {
  items: T[];
  page: number;
  pageSize: number;
  totalItems: number;
  totalPages: number;
};
```

**Convenções HTTP:**

```http
GET /api/profissionais?page=1&pageSize=20&search=texto&sortBy=nome&sortDirection=asc
```

- `page` é baseada em `1` no contrato da API;
- `pageSize` possui valor padrão e limite máximo definidos centralmente;
- valores inválidos retornam erro de validação padronizado;
- a resposta sempre informa `items`, `page`, `pageSize`, `totalItems` e `totalPages`;
- filtros e ordenações permitidos devem ser declarados; não montar SQL a partir de nomes arbitrários;
- a ordenação deve possuir desempate estável por identificador;
- contagem e busca devem respeitar o mesmo tenant e os mesmos filtros;
- endpoints tenant-scoped devem obter o tenant do contexto autorizado;
- o banco deve executar filtro, ordenação e paginação; não materializar toda a coleção antes de aplicar `Skip`/`Take` ou equivalente.

**Passos:**

- [ ] Inventariar todos os endpoints atuais de listagem e registrar divergências de contrato.
- [ ] Criar contratos compartilhados de entrada e saída na camada adequada, sem acoplar domínio ao protocolo HTTP.
- [ ] Definir `pageSize` padrão e máximo em configuração central.
- [ ] Padronizar primeiro a listagem de profissionais e aplicar aos demais endpoints existentes.
- [ ] Aplicar paginação, filtros e ordenação na consulta ao banco antes da materialização.
- [ ] Garantir ordenação determinística mesmo quando o campo principal se repetir.
- [ ] Validar campos de ordenação por allowlist.
- [ ] Incluir página, tamanho, busca, ordenação e filtros na query key tenant-scoped.
- [ ] Receber `signal` das query functions e repassá-lo ao Axios.
- [ ] Aplicar debounce apenas à pesquisa textual, não aos filtros discretos.
- [ ] Representar página, tamanho, busca, ordenação e filtros relevantes em `searchParams`.
- [ ] Redefinir a página para `1` quando busca, filtro, ordenação ou tamanho da página mudar.
- [ ] Restaurar a tela corretamente ao navegar para trás ou compartilhar URL.
- [ ] Testar request cancelada sem exibir erro ao usuário.
- [ ] Testar digitação rápida produzindo somente a consulta final esperada.
- [ ] Testar primeira página, última página, página vazia, tamanho máximo, parâmetros inválidos e total consistente.
- [ ] Testar isolamento de tenant na contagem e nos itens retornados.
- [ ] Atualizar documentação OpenAPI/Swagger dos endpoints alterados.

**Critério de aceite:** endpoints de listagem existentes usam o contrato paginado comum; paginação, filtros e ordenação ocorrem no servidor; query keys incluem tenant e todos os parâmetros; requisições obsoletas são canceladas; o estado da listagem é reproduzível pela URL; `FRONT-009` encerrado. Nenhum componente genérico de tabela é criado nesta tarefa.

### Tarefa 12 — Separar contratos, APIs e query keys por domínio

**Estrutura-alvo:**

```text
src/features/professionals/
  api/
  components/
  hooks/
  schemas/
  types/
  query-keys/
src/features/professional-roles/
  api/
  components/
  schemas/
  types/
  query-keys/
src/features/professional-specialties/
  api/
  components/
  schemas/
  types/
  query-keys/
```

**Passos:**

- [ ] Inventariar exports de `src/types/api.ts` e consumidores.
- [ ] Mover somente contratos pertencentes a uma feature.
- [ ] Mover APIs de funções e especialidades para suas próprias features.
- [ ] Manter em `lib` apenas infraestrutura transversal e envelopes realmente compartilhados.
- [ ] Criar `index.ts` somente onde melhorar a API pública sem gerar ciclos.
- [ ] Validar o grafo `app -> features -> lib`, nunca feature irmã por detalhes internos.
- [ ] Executar type-check, testes e build após cada movimento.

**Critério de aceite:** features irmãs não importam detalhes internos de profissionais; `FRONT-013` encerrado.

### Tarefa 13 — Padronizar formulários MUI + RHF + Zod

**Componentes candidatos:** adaptadores tipados para campo textual, select e mensagens de erro, extraídos apenas quando houver repetição comprovada.

**Passos:**

- [ ] Fazer `MasterDataForm` apresentar erros Zod e da API.
- [ ] Usar estado pending para impedir submissão duplicada.
- [ ] Mapear envelope de validação da API para `setError` por campo.
- [ ] Manter mensagem geral para erros sem campo.
- [ ] Definir reset após sucesso conforme tipo de tela.
- [ ] Padronizar feedback acessível de sucesso e erro no contexto da própria tela ou formulário.
- [ ] Testar sucesso, validação local, validação da API, conflito e falha de rede.

**Critério de aceite:** todos os formulários atuais têm comportamento uniforme e acessível; `FRONT-010` encerrado.

### Tarefa 14 — Corrigir acessibilidade dos controles

**Passos:**

- [ ] Associar `InputLabel`, `labelId`, `FormControl` e `FormHelperText` aos selects.
- [ ] Associar erros com `aria-describedby` e estado inválido.
- [ ] Garantir um `h1` semântico por página.
- [ ] Validar navegação integral por teclado.
- [ ] Preservar foco após erro, fechamento de modal e navegação.
- [ ] Executar testes automatizados com axe onde compatível.
- [ ] Fazer testes Playwright por papel e label.

**Critério de aceite:** zero violação crítica/séria no escopo auditado e controles utilizáveis por teclado; `FRONT-011` encerrado.

### Tarefa 15 — Padronizar ações destrutivas e mudanças de estado

**Passos:**

- [ ] Criar diálogo compartilhado de confirmação com título, consequência e ação explícita.
- [ ] Exigir confirmação para inativação e exclusões futuras.
- [ ] Controlar pending por registro para impedir clique duplicado.
- [ ] Exibir sucesso/erro de forma acessível no contexto da própria lista ou ação.
- [ ] Desabilitar somente a ação afetada, mantendo a lista utilizável.
- [ ] Se houver atualização otimista, implementar snapshot e rollback testados.
- [ ] Testar confirmar, cancelar, falhar e repetir clique.

**Critério de aceite:** nenhuma ação destrutiva ou inativação ocorre silenciosamente; `FRONT-012` encerrado.

### Tarefa 16 — Adicionar formatação e manutenção automática

**Passos:**

- [ ] Adotar Prettier ou formatter já aprovado no projeto.
- [ ] Criar `format` e `format:check`.
- [ ] Formatar arquivos novos atualmente comprimidos em uma linha.
- [ ] Adicionar `format:check` ao CI sem modificar arquivos no pipeline.
- [ ] Preservar formatação gerada do Next.js e arquivos excluídos apropriados.

**Critério de aceite:** arquivos possuem formato estável e o CI detecta divergência; restante do `FRONT-015` encerrado.

---

## Fase 6 — Defesa em profundidade e qualidade operacional

### Tarefa 17 — Configurar headers de segurança no ponto correto

**Passos:**

- [ ] Identificar onde TLS e headers terminam: Next.js, proxy ou gateway.
- [ ] Criar CSP inicialmente em modo de relatório e observar violações legítimas.
- [ ] Configurar `frame-ancestors`, `Referrer-Policy`, `Permissions-Policy` e `X-Content-Type-Options`.
- [ ] Validar compatibilidade da CSP com Next.js, MUI, fontes e imagens reais.
- [ ] Não usar `unsafe-inline`/`unsafe-eval` sem justificativa documentada e restrita.
- [ ] Testar headers no ambiente equivalente à produção.

**Critério de aceite:** headers são emitidos por um único ponto conhecido, testados e documentados; `FRONT-016` encerrado.

### Tarefa 18 — Medir antes de otimizar

**Passos:**

- [ ] Registrar bundle por rota e principais Client Components.
- [ ] Medir Core Web Vitals em ambiente representativo.
- [ ] Identificar waterfalls e chamadas duplicadas.
- [ ] Registrar recomendações de lazy loading ou virtualização somente quando a medição justificar, sem implementá-las neste plano caso representem nova evolução funcional.
- [ ] Definir orçamento inicial de desempenho e monitorá-lo no CI quando isso puder ser feito sem adicionar funcionalidade ao produto.

**Critério de aceite:** decisões de performance são baseadas em evidência, não em micro-otimizações.

---

## Validação final obrigatória

- [ ] Executar instalação determinística a partir de workspace limpo.
- [ ] Executar lint.
- [ ] Executar `format:check`.
- [ ] Executar type-check dedicado.
- [ ] Executar testes unitários e de componentes.
- [ ] Executar testes de integração da API.
- [ ] Executar testes negativos multi-tenant.
- [ ] Executar build de produção.
- [ ] Executar Playwright em ambiente efêmero.
- [ ] Executar auditoria de acessibilidade no escopo atual.
- [ ] Confirmar que nenhum segredo foi incluído no Git.
- [ ] Confirmar branch protection com todos os checks obrigatórios.
- [ ] Atualizar `AUDITORIA_FRONTEND_NEXTJS.md` com o status de cada achado e nova nota.

## Definição de pronto da arquitetura

A arquitetura somente será considerada aderente quando:

1. `FRONT-001` a `FRONT-016` estiverem encerrados ou formalmente aceitos com justificativa, responsável e prazo.
2. Nenhum teste permitir acesso ou cache cruzado entre tenants e usuários.
3. Nenhum token reutilizável estiver acessível ao JavaScript do navegador.
4. Toda feature nova possuir contratos, API, query keys, componentes e testes no domínio correto.
5. Páginas forem Server Components por padrão, com ilhas cliente justificadas.
6. PRs forem bloqueados automaticamente por lint, formatação, tipos, testes, build e E2E crítico.
7. APIs de listagem existentes utilizarem contrato paginado comum, limites seguros e ordenação determinística no servidor.
8. Listagens e formulários apresentarem estados acessíveis de loading, erro, vazio e sucesso no contexto da própria tela.
9. A documentação arquitetural refletir fielmente a implementação validada.

## Estratégia recomendada de execução

Executar em branches/PRs independentes, nesta ordem:

1. `security/tenant-authorization`
2. `fix/tenant-session-cache-isolation`
3. `test/frontend-and-api-isolation`
4. `ci/required-quality-gates`
5. `refactor/next-server-first-boundaries`
6. `refactor/feature-contracts-and-forms`
7. `refactor/paginated-list-api-contracts`

Cada PR deve incluir testes, documentação afetada e evidência dos comandos executados. Não agrupar segurança, refatoração visual e novos componentes no mesmo PR.

## Autorização para disparo

O plano está pronto para execução faseada. O Codex deve:

1. executar uma branch/PR por item da estratégia acima;
2. iniciar pelas Fases 0 e 1;
3. respeitar os gates e interromper a Tarefa 5 após produzir a ADR, aguardando aprovação;
4. não implementar tabela genérica, nova biblioteca de tabela, evolução de toast ou central de notificações;
5. manter paginação e contratos das APIs de listagem dentro do escopo arquitetural;
6. não iniciar a fase seguinte enquanto os critérios de saída da fase atual não estiverem comprovados;
7. atualizar os checkboxes e registrar comandos/testes executados ao final de cada PR.
