# Auditoria técnica do frontend

> **Nota de atualização:** as seções originais abaixo registram o diagnóstico inicial. A reavaliação corrente está documentada na seção **Reavaliação pós-adequação arquitetural** ao final deste arquivo; quando houver conflito, prevalece a reavaliação.

> Data da auditoria: 21/09/2026  
> Escopo: `frontend/`, integrações diretamente relacionadas à segurança multi-tenant na API e automação do repositório.  
> Método: inspeção estática, leitura da documentação local e da documentação instalada do Next.js 16.3.5, inventário de dependências, busca objetiva e execução dos scripts seguros disponíveis. Nenhum código, pacote, lockfile ou configuração foi alterado.

## 1. Resumo executivo

**Nota geral: 4,7/10 — parcialmente conforme, ainda inadequado para produção multi-tenant.**

A fundação possui decisões corretas: App Router real, TypeScript `strict`, organização inicial por feature, cliente Axios único, tratamento comum de erros, React Hook Form com Zod, configuração estável do `QueryClient`, tema MUI central e integração oficial do MUI com o App Router. Lint e build de produção passam na versão efetivamente instalada.

Os cinco maiores riscos são:

1. **CRÍTICO — autorização multi-tenant quebrada na API:** o header `X-Tenant-Id`, controlável pelo navegador, é usado como contexto de dados, enquanto a policy considera recursos agregados de todos os tenants do token e não valida o vínculo usuário/tenant a cada request.
2. **CRÍTICO — cache compartilhado entre tenants e sessões:** query keys não incluem tenant e logout remove somente queries de autenticação; dados de outro tenant/usuário podem permanecer e ser exibidos.
3. **ALTO — JWT em `localStorage`:** qualquer XSS na origem pode ler e exfiltrar o token Bearer; não há renovação de sessão observável.
4. **ALTO — ausência total de testes automatizados do frontend e dos fluxos críticos multi-tenant:** não há runner, arquivos de teste, MSW nem E2E.
5. **ALTO — ausência de CI em GitHub Actions:** nenhum workflow protege pull requests com lint, type-check, testes e build.

**Recomendação:** interromper promoção para produção e merges que ampliem dados multi-tenant até corrigir os itens de Prioridade 0. O desenvolvimento pode continuar apenas em uma frente dedicada à estabilização da base, com testes de isolamento primeiro. Funcionalidades sem relação com a correção aumentariam o custo e a superfície de risco.

## 2. Tecnologias e versões identificadas

| Tecnologia | Versão efetivamente instalada | Observações |
| --- | ---: | --- |
| Node.js | 24.17.0 | Executado localmente; coincide com `.node-version` e `package.json:5-7`. |
| npm | 11.13.0 | Gerenciador identificado por `package-lock.json`; lockfile versionado. |
| Next.js | 16.3.5 | App Router em `frontend/src/app`; versão fixa no manifesto e instalada. |
| React / React DOM | 19.2.8 | Versões fixas e alinhadas. |
| TypeScript | 5.9.3 | `strict: true`, `noEmit: true` em `frontend/tsconfig.json:7-8`. |
| MUI / MUI Next.js | 9.4.0 / 9.4.0 | Integração `@mui/material-nextjs/v16-appRouter` em `src/providers/AppProviders.tsx:5,17`. |
| Emotion | 11.14.0 / 11.14.1 | Dependência de styling do MUI. |
| TanStack Query | 5.103.1 | Provider global e cliente único. |
| Axios | 1.20.0 | Cliente HTTP centralizado. |
| React Hook Form | 7.88.0 | Usado em login e cadastros. |
| Zod | 4.6.5 | Integrado por `@hookform/resolvers` 5.9.1. |
| ESLint | 9.39.5 | Configuração Next Core Web Vitals + TypeScript. |
| Testes frontend | Ausente | Não há dependência, script, configuração ou arquivo de teste frontend. |
| GitHub Actions | Ausente | Não existe `.github/workflows`. |

Scripts disponíveis em `frontend/package.json:8-12`: `dev`, `build`, `start` e `lint`. Não existem scripts `type-check`, `test`, `test:e2e` ou `format:check`.

Estrutura real: `src/app`, `src/features`, `src/lib`, `src/providers`, `src/theme`, `src/types` e `src/utils`; `src/components` existe, mas está vazio. O roteamento é App Router com `/`, `/login`, `/app`, cadastros administrativos, rota dinâmica `/app/profissionais/[id]` e `/portal`. Não existem Route Groups.

Havia alterações locais antes da auditoria em páginas e features de profissionais, funções/cargos e especialidades, além de backend e banco. Elas foram preservadas e consideradas como trabalho em andamento; o único arquivo criado por esta auditoria é este relatório.

## 3. Resultado das validações executadas

| Validação | Comando | Resultado | Observação |
| --------- | ------- | --------- | ---------- |
| Versões do runtime | `node --version`; `npm --version` | **PASSOU** | Node 24.17.0; npm 11.13.0. |
| Dependências instaladas | `npm ls --depth=0` | **PASSOU** | Árvore de dependências de nível superior resolvida, sem pacote ausente reportado. |
| Instalação determinística | Não executado | **N/A** | `node_modules` já existia; por regra, não houve reinstalação nem risco de alterar lockfile. |
| Lint | `npm run lint` | **PASSOU** | ESLint terminou com código 0 e sem mensagens. |
| Type-check dedicado | Ausente | **NÃO CONFIGURADO** | Não existe script `type-check`; o `next build` executou TypeScript com sucesso. |
| Testes unitários/componentes | Ausente | **NÃO CONFIGURADO** | Não há runner, script ou testes frontend. |
| Testes E2E | Ausente | **NÃO CONFIGURADO** | Não há Playwright/Cypress nem fluxo E2E. |
| Build de produção | `npm run build` | **PASSOU** | Next.js 16.3.5/Turbopack compilou, validou TypeScript e gerou 13 rotas; código 0. |
| CI de pull request | Inspeção de `.github/workflows` | **AUSENTE** | Nenhum workflow encontrado. |

