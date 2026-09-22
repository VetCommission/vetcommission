# Demanda de CI/CD, testes e performance do frontend

## 1. Objetivo

Criar uma frente exclusiva de qualidade automatizada para o frontend Next.js do Vetcom. A frente deverá impedir que alterações com regressão de tipagem, formatação, comportamento, segurança multi-tenant, build ou performance sejam integradas sem uma decisão explícita.

Esta demanda contempla a automação. A implementação de novas telas e regras de produto continua fora deste documento.

## 2. Estado atual conhecido

- Aplicação em `frontend/` usando Next.js `16.3.5`, React `19.2.8`, Node.js `24.17.0` e npm.
- Lockfile presente em `frontend/package-lock.json`.
- Scripts existentes: `lint`, `format`, `format:check`, `build`, `dev` e `start`.
- Ainda não existe workflow em `.github/workflows`.
- Ainda não existe runner de testes frontend.
- Ainda não existem testes `*.test.*` ou `*.spec.*` frontend.
- Ainda não existe configuração de Vitest, Testing Library, MSW ou Playwright.
- A API .NET possui testes unitários e de integração, mas eles não substituem os testes da interface.
- O token em `localStorage` permanece fora desta demanda por decisão explícita do projeto.

## 3. Resultado esperado

Ao final desta demanda:

1. Todo Pull Request executará validações determinísticas.
2. O pipeline falhará quando qualquer gate obrigatório falhar.
3. Os fluxos críticos terão testes de comportamento, não de implementação interna.
4. Os testes de tenant impedirão regressões de isolamento e cache.
5. O build de produção será validado antes do merge.
6. O bundle terá orçamento versionado e proteção contra regressões.
7. Os checks essenciais serão obrigatórios na proteção da branch.
8. Nenhuma etapa executará contra produção ou dependerá de dados reais.

## 4. Dependências e premissas

### 4.1 Ambiente

- Node.js fixado em `24.17.0`.
- npm usando `frontend/package-lock.json`.
- API e banco de testes isolados para os cenários E2E.
- Fixtures ou seed determinístico para usuários, tenants, clínicas e permissões.
- Nunca reutilizar banco de produção.

### 4.2 Dependências de desenvolvimento previstas

- Vitest.
- Testing Library para React.
- `@testing-library/jest-dom`.
- `@testing-library/user-event`.
- Ambiente DOM compatível, como `jsdom`.
- MSW para mocks de API.
- Playwright para E2E.
- Ferramenta de análise de bundle compatível com Next.js 16.
- Lighthouse ou ferramenta equivalente para performance em ambiente controlado.

As versões devem ser confirmadas contra Next.js 16, React 19 e TypeScript instalado antes da adição ao projeto.

## 5. Scripts esperados no frontend

Adicionar scripts sem comportamento destrutivo:

```json
{
  "lint": "eslint",
  "format:check": "prettier --check .",
  "type-check": "tsc --noEmit",
  "test": "vitest",
  "test:run": "vitest run",
  "test:watch": "vitest",
  "test:coverage": "vitest run --coverage",
  "test:e2e": "playwright test",
  "test:e2e:ui": "playwright test --ui",
  "analyze": "...",
  "performance:check": "..."
}
```

Os comandos `analyze` e `performance:check` devem ser definidos somente após a escolha das ferramentas e dos budgets. O pipeline nunca deve usar `lint --fix`, atualizar snapshots automaticamente ou modificar o lockfile.

## 6. Estrutura esperada de testes

Sugestão de organização:

```text
frontend/
├── src/
│   └── ...
├── test/
│   ├── setup.ts
│   ├── render.tsx
│   ├── handlers.ts
│   ├── server.ts
│   ├── factories/
│   └── fixtures/
├── e2e/
│   ├── auth.spec.ts
│   ├── tenant-isolation.spec.ts
│   ├── professionals.spec.ts
│   └── master-data.spec.ts
└── playwright.config.ts
```

A estrutura final pode variar, mas deve manter separados:

- testes unitários;
- testes de componentes;
- mocks de rede;
- fixtures de autenticação;
- testes E2E;
- artefatos de execução.

## 7. Configuração dos testes unitários e de componentes

### 7.1 Setup

- Configurar ambiente DOM.
- Registrar `jest-dom`.
- Criar `QueryClient` novo por teste.
- Desabilitar retry automático durante os testes.
- Limpar mocks e cache após cada teste.
- Garantir que nenhum teste dependa de API real.
- Configurar MSW para iniciar, resetar e encerrar o servidor corretamente.

### 7.2 Renderizador compartilhado

Criar um utilitário que monte componentes com os providers mínimos:

- MUI ThemeProvider;
- QueryClientProvider;
- AuthProvider quando necessário;
- TenantProvider quando necessário;
- ToastProvider somente quando o componente exigir.