O build bem-sucedido comprova compilação e tipos alcançados pelo Next, mas não comprova comportamento, segurança, acessibilidade ou isolamento multi-tenant.

## 4. Avaliação por área

| Área | Situação | Nota | Evidências | Recomendação |
| ---- | -------- | ---: | ---------- | ------------ |
| Arquitetura e organização | Parcialmente conforme | 6,5 | Features reais em `src/features`; páginas geralmente compõem telas. `src/types/api.ts` centraliza domínios distintos; APIs de master data residem em `features/professionals/professionalsApi.ts:40-63` e são importadas por outras features. | Manter páginas finas, mas co-localizar API, schemas, tipos e keys por feature; definir direção de dependências. |
| App Router | Parcialmente conforme | 6,0 | `src/app/layout.tsx` e páginas do App Router são válidos; faltam `loading.tsx`, `error.tsx` e `not-found.tsx`; seis páginas são Client Components desnecessariamente. | Adicionar estados por segmento e restaurar páginas como Server Components que só compõem UI. |
| Server/Client Components | Parcialmente conforme | 5,5 | 23 arquivos com `"use client"`; componentes interativos justificam vários limites, mas páginas e tema ampliam o grafo cliente. Providers globais estão no root. | Reduzir fronteiras cliente, mantendo somente providers e ilhas interativas. |
| Integração com API | Parcialmente conforme | 6,0 | `apiClient.ts:9-35` centraliza URL, headers, timeout e erros; endpoints centralizados. Query functions não recebem `AbortSignal`; DTOs e modelos de UI estão misturados em `types/api.ts`. | Propagar cancelamento, mapear validações por campo e separar contratos externos quando houver transformação. |
| TanStack Query e estado | Não conforme | 3,0 | Keys de profissionais em `professionalQueryKeys.ts:1-4` não contêm tenant; roles/specialties usam arrays literais; logout remove apenas `['auth']` em `AuthProvider.tsx:50-55`. | Tornar tenant parte obrigatória das keys e limpar cache sensível em troca/logout. |
| Formulários | Parcialmente conforme | 5,5 | RHF+Zod e inferência no login/profissional; `MasterDataForm.tsx:9` não exibe erros, não bloqueia submissão duplicada e não trata falha da API. Selects do profissional não têm label acessível. | Padronizar formulário, erros de campo/API, pending, reset e acessibilidade. |
| TypeScript e qualidade | Parcialmente conforme | 7,0 | `strict`; zero `any`, `ts-ignore`, `eslint-disable`, `console.log`, TODO/FIXME na busca. Arquivos novos comprimidos em uma linha prejudicam manutenção. | Manter rigor e decompor/formatar componentes; adicionar type-check explícito. |
| MUI e design system | Parcialmente conforme | 7,0 | Tema central em `theme/theme.ts`; cache provider correto; tokens e responsividade básicos. Há muitos `sx` locais e ausência de variantes/layout administrativo compartilhado. | Evoluir tokens/variantes conforme repetição real e auditar contraste/foco em navegador. |
| Autenticação/autorização | Não conforme | 2,5 | Token em `localStorage`; guards somente clientes; ausência de refresh; autorização multi-tenant da API não vincula request ao tenant. | Migrar sessão para cookie seguro/BFF e corrigir enforcement servidor antes de produção. |
| Multi-tenancy | Não conforme | 1,5 | Tenant manipulável no browser (`tenantStorage.ts`, `apiClient.ts`), cache não segmentado e policy backend não valida vínculo tenant/recurso por request. | Correção P0, testes negativos e defesa em profundidade no banco/repositórios. |
| UX e acessibilidade | Parcialmente conforme | 5,5 | Login possui labels, autocomplete, alt e estados; listas possuem loading/empty/error. Faltam confirmação destrutiva, 404/error boundaries, labels de selects e feedback uniforme. | Cobrir teclado, foco, leitores de tela, mobile e falhas em testes de componente/E2E. |
| Performance | Parcialmente conforme | 6,0 | `next/image`, fontes locais e build estático; filtros disparam request a cada tecla (`ProfessionalsList.tsx:11`); 23 Client Components e providers globais. | Debounce/URL para filtros, cancelamento e medição de bundle/Web Vitals antes de otimizações adicionais. |
| Testes | Não conforme | 0,5 | Nenhum teste ou ferramenta frontend. | Criar pirâmide mínima com componentes/integração e E2E dos fluxos críticos. |
| CI/CD | Não conforme | 0,5 | Nenhum workflow. | Criar CI de PR determinístico e checks obrigatórios. |

Não foi encontrada evidência de dependência circular por inspeção do grafo atual. A base é pequena e os imports seguem predominantemente `app -> feature -> lib/types`; contudo, o compartilhamento via `features/professionals/professionalsApi.ts` por três features é uma direção inadequada e tende a gerar ciclos conforme o sistema crescer.

## 5. Achados detalhados por severidade

### FRONT-001

- **Severidade:** CRÍTICO
- **Área:** Segurança / autorização / multi-tenancy
- **Descrição:** a API usa o tenant informado pelo cliente sem comprovar, na policy de cada request, que o usuário pertence a esse tenant e possui nele o recurso exigido. O JWT agrega os recursos de todos os tenants do usuário.
- **Impacto:** um usuário com `profissionais.gerenciar` em qualquer tenant pode alterar `X-Tenant-Id` e tentar operar sobre outro tenant conhecido. Os repositórios filtram corretamente pelo tenant recebido, mas esse filtro passa a selecionar justamente o tenant arbitrário. É um caminho concreto de acesso cruzado.
- **Evidência:** `frontend/src/lib/api/apiClient.ts:24-27`; `src/VetCommission.WebApi/Auth/HttpTenantContext.cs:9-17`; `src/VetCommission.WebApi/Auth/JwtTokenService.cs:30-33`; `src/VetCommission.WebApi/Auth/AccessResourceAuthorizationHandler.cs:12-18`; `src/VetCommission.Infrastructure/Professionals/ProfessionalRepository.cs:10-43`. Existe `TenantContextValidator`, mas a busca de usos encontrou apenas registro de DI e testes, não uso no pipeline (`src/VetCommission.Application/DependencyInjection.cs:19`).
- **Correção recomendada:** vincular autorização a `(userId, tenantId, recurso)` em toda request; rejeitar tenant ausente/inválido/não associado; evitar claims de recurso sem escopo de tenant ou codificá-las por tenant; manter filtros de repositório e adicionar testes de integração cruzados.
- **Esforço:** grande
- **Bloqueia merge:** sim, para qualquer entrega multi-tenant; bloqueia produção incondicionalmente.

### FRONT-002

- **Severidade:** CRÍTICO
- **Área:** TanStack Query / multi-tenancy
- **Descrição:** dados remotos são armazenados sob keys independentes do tenant ativo.
- **Impacto:** ao trocar tenant, o cache com `staleTime` de 30 segundos pode exibir imediatamente dados do tenant anterior; respostas em voo podem preencher a mesma key após a troca. Isso é vazamento de dados entre tenants na interface.
- **Evidência:** `frontend/src/features/professionals/professionalQueryKeys.ts:1-4`; keys literais em `ProfessionalForm.tsx:33-34`, `ProfessionalRolesList.tsx:10` e `ProfessionalSpecialtiesList.tsx:10`; troca de tenant apenas muda header/storage em `TenantProvider.tsx:54-64`; `queryClient.ts:6-9` mantém dados frescos por 30 segundos.
- **Correção recomendada:** incluir `activeTenantId` em toda key tenant-scoped, desabilitar query sem tenant, cancelar/remover queries do tenant anterior antes da troca e propagar `AbortSignal` ao Axios.
- **Esforço:** médio
- **Bloqueia merge:** sim.

### FRONT-003

- **Severidade:** CRÍTICO
- **Área:** Sessão / privacidade
- **Descrição:** logout remove apenas a query `['auth']`; caches de profissionais e dados mestres permanecem no `QueryClient` global.
- **Impacto:** outro usuário que autentique na mesma aba pode receber dados cacheados da sessão anterior, inclusive de outro tenant, antes de nova busca.
- **Evidência:** `frontend/src/features/auth/AuthProvider.tsx:50-55`; provider único durante toda a vida da aplicação em `src/providers/AppProviders.tsx:13-28`; demais keys não são removidas.
- **Correção recomendada:** cancelar queries em voo e limpar todo cache sensível no logout e na mudança de identidade; reconstruir ou limpar o `QueryClient` com política explícita.
- **Esforço:** pequeno
- **Bloqueia merge:** sim.

### FRONT-004

- **Severidade:** ALTO
- **Área:** Autenticação / XSS
- **Descrição:** o access token Bearer é persistido em `localStorage`.
- **Impacto:** qualquer XSS executado na origem pode ler e exfiltrar o token, permitindo sequestro da sessão até sua expiração. `SameSite` e `HttpOnly` não podem proteger um token lido por JavaScript.
- **Evidência:** `frontend/src/features/auth/authStorage.ts:1-28`; `AuthProvider.tsx:35,67-70`; `apiClient.ts:17-20`; a decisão também está documentada em `frontend/README.md:85-93`.
- **Correção recomendada:** preferir cookie de sessão `HttpOnly`, `Secure` e `SameSite` emitido pelo backend/BFF; definir estratégia de CSRF conforme a topologia; não expor refresh token ao JavaScript.
- **Esforço:** grande
- **Bloqueia merge:** sim para autenticação destinada à produção; pode ser temporariamente aceito apenas em ambiente local isolado e documentado.

### FRONT-005

- **Severidade:** ALTO
- **Área:** Testes
- **Descrição:** não existe estratégia automatizada de testes frontend.
- **Impacto:** login, permissões, cache, formulários e isolamento podem regredir sem sinal; os três achados críticos atuais não seriam detectados antes de produção.
- **Evidência:** `frontend/package.json:8-36` não contém runner/script; inventário não encontrou `*.test.*`, `*.spec.*`, Playwright, Vitest, Jest ou MSW no frontend.
- **Correção recomendada:** configurar testes de unidade/componente e integração com mock de rede, mais Playwright para fluxos críticos; começar por isolamento de cache, guards e login.
- **Esforço:** grande
- **Bloqueia merge:** sim para mudanças de autenticação, permissão e multi-tenancy; tornar obrigatório progressivamente para o restante.

### FRONT-006

- **Severidade:** ALTO
- **Área:** CI/CD
- **Descrição:** não há GitHub Actions para pull requests.
- **Impacto:** nem mesmo lint e build, hoje verdes localmente, são garantidos antes do merge; Node e lockfile podem divergir no ambiente de integração.
- **Evidência:** ausência de `.github/workflows`; scripts limitados em `frontend/package.json:8-12`.
- **Correção recomendada:** workflow de PR com Node 24.17.0, `npm ci`, lint, type-check explícito, testes, build e E2E crítico; permissões somente leitura e cache baseado no lockfile.
- **Esforço:** médio
- **Bloqueia merge:** sim após o workflow ser introduzido; até lá, validação manual é insuficiente para produção.

### FRONT-007

- **Severidade:** ALTO
- **Área:** Tratamento de falhas / App Router
- **Descrição:** não existem `error.tsx`, `global-error.tsx`, `not-found.tsx` próprios ou `loading.tsx`.
- **Impacto:** exceções de renderização e rotas inexistentes caem em experiências genéricas; a rota de detalhes retorna tela vazia em erro/ausência.
- **Evidência:** inventário de `frontend/src/app`; `src/app/app/profissionais/[id]/page.tsx:14-16` trata apenas loading e dado presente, sem erro ou not-found.
- **Correção recomendada:** definir boundaries por segmento, 404 e loading coerentes, com retry seguro e telemetria sem dados sensíveis.
- **Esforço:** médio
- **Bloqueia merge:** não isoladamente; sim se a tela crítica não tiver fallback equivalente.