O utilitário não deve esconder dependências relevantes do teste nem criar estado global compartilhado entre casos.

### 7.3 Cenários obrigatórios

#### Autenticação

- Inicialização sem token.
- Sessão válida.
- Falha de `/auth/me`.
- Logout manual.
- Limpeza de cache no logout.
- Login válido.
- Login inválido.
- Resposta `401`.
- Resposta `403`.
- Redirecionamento após login.

#### Tenant

- Seleção de tenant ativo.
- Troca de tenant A para B.
- Cancelamento de request do tenant anterior.
- Remoção do cache do tenant anterior.
- Retorno de B para A sem dados transitórios incorretos.
- Query desabilitada sem tenant ativo.

#### Autorização

- Usuário sem sessão.
- Usuário sem tenant.
- Usuário sem recurso.
- Usuário com recurso permitido.
- Menus e botões condicionados por permissão.

#### Formulários

- Validação local.
- Mensagem por campo.
- Erro geral.
- Erro de validação retornado pela API.
- Conflito `409`.
- Falha de rede.
- Estado de submissão.
- Prevenção de duplo clique.
- Reset após sucesso.
- Foco no primeiro erro.

#### TanStack Query

- Query keys tenant-scoped.
- Parâmetros de paginação e filtros na key.
- `enabled` sem tenant.
- Cancelamento via `AbortSignal`.
- Invalidação após mutation.
- Ausência de duplicação indevida de requests.

## 8. Testes E2E obrigatórios

Os E2E devem usar locators por papel, label ou texto acessível. Não devem depender de classes internas do MUI, seletores frágeis ou waits fixos.

### 8.1 Login e sessão

- Login válido.
- Login inválido.
- Sessão expirada.
- Logout.
- Acesso direto a rota protegida.
- Redirecionamento para login.

### 8.2 Autorização

- Usuário com acesso administrativo.
- Usuário sem acesso ao recurso.
- Botões e menus ocultos quando não permitidos.
- Acesso direto por URL sem permissão.

### 8.3 Multi-tenancy

- Tenant A visualiza apenas seus dados.
- Tenant B visualiza apenas seus dados.
- Troca A → B sem vazamento visual.
- Retorno B → A sem exibição de dados antigos.
- Tentativa de acessar profissional de outro tenant.
- Manipulação de identificador na URL.
- Manipulação de tenant em payload ou header.

### 8.4 Cadastros

- Cadastro de profissional.
- Edição de profissional.
- Validação obrigatória.
- Erro de validação da API.
- Falha de rede.
- Ativação/inativação com confirmação.
- Cancelamento da confirmação.
- Bloqueio de duplo clique.

### 8.5 Dados mestres

- Cadastro de função/cargo.
- Cadastro de especialidade.
- Paginação.
- Página vazia.
- Erro de carregamento.

## 9. Ambiente E2E

- Usar API, banco e fixtures isolados.
- Definir estratégia de criação e limpeza de dados.
- Usar usuários de teste com permissões explícitas.
- Não usar credenciais pessoais.
- Não executar contra produção.
- Não registrar tokens em logs.
- Armazenar trace, screenshot e vídeo somente em falhas.
- Repetições devem ser limitadas e justificadas; não mascarar flakiness.

## 10. Workflow GitHub Actions

Criar `.github/workflows/frontend-ci.yml` ou nome equivalente estável.

### 10.1 Configuração mínima

- Trigger em Pull Request.
- Trigger em push da branch principal.
- `permissions: contents: read`.
- Node `24.17.0`.
- `npm ci`.
- Cache baseado no hash de `frontend/package-lock.json`.
- Diretório de trabalho explícito: `frontend`.
- Sem segredos em texto.
- Sem `continue-on-error` nos gates.

### 10.2 Jobs recomendados

#### `frontend-quality`

Executa:

```text
npm ci
npm run lint
npm run format:check
npm run type-check
```

#### `frontend-unit`

Executa:

```text
npm ci
npm run test:run -- --coverage
```

Publica relatório de cobertura como artefato quando necessário.

#### `frontend-build`

Executa:

```text
npm ci
npm run build
```

O build deve falhar para qualquer erro de compilação, import inválido ou type-check do Next.

#### `frontend-e2e`

Executa em ambiente isolado:

```text
npm ci
npx playwright install --with-deps
npm run test:e2e
```

Esse job pode depender da API e do banco de teste. A dependência deve ser explícita.

#### `frontend-performance`

Executa após o build e, preferencialmente, em preview/staging:

```text
npm ci
npm run build
npm run analyze
npm run performance:check
```

O job deve falhar apenas quando o budget estiver definido e a ferramenta tiver resultado determinístico.

## 11. Segurança do workflow