### FRONT-008

- **Severidade:** MÉDIO
- **Área:** Server/Client Components / performance
- **Descrição:** seis `page.tsx` são marcados como cliente mesmo quando apenas compõem componentes e props estáticas.
- **Impacto:** aumenta desnecessariamente o grafo cliente e perde a convenção de Server Component por padrão. A rota dinâmica também faz toda busca no cliente, sem streaming ou boundary do App Router.
- **Evidência:** diretiva na linha 1 de `app/app/profissionais/page.tsx`, `novo/page.tsx`, `[id]/page.tsx`, `funcoes-cargos/page.tsx`, `funcoes-cargos/novo/page.tsx`, `especialidades/page.tsx` e `especialidades/novo/page.tsx` (sete páginas; seis são composição estática, a dinâmica usa hooks).
- **Correção recomendada:** remover a diretiva das páginas de composição e manter interatividade nas features; avaliar page servidor + componente cliente/hidratação para detalhes.
- **Esforço:** pequeno a médio
- **Bloqueia merge:** não.

### FRONT-009

- **Severidade:** MÉDIO
- **Área:** API / cancelamento / performance
- **Descrição:** query functions ignoram o `AbortSignal` fornecido pelo TanStack Query; filtros fazem request a cada tecla.
- **Impacto:** chamadas obsoletas continuam consumindo rede e podem atualizar cache fora de ordem; a busca cresce desnecessariamente com a digitação.
- **Evidência:** `professionalsApi.ts:15-17` não recebe `signal`; `ProfessionalsList.tsx:11` cria filtros diretamente dos três `useState` e executa a query; Axios possui timeout em `apiClient.ts:9-14`, mas timeout não substitui cancelamento.
- **Correção recomendada:** aceitar `signal` nas funções API, passá-lo ao Axios e representar filtros relevantes na URL com debounce explícito.
- **Esforço:** médio
- **Bloqueia merge:** não, salvo em telas de grande volume.

### FRONT-010

- **Severidade:** MÉDIO
- **Área:** Formulários / erros
- **Descrição:** o formulário de dados mestres não apresenta erros Zod/API, não bloqueia envio duplicado e não fornece feedback de processamento; o formulário profissional apresenta somente a primeira mensagem geral da API.
- **Impacto:** falhas podem virar rejeições sem tratamento ou UX silenciosa; duplo clique pode duplicar operações; erros por campo retornados pela API são desperdiçados.
- **Evidência:** `MasterDataForm.tsx:8-9`; `ProfessionalForm.tsx:35-50,65`; o envelope já oferece `field` em `types/api.ts:1-9`, mas não há mapeamento para `setError`.
- **Correção recomendada:** usar mutation/pending, `try/catch`, mensagem geral e `setError` por campo; padronizar componentes/adapters MUI+RHF.
- **Esforço:** médio
- **Bloqueia merge:** sim para formulários financeiros; não para o cadastro atual se tratado antes de produção.

### FRONT-011

- **Severidade:** MÉDIO
- **Área:** Acessibilidade
- **Descrição:** selects de função/especialidade e filtro de status não têm associação de label visível; erro da função não possui texto auxiliar associado. O título do formulário de master data não declara heading semântico.
- **Impacto:** usuários de leitor de tela recebem contexto insuficiente; mensagens de validação podem não ser anunciadas.
- **Evidência:** `ProfessionalForm.tsx:54-61`; `ProfessionalsList.tsx:12`; `MasterDataForm.tsx:9` usa `Typography variant="h4"` sem `component="h1"`.
- **Correção recomendada:** `FormControl`, `InputLabel`, `labelId`, `FormHelperText`, `aria-describedby` e headings semânticos; validar com axe e teclado.
- **Esforço:** pequeno
- **Bloqueia merge:** não, exceto requisito regulatório/contratual.

### FRONT-012

- **Severidade:** MÉDIO
- **Área:** UX / ações destrutivas
- **Descrição:** ativar/inativar é disparado imediatamente, sem confirmação, feedback de sucesso, bloqueio por linha ou rollback visível.
- **Impacto:** clique acidental altera estado; múltiplos cliques geram chamadas concorrentes; falha da mutation não é apresentada na lista.
- **Evidência:** `ProfessionalsList.tsx:11-12`.
- **Correção recomendada:** confirmação para inativação, estado pending por item, mensagem de erro/sucesso e estratégia de rollback se houver otimista.
- **Esforço:** pequeno
- **Bloqueia merge:** não.

### FRONT-013

- **Severidade:** MÉDIO
- **Área:** Arquitetura / contratos
- **Descrição:** `types/api.ts` mistura contratos de autenticação, profissionais e dados mestres; funções de roles/specialties estão dentro da API de profissionais e são consumidas por features irmãs.
- **Impacto:** acoplamento transversal crescente, ownership ambíguo e maior chance de ciclos/alterações em cascata.
- **Evidência:** `types/api.ts:1-61`; `professionalsApi.ts:40-63`; imports em `professional-roles/ProfessionalRolesList.tsx:6`, `professional-specialties/ProfessionalSpecialtiesList.tsx:6` e `master-data/MasterDataForm.tsx:7`.
- **Correção recomendada:** co-localizar contratos, API, schemas e query keys por domínio; compartilhar apenas tipos realmente transversais em `lib`/`types` explícitos.
- **Esforço:** médio
- **Bloqueia merge:** não.

### FRONT-014

- **Severidade:** MÉDIO
- **Área:** Estado de autenticação
- **Descrição:** ao falhar `/auth/me`, o efeito limpa storage e o cliente Axios, mas não atualiza `accessToken` no estado React.
- **Impacto:** a aplicação permanece com token inválido na memória/contexto (`accessToken` não nulo), gerando estado contraditório e dificultando recuperação/novo login; o efeito também omite `clearSession` da dependência.
- **Evidência:** `AuthProvider.tsx:35,57-62,82-98` versus `clearSession` completo em `50-55`.
- **Correção recomendada:** reutilizar uma transição única e idempotente de logout/expiração, atualizar estado e caches, e tratar 401 globalmente sem loops.
- **Esforço:** pequeno
- **Bloqueia merge:** não isoladamente.

### FRONT-015

- **Severidade:** BAIXO
- **Área:** Roteamento e manutenção
- **Descrição:** navegação comum usa `router.push` em botões onde `Link` preservaria semântica, abertura em nova aba e prefetch; alguns arquivos adicionados estão inteiramente em uma linha.
- **Impacto:** acessibilidade/UX de links e legibilidade/revisão reduzidas.
- **Evidência:** `ProfessionalsList.tsx:12`, `ProfessionalRolesList.tsx:10-16`, `ProfessionalSpecialtiesList.tsx:10-16`, `MasterDataForm.tsx:9` e páginas `novo/page.tsx:1`.
- **Correção recomendada:** usar `Link` para navegação declarativa e formatador verificado em CI.
- **Esforço:** pequeno
- **Bloqueia merge:** não.

### FRONT-016

- **Severidade:** BAIXO
- **Área:** Configuração / segurança de navegador
- **Descrição:** `next.config.ts` está vazio; não há evidência de headers de defesa como CSP, `frame-ancestors`, `Referrer-Policy` e `Permissions-Policy` no frontend/reverse proxy auditado.
- **Impacto:** menor defesa em profundidade contra XSS/clickjacking e maior impacto do token em `localStorage`.
- **Evidência:** `frontend/next.config.ts:1-7`.
- **Correção recomendada:** definir headers no ponto real de terminação HTTP, com CSP compatível com MUI/Next e testes; não adicionar cegamente sem conhecer proxy/deploy.
- **Esforço:** médio
- **Bloqueia merge:** não isoladamente; reforça o bloqueio do armazenamento de token.

## 6. Server Components e Client Components

Foram encontrados **23 arquivos** com `"use client"`.

**Justificados pela implementação atual:**

- `providers/AppProviders.tsx`: mantém `QueryClient` estável e providers que dependem de contexto/estado.
- `providers/ToastProvider.tsx`: contexto e estado de Snackbar.
- `features/auth/AuthProvider.tsx`, `TenantProvider.tsx`, `RequireAuth.tsx`, `RequireAcesso.tsx`, `HomeRedirect.tsx`: hooks, contexto, effects e navegação cliente. A estratégia em si merece revisão de segurança, mas a diretiva é tecnicamente necessária.
- `features/auth/LoginPage.tsx`, `AuthenticatedHome.tsx`: formulários/eventos/hooks.
- `features/professionals/ProfessionalsList.tsx`, `ProfessionalForm.tsx`; `features/master-data/MasterDataForm.tsx`; listas de roles/specialties: queries, formulários e navegação interativa.

**Merecem revisão:**

- `features/auth/AuthStates.tsx`: não usa hook nem browser API; pode ser Server Component se não continuar importado por módulos cliente. Hoje a importação por guards clientes o inclui no bundle mesmo sem diretiva.
- `theme/theme.ts`: `createTheme` é consumido pelo provider cliente, mas a diretiva no módulo de configuração é redundante; remover exige validar a integração MUI 9/Next 16.
- `app/app/profissionais/page.tsx`, `novo/page.tsx`, `funcoes-cargos/page.tsx`, `novo/page.tsx`, `especialidades/page.tsx`, `novo/page.tsx`: apenas compõem componentes e valores serializáveis; devem permanecer Server Components por padrão.
- `app/app/profissionais/[id]/page.tsx`: hooks justificam o estado atual, porém concentra guard e data fetching no cliente; avaliar uma fronteira cliente menor e loading/error/not-found do segmento.

O `layout.tsx` permanece Server Component e passa `children` ao provider cliente, padrão suportado pelo Next. Não foram identificadas props não serializáveis atravessando a fronteira, importação de módulo server-only no cliente ou uso de `document`. O uso de `window/localStorage` é protegido por `typeof window`, evitando acesso no SSR, mas continua sendo risco de segurança. Não há evidência direta de erro de hidratação no build; a autenticação exclusivamente cliente causa uma etapa de loading/possível mudança visual até restaurar a sessão.

## 7. Segurança e multi-tenancy

### Riscos confirmados

| Risco | Severidade | Evidência resumida | Exigência de backend |
| --- | --- | --- | --- |
| Tenant arbitrário aceito sem vínculo por request | CRÍTICO | `HttpTenantContext.cs:13-17` lê o header; policy verifica apenas claim global de recurso. | Validar usuário + tenant + recurso em toda operação e filtrar toda query/escrita. |
| Cache cruza tenant | CRÍTICO | Keys sem tenant e nenhuma limpeza na troca. | Mesmo corrigindo o frontend, API nunca deve confiar no tenant/ID enviado. |
| Cache cruza logout/login | CRÍTICO | Logout remove só `['auth']`. | Respostas devem continuar isoladas e autorizadas independentemente de cache cliente. |
| Token em `localStorage` | ALTO | `authStorage.ts:12,20`. | Preferir cookie seguro/BFF; rotação, revogação e expiração server-side. |
| Permissões de UI como guard | ALTO se tratadas como segurança | `RequireAcesso.tsx:14-20` somente oculta/renderiza conteúdo. | API deve exigir policy em todos os endpoints; menus nunca são barreira de segurança. |
| IDs em URL/payload | Alto potencial | `/profissionais/[id]` e `userId` opcional no payload. | Ignorar tenant vindo do corpo, validar ownership de `id`/`userId` no tenant e retornar 404/403 consistente. |
| Ausência de CSRF/CSP observável | Médio/baixo no desenho atual | Bearer header reduz CSRF tradicional, mas não XSS; `next.config.ts` vazio. | Ao migrar para cookie, adotar SameSite e token/origin check conforme topologia. |