- Permissão mínima no token do GitHub Actions.
- Segredos somente em Secrets/Variables do GitHub.
- Nunca imprimir tokens, cookies, headers de autorização ou dados pessoais.
- Não expor segredos a workflows de Pull Requests vindos de forks.
- Não executar scripts recebidos do código do Pull Request com credenciais de produção.
- Não usar `pull_request_target` sem revisão de segurança específica.
- Fixar versões de actions relevantes quando possível.
- Separar jobs que exigem segredos dos jobs executados em código não confiável.

## 12. Bundle e performance

### 12.1 Linha de base

Antes de otimizar, registrar:

- tamanho do JavaScript inicial por rota;
- tamanho total por rota;
- dependências maiores;
- Client Components carregados;
- quantidade de requests;
- requests duplicadas;
- waterfalls;
- LCP, INP e CLS.

### 12.2 Budgets iniciais

Os budgets devem ser definidos após a medição. Sugestões de métricas:

- JavaScript inicial por rota;
- tamanho total de assets;
- número máximo de requests iniciais;
- LCP máximo;
- CLS máximo;
- INP máximo;
- limite de regressão permitido por Pull Request.

Não aplicar virtualização, lazy loading ou memoização sem evidência de impacto.

### 12.3 Regressão

- Comparar o resultado com a baseline versionada.
- Exibir diferença no Pull Request.
- Falhar somente acima do limite acordado.
- Permitir atualização do budget somente por mudança revisada.
- Manter relatório histórico quando a ferramenta permitir.

## 13. Acessibilidade no pipeline

Depois da estabilização dos testes E2E:

- adicionar axe nos fluxos críticos;
- validar nome acessível, labels e headings;
- validar foco de diálogos;
- validar mensagens de erro;
- validar navegação básica por teclado;
- impedir regressões críticas de contraste e semântica.

Falhas críticas de acessibilidade devem impedir o merge; alertas de baixa severidade podem começar como relatório não bloqueante.

## 14. Branch protection

Depois que os jobs estiverem estáveis, tornar obrigatórios:

- `frontend-quality`;
- `frontend-unit`;
- `frontend-build`;
- `frontend-e2e` para alterações de autenticação, autorização, tenant e fluxos críticos;
- `frontend-performance` após a baseline estar estabilizada.

Também configurar:

- aprovação obrigatória conforme política do repositório;
- branch atualizada antes do merge, se aplicável;
- proibição de merge com checks pendentes;
- proibição de ignorar checks por `skip ci` sem política explícita.

## 15. Critérios de aceite

- Workspace limpo instala com `npm ci` sem alterar o lockfile.
- Node e npm usados no CI são compatíveis com o projeto.
- Lint, formatação, type-check, testes e build falham corretamente quando há erro.
- Testes frontend não acessam API real.
- E2E não acessa produção.
- Casos negativos de tenant e autorização existem.
- Testes de formulários cobrem sucesso, validação, conflito e rede.
- Testes de logout limpam sessão e cache.
- Artefatos de falha são preservados sem dados sensíveis.
- Bundle budget é baseado em medição real.
- Checks obrigatórios estão configurados na branch protection.
- O workflow não contém segredos.
- A documentação de execução local está atualizada.

## 16. Sequência recomendada de execução

1. Confirmar versões e instalar dependências de desenvolvimento compatíveis.
2. Criar configuração Vitest e setup global.
3. Criar renderizador de testes e MSW.
4. Cobrir AuthProvider, TenantProvider, guards e query keys.
5. Cobrir formulários e erros de API.
6. Criar fixtures isoladas para E2E.
7. Configurar Playwright e fluxos críticos.
8. Adicionar scripts de type-check, testes e E2E.
9. Criar workflow de qualidade, testes e build.
10. Medir bundle e performance.
11. Definir budgets e job de regressão.
12. Adicionar acessibilidade automatizada.
13. Configurar branch protection.
14. Atualizar a auditoria com os resultados reais.

## 17. Definição de pronto

A demanda somente será considerada concluída quando:

- todos os jobs obrigatórios estiverem funcionando em Pull Request;
- os fluxos críticos tiverem cobertura automatizada;
- o pipeline executar a partir de instalação limpa;
- os testes de isolamento multi-tenant estiverem presentes;
- o build de produção for validado;
- os budgets de performance estiverem versionados;
- os checks estiverem obrigatórios na branch;
- não houver segredo exposto nos workflows;
- a auditoria técnica for atualizada com evidências dos jobs executados.

## 18. Fora do escopo

- Migração do token de `localStorage` para cookie `HttpOnly`.
- Alterações funcionais em tabelas de pesquisa ou toast.
- Execução contra produção.
- Atualização automática de dependências.
- Otimizações especulativas sem medição.
- Criação de regras de negócio no frontend para substituir a API.