Não foi encontrado `dangerouslySetInnerHTML`, segredo hardcoded no código frontend, `console.log` ou variável `NEXT_PUBLIC_*` além da URL pública da API. Foram detectados arquivos `.env` na raiz, mas seus valores não foram reproduzidos nem incluídos neste relatório. A existência de `.env` por si só não prova exposição; deve-se garantir que não esteja versionado e que somente valores não sensíveis usem `NEXT_PUBLIC_*`.

Há sinais positivos no backend: controllers usam `[Authorize]` e repositórios filtram `TenantId`. Isso não neutraliza o FRONT-001, pois o contexto filtrado ainda deriva do header manipulável e as claims de recurso são agregadas entre tenants. O `TenantContextValidator` existente demonstra a regra pretendida, mas não foi encontrado no fluxo das requisições de negócio.

Sessão: há expiração informada pelo backend (`types/api.ts:25-29`), porém o frontend não agenda expiração, não renova token e só descobre invalidade em `/auth/me`/requests. Logout é apenas local; não foi observada revogação server-side. Não há open redirect identificado: destinos são constantes em `getAuthRedirectPath.ts:4-13`. Não há renderização de HTML não confiável.

## 8. Testes existentes e lacunas

### Estado atual

Não existem testes frontend unitários, de componentes, integração ou E2E. Também não há MSW/equivalente, fixtures, coverage ou scripts. Os testes .NET encontrados não substituem testes da interface. Há teste unitário do `TenantContextValidator`, mas não há evidência de integração desse validator ao pipeline de autorização, ilustrando por que teste unitário isolado não basta.

### Lacunas prioritárias

1. **Isolamento de tenant:** trocar tenant cancela/remove cache anterior; header adulterado para tenant sem vínculo retorna 403; IDs de outro tenant nunca retornam dados.
2. **Isolamento de sessão:** logout limpa todo dado remoto; login de outro usuário não renderiza cache anterior.
3. **Login/sessão:** sucesso, 401, 403, expiração, falha de `/me`, logout e redirecionamento por perfil.
4. **Autorização:** acesso direto por URL, menu/botão oculto e, principalmente, rejeição pela API para cada perfil.
5. **Profissionais:** listar, buscar, criar, editar, ativar/inativar, validação por campo, conflito e falha de rede.
6. **Funções/cargos e especialidades:** pending, duplo envio, erro e estado vazio.
7. **Acessibilidade:** labels, foco, teclado, announcements de erro e modal de confirmação.
8. **Fluxos futuros ainda não implementados:** procedimentos, regras de comissão, cálculo/visualização, contestação, resolução e fechamento. Classificação: não aplicável ao código atual, mas obrigatória antes de suas entregas.

Estratégia recomendada: Vitest + Testing Library (ou equivalentes compatíveis) para lógica/componentes; MSW para contratos HTTP e falhas; Playwright para login, autorização e isolamento cross-tenant. Evitar asserts sobre classes internas do MUI ou implementação de hooks; afirmar comportamento e saída acessível.

## 9. CI/CD e proteção de branch

### Jobs existentes

Nenhum workflow GitHub Actions foi encontrado.

### Pipeline mínimo de pull request

1. Checkout com permissões `contents: read`.
2. Setup da versão exata Node 24.17.0 e cache npm baseado em `frontend/package-lock.json`.
3. `npm ci` dentro de `frontend`.
4. `npm run lint`.
5. `npm run type-check` após criar o script explícito.
6. `npm test` após introduzir a suíte.
7. `npm run build` com apenas variáveis não sensíveis/ambiente de CI.
8. E2E crítico em ambiente efêmero, nunca contra produção, com dados isolados.
9. Formatação, somente quando houver formatter e script de check.

Cada passo deve falhar o job por código diferente de zero. Não usar `continue-on-error` nos gates. Segredos devem residir em GitHub Environments/Secrets, com permissões mínimas e sem execução de segredos em PR de fork.

### Checks recomendados como obrigatórios

- `frontend / lint`
- `frontend / type-check`
- `frontend / unit-and-component-tests`
- `frontend / build`
- `frontend / e2e-critical`
- `backend / tenant-isolation-integration` (bloqueador conjunto do FRONT-001)

Enquanto esses checks não existirem, exigir evidência manual equivalente não oferece a mesma repetibilidade e proteção.

## 10. Plano de correção priorizado

### Prioridade 0: correção imediata

| Ação | Dependências | Esforço |
| --- | --- | --- |
| Corrigir autorização da API para validar `(user, tenant, recurso)` por request e não agregar permissões sem escopo. | Decisão de modelo de claims/sessão; testes de integração com dois tenants e perfis distintos. | Grande |
| Tornar todas as query keys tenant-scoped, cancelar/remover queries na troca e limpar o cache no logout/troca de usuário. | FRONT-001 pode avançar em paralelo; requer testes de QueryClient/providers. | Médio |
| Criar testes negativos de isolamento: header, URL, payload, cache e troca de sessão. | Infra de testes frontend + integração API. | Grande |
| Impedir produção até os três itens anteriores passarem em CI. | Workflow mínimo. | Pequeno |

### Prioridade 1: antes de novas funcionalidades

| Ação | Dependências | Esforço |
| --- | --- | --- |
| Migrar token para cookie `HttpOnly` seguro/BFF e definir CSRF/refresh/revogação. | Arquitetura de deploy, domínio e CORS. | Grande |
| Criar CI de PR com instalação determinística, lint, type-check, testes e build. | Scripts de teste/type-check. | Médio |
| Adicionar suíte de componentes/integração com mock HTTP e E2E de login/permissões. | Escolha das ferramentas. | Grande |
| Unificar transições de sessão expirada/logout e limpar estado/cache. | Estratégia de sessão definida. | Médio |
| Adicionar `error.tsx`, `loading.tsx`, `not-found.tsx` e tratamento completo da rota de detalhes. | Padrão de UX/telemetria. | Médio |

### Prioridade 2: durante a evolução do MVP

| Ação | Dependências | Esforço |
| --- | --- | --- |
| Separar API/types/query keys por feature e reduzir dependência de master data sobre profissionais. | Contratos estabilizados. | Médio |
| Padronizar RHF+MUI, erros por campo/API, pending e feedback de sucesso. | Envelope de validação estável. | Médio |
| Remover `use client` das páginas de composição; avaliar streaming/prefetch onde trouxer benefício. | Testes para evitar regressão. | Médio |
| Representar filtros/paginação na URL, usar debounce e cancelamento. | Contrato de paginação backend. | Médio |
| Confirmar ações destrutivas e melhorar estados por item. | Componente/dialog padrão. | Pequeno |

### Prioridade 3: melhorias futuras

| Ação | Dependências | Esforço |
| --- | --- | --- |
| Medir bundle e Core Web Vitals antes de lazy loading/otimizações adicionais. | Ambiente observável. | Médio |
| Consolidar variantes/tokens MUI quando a repetição estiver comprovada. | Mais telas reais. | Médio |
| Adotar CSP e headers no ponto de terminação HTTP com testes. | Topologia de produção definida. | Médio |
| Avaliar modo escuro apenas se entrar no escopo de produto. | Decisão de produto/design. | Médio |

## 11. Pontos positivos

- Versões atuais e coerentes de Next.js 16, React 19, MUI 9 e TanStack Query 5; recomendações deste relatório respeitam essas versões.
- App Router real e root layout semanticamente válido com `lang="pt-BR"` (`app/layout.tsx:19-25`).
- Uso correto do adaptador oficial `AppRouterCacheProvider` para MUI/Next 16 (`AppProviders.tsx:5,17`).
- `QueryClient` criado uma única vez por montagem (`AppProviders.tsx:13-18`) e defaults sensatos de retry/stale/refocus (`queryClient.ts:3-15`).
- Cliente Axios único com base URL central, timeout, Bearer/tenant e normalização de erros (`apiClient.ts:9-35`).
- Endpoints centralizados (`lib/api/endpoints.ts`).
- TypeScript strict, alias `@/*` e ausência de `any`, supressões TypeScript/ESLint e logs de debug na busca objetiva.
- React Hook Form + Zod com inferência de tipos no login e cadastro profissional (`LoginPage.tsx:24-61`; `ProfessionalForm.tsx:15-41`).
- Prevenção de submissão duplicada no login e formulário profissional (`LoginPage.tsx:158-166`; `ProfessionalForm.tsx:65`).
- Uso de `next/image`, alt text e imagens locais no login/home.
- Listas possuem estados explícitos de loading, erro e vazio; chaves de linhas usam IDs estáveis.
- Tema centralizado com paleta, tipografia, spacing e overrides básicos (`theme/theme.ts:5-56`).
- Repositórios backend filtram entidades por `TenantId` e controllers exigem policies; são boas camadas de defesa, embora a validação do contexto ainda esteja incompleta.
- Lockfile presente e Node fixado em `.node-version`/`engines`.
- `npm run lint` e `npm run build` passaram nesta auditoria.

## 12. Limitações da auditoria

- Não foram iniciados frontend, API, banco ou navegador; portanto, navegação real, CORS, cookies, responsividade visual, foco, contraste, hidratação e Core Web Vitals não foram medidos dinamicamente.
- Testes E2E não existem e não foram improvisados; a regra era executar apenas comandos disponíveis e seguros.
- Não foi executado `npm ci` porque as dependências já estavam instaladas; não houve alteração ou revalidação limpa do lockfile em ambiente vazio.
- Não houve scanner de vulnerabilidades de dependências nem consulta externa de advisories; isso exigiria rede e não substitui política de atualização controlada.
- O frontend está em fase parcial do MVP: procedimentos, regras, comissões, contestações e fechamento ainda não aparecem no código e foram classificados como lacunas futuras, não como defeitos de implementação inexistente.
- Alterações locais preexistentes podem representar código ainda não revisado. Foram auditadas no estado atual, mas não modificadas.
- Valores de `.env`, tokens, credenciais e chaves não foram expostos no relatório. A auditoria não certifica a segurança de ambientes/deploy não presentes no repositório.
- A análise de ciclos foi estática/manual; não há ferramenta dedicada configurada. Nenhum ciclo foi evidenciado no grafo pequeno atual.

## Conclusão obrigatória

1. **O projeto está aderente às boas práticas atuais de Next.js?** Parcialmente. A fundação técnica e o build são adequados, mas o uso amplo de Client Components, a falta de boundaries/testes e, principalmente, a sessão/segurança impedem conformidade plena.
2. **A arquitetura está adequada para o crescimento do Vetcom?** Ainda não. A organização por feature é um bom começo, mas isolamento de tenant, escopo do cache, contratos por domínio, testes e CI precisam ser corrigidos antes que o número de módulos cresça.
3. **Existem problemas que devem bloquear novos desenvolvimentos?** Sim. Novas funcionalidades multi-tenant devem aguardar a correção do enforcement na API e do cache/sessão no frontend. Trabalho dedicado à estabilização pode continuar.
4. **Existem problemas que devem bloquear merge ou produção?** Sim. FRONT-001, FRONT-002 e FRONT-003 bloqueiam merge de funcionalidades multi-tenant e produção. FRONT-004, FRONT-005 e FRONT-006 também devem bloquear uma liberação produtiva.
5. **Quais são as três próximas ações recomendadas?** (1) corrigir e testar autorização `(usuário, tenant, recurso)` na API; (2) segmentar/limpar/cancelar o cache por tenant e sessão; (3) criar CI com testes automatizados de isolamento, login e autorização antes de retomar features.

## Reavaliação pós-adequação arquitetural — 21/09/2026

Esta seção substitui as conclusões conflitantes do diagnóstico inicial para o estado atual do workspace.

### Validações atuais

| Validação | Resultado | Evidência |
| --- | --- | --- |
| Frontend lint | PASSOU | `frontend`: `npm.cmd run lint`, código 0. |
| Frontend build | PASSOU | `frontend`: `npm.cmd run build`, Next 16.3.5, TypeScript e 13 rotas geradas. |
| Testes unitários backend | PASSOU | `dotnet test tests/VetCommission.UnitTests/VetCommission.UnitTests.csproj -c Release --no-restore`: 21/21. |
| Testes integração backend | PASSOU | `dotnet test tests/VetCommission.IntegrationTests/VetCommission.IntegrationTests.csproj -c Release --no-restore`: 15/15. |
| Testes frontend | NÃO CONFIGURADO | Adiados por decisão de escopo; não há runner frontend. |
| CI/CD | NÃO CONFIGURADO | Nenhum workflow GitHub Actions foi criado. |

### Status atualizado dos achados

- `FRONT-001`: **resolvido tecnicamente**. A policy consulta usuário, tenant e recurso no banco em `src/VetCommission.WebApi/Auth/AccessResourceAuthorizationHandler.cs:19-30`; os testes de isolamento cobrem header arbitrário, tenant inválido, recurso cruzado e vínculos de usuário.
- `FRONT-002`: **parcialmente resolvido**. Query keys de profissionais, funções e especialidades passaram a incluir tenant; queries recebem `enabled` e `AbortSignal`. Paginação e filtros na URL continuam pendentes.
- `FRONT-003`: **resolvido tecnicamente**. Logout e falha de `/auth/me` cancelam/limpam o `QueryClient` em `frontend/src/features/auth/AuthProvider.tsx:50-64`; troca de tenant remove queries de outros tenants em `TenantProvider.tsx:54-65`.
- `FRONT-004`: **aberto — alto**. Access token ainda está em `localStorage` em `frontend/src/features/auth/authStorage.ts`; depende da ADR e da decisão de topologia da tarefa 5.
- `FRONT-005`: **aberto — alto**. Testes frontend seguem adiados; os testes backend não substituem cobertura de guards, formulários, cache e UI.
- `FRONT-006`: **aberto — alto**. Não há `.github/workflows` nem checks obrigatórios.
- `FRONT-007`: **parcialmente resolvido**. Foram criados `src/app/loading.tsx`, `error.tsx`, `global-error.tsx` e `not-found.tsx`; a tela de detalhe ainda precisa distinguir erro de API e registro inexistente com `notFound()`.
- `FRONT-008`: **aberto — médio**. Páginas e features ainda possuem fronteiras cliente amplas.
- `FRONT-009`: **parcialmente resolvido**. Cancelamento foi propagado ao Axios; debounce, paginação, ordenação e estado reproduzível na URL permanecem pendentes.
- `FRONT-010`: **parcialmente resolvido**. `MasterDataForm` agora trata erro geral e `isSubmitting`; mapeamento de erros por campo e padronização de todos os formulários permanecem pendentes.
- `FRONT-011`: **aberto — médio**. Selects e foco/axe ainda não foram auditados dinamicamente.
- `FRONT-012`: **aberto — médio**. Ativação/inativação de profissional ainda não exige confirmação.
- `FRONT-013`: **aberto — médio**. APIs e contratos continuam concentrados em `features/professionals/professionalsApi.ts` e `src/types/api.ts`.
- `FRONT-014`: **parcialmente resolvido**. Falha de `/auth/me` agora invalida autenticação derivada e limpa cache; migração para cookie seguro continua pendente.
- `FRONT-015`: **parcialmente resolvido**. Arquivos alterados nesta etapa foram formatados e links quebrados de funções/especialidades foram removidos; navegação declarativa geral ainda pode evoluir.
- `FRONT-016`: **parcialmente resolvido**. `next.config.ts` agora emite CSP em modo report-only, `Referrer-Policy`, `Permissions-Policy` e `X-Content-Type-Options`; a CSP precisa ser validada no ambiente real e o ponto de terminação TLS ainda não foi confirmado.

### Novos achados corrigidos nesta reavaliação

- **AUTH-017 — resolvido:** controllers de funções e especialidades usavam `ProfessionalsManage` em vez de suas policies específicas. Corrigidos em `ProfessionalRolesController.cs:3` e `ProfessionalSpecialtiesController.cs:3`; teste `ProfessionalMasterData_RequireTheirSpecificResources` passou.
- **ROUTE-018 — resolvido:** botões “Visualizar” apontavam para rotas `[id]` inexistentes nas telas de funções e especialidades. As ações foram removidas até existir fluxo de detalhe/edição real.
- **CONFIG-019 — resolvido:** ausência de `NEXT_PUBLIC_API_URL` não usa mais silenciosamente uma URL local em produção. `apiClient.ts:9-24` falha no momento da requisição quando a configuração não existe; fallback localhost permanece somente fora de produção.

### Classificação corrente

**Nota atual estimada: 6,3/10 — parcialmente conforme.** A autorização de API, isolamento de cache e boundaries básicos melhoraram, mas a base ainda não está pronta para produção devido a token em `localStorage`, ausência de testes frontend, CI, E2E, paginação/URL e validação de deploy.

**Bloqueadores atuais de produção:** `FRONT-004`, `FRONT-005`, `FRONT-006` e a ausência de confirmação operacional da topologia de cookies/CSRF. **Bloqueadores para novas funcionalidades multi-tenant:** autenticação segura, testes frontend e CI. Nenhum arquivo de tabela genérica ou evolução de toast foi incluído.
