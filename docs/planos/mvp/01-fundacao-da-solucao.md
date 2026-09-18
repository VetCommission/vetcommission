# Fundação da solução — plano de implementação

> **Para execução agentic:** usar `superpowers:test-driven-development` durante as tarefas e `superpowers:verification-before-completion` antes de concluir. Não iniciar sem aprovação de `00-decisoes-arquiteturais.md`.

**Objetivo:** criar a estrutura compilável da solução .NET e do frontend Next.js, com padrões transversais, testes-base e automação local, sem implementar entidades do negócio.

**Arquitetura:** monólito modular em Clean Architecture pragmática, organizado por feature/caso de uso. WebApi e Worker são executáveis separados que compartilham Application, Domain e Infrastructure. Frontend único com route groups futuros para admin e profissional.

**Stack:** .NET 10, ASP.NET Core, MediatR, FluentValidation, EF Core/Npgsql, Serilog, xUnit, Testcontainers PostgreSQL, Next.js App Router, React, TypeScript strict, MUI, TanStack Query, Axios, React Hook Form e Zod; Node `24.17.0`.

**Especificações:** `ARQUITETURA.MD`, `ARQUITETURA_POSTGRES_DB.MD`, `docs/planos/mvp/README.md` e decisões aprovadas em `00-decisoes-arquiteturais.md`.

**Branch:** `<linear-id>-fundacao-solucao`  
**PR:** uma entrega técnica para `main`, sem entidades ou telas de negócio.

## Restrições globais

- Não criar tabelas de domínio nesta fatia.
- Não usar EF Migrations, `EnsureCreated` ou `EnsureDeleted`.
- Não editar `V001` ou `V002`.
- Não implementar login, tenant, permissões ou telas de negócio.
- Não versionar connection string, senha ou JWT secreto real.
- Arquivos gerados por scaffold ficarão em `Infrastructure/Persistence/Generated` nas fatias persistentes futuras.

## Estrutura resultante

```text
VetCommission.sln
src/
  VetCommission.Domain/
  VetCommission.Application/
  VetCommission.Infrastructure/
  VetCommission.WebApi/
  VetCommission.Worker/
tests/
  VetCommission.UnitTests/
  VetCommission.IntegrationTests/
frontend/
  src/app/
  src/components/
  src/features/
  src/lib/
  src/providers/
  src/theme/
  src/types/
  src/utils/
```

## Tarefa 1 — Criar solução e projetos .NET

**Criar:** `VetCommission.sln`, cinco projetos em `src/` e dois em `tests/`.  
**Produz:** grafo de dependências compilável, sem referência circular.

Passos:

1. Criar solution, class libraries, Web API, Worker e projetos xUnit com `dotnet new` em .NET 10.
2. Adicionar projetos à solution.
3. Referenciar `Domain` por `Application` e `Infrastructure`; referenciar `Application` e `Infrastructure` por WebApi e Worker; referenciar projetos testados pelos testes.
4. Remover arquivos de exemplo gerados.
5. Executar `dotnet restore VetCommission.sln` e `dotnet build VetCommission.sln --no-restore`.

**Verificação esperada:** build com zero erros e ausência de referências de Domain para outros projetos.

## Tarefa 2 — Instalar dependências por responsabilidade

**Modificar:** arquivos `.csproj` dos projetos.  
**Produz:** dependências limitadas à camada correta.

- Application: MediatR, FluentValidation e integração de DI.
- Infrastructure: EF Core, Npgsql, NamingConventions e abstrações necessárias de autenticação/serviços.
- WebApi: JWT Bearer, Serilog ASP.NET Core e Swagger.
- Worker: Serilog e hosting.
- UnitTests: xUnit, FluentAssertions e mocking adotado pelo projeto.
- IntegrationTests: `Microsoft.AspNetCore.Mvc.Testing`, Testcontainers PostgreSQL e FluentAssertions.

Executar restore e build após cada grupo de projetos. Revisar com `dotnet list <projeto> reference` para confirmar o grafo.

## Tarefa 3 — Criar primitives da Application por TDD

**Criar:**

```text
src/VetCommission.Application/Common/Errors/NotificationError.cs
src/VetCommission.Application/Common/Errors/ErrorCodes.cs
src/VetCommission.Application/Common/Results/NotificationResult.cs
src/VetCommission.Application/Common/Behaviors/ValidationBehavior.cs
src/VetCommission.Application/DependencyInjection.cs
src/VetCommission.Application/AssemblyReference.cs
tests/VetCommission.UnitTests/Application/Common/Results/NotificationResultTests.cs
tests/VetCommission.UnitTests/Application/Common/Behaviors/ValidationBehaviorTests.cs
```

**Interfaces produzidas:** `NotificationResult`, `NotificationResult<T>`, `NotificationError`, códigos estáveis e behavior MediatR.

Passos:

1. Escrever testes de sucesso, falha e propagação de erros.
2. Executar os testes e confirmar falha pela inexistência dos tipos.
3. Implementar os resultados conforme `ARQUITETURA.MD`.
4. Escrever teste que demonstra que validator inválido impede o handler e devolve erros com campo.
5. Implementar o behavior e registro de validators/handlers.
6. Executar somente esses testes e depois toda a suite unitária.

## Tarefa 4 — Criar esqueleto da Infrastructure

**Criar:**

```text
src/VetCommission.Infrastructure/Persistence/Generated/.gitkeep
src/VetCommission.Infrastructure/Persistence/Mappers/.gitkeep
src/VetCommission.Infrastructure/Repositories/.gitkeep
src/VetCommission.Infrastructure/Services/.gitkeep
src/VetCommission.Infrastructure/Auth/.gitkeep
src/VetCommission.Infrastructure/DependencyInjection.cs
```

O registro deve validar a presença de `ConnectionStrings:DefaultConnection` quando o DbContext passar a existir. Nesta fatia não criar contexto artificial nem executar scaffold sem tabelas de aplicação.

## Tarefa 5 — Bootstrap e pipeline da WebApi

**Criar/modificar:** `Program.cs`, `appsettings*.json`, options, extensions e middlewares da WebApi.

**Interfaces produzidas:** `/health`, Swagger em desenvolvimento, `X-Correlation-Id` e envelope seguro para exceção inesperada.

Passos TDD:

1. Criar factory mínima de integração.
2. Escrever teste de `/health` retornando `200`.
3. Escrever teste que envia/recebe `X-Correlation-Id`.
4. Escrever teste de erro inesperado sem stack trace no corpo.
5. Implementar bootstrap na ordem prescrita em `ARQUITETURA.MD`.
6. Executar testes de integração com ambiente isolado.

Não configurar autenticação fictícia; JWT e policies entram na fatia 02.

## Tarefa 6 — Bootstrap do Worker

**Criar/modificar:** `Program.cs`, `appsettings*.json` e options do Worker.

O Worker deve iniciar, registrar Application/Infrastructure e encerrar por `CancellationToken`. Não criar polling vazio, job de exemplo ou integração Slack.

**Verificação:** build do projeto e teste de composição/DI sem hosted service de negócio.

## Tarefa 7 — Criar fundação do frontend

**Criar:** aplicação Next.js em `frontend/`, `.node-version` com `24.17.0` e estrutura definida na arquitetura.

**Dependências:** MUI, Emotion, TanStack React Query, Axios, React Hook Form, Zod e resolver Zod.

**Arquivos centrais:**

```text
frontend/src/app/layout.tsx
frontend/src/app/page.tsx
frontend/src/providers/AppProviders.tsx
frontend/src/providers/ToastProvider.tsx
frontend/src/lib/api/apiClient.ts
frontend/src/lib/api/endpoints.ts
frontend/src/lib/api/apiError.ts
frontend/src/lib/query/queryClient.ts
frontend/src/theme/theme.ts
frontend/src/types/api.ts
frontend/src/utils/formatters.ts
```

Passos:

1. Criar projeto App Router com TypeScript strict, ESLint e alias `@/*`.
2. Instalar dependências.
3. Implementar tema com Inter e tokens de `03_DESIGN_SYSTEM_MVP.md`.
4. Montar providers na ordem Query, Theme, CssBaseline e Toast. Auth/Tenant entram na fatia 02.
5. Implementar cliente HTTP sem sessão, preservando pontos de extensão para Bearer e tenant.
6. Criar página inicial técnica mínima com nome do produto e estado “fundação pronta”; não reproduzir landing comercial.
7. Executar `npm run lint` e `npm run build`.

## Tarefa 8 — Automação local

**Criar:**

```text
scriptsPS/Start-Api.ps1
scriptsPS/Start-Worker.ps1
scriptsPS/Start-Frontend.ps1
scriptsPS/Start-All.ps1
scriptsPS/Build-Frontend.ps1
```

Os scripts devem resolver a raiz a partir de `$PSScriptRoot`, falhar cedo, não conter segredos e respeitar as portas API `5077` e frontend `3000`. `Start-All` verifica o health do PostgreSQL antes dos processos dependentes.

Atualizar `scriptsPS/README.md` com comandos, pré-requisitos e diagnóstico.

## Tarefa 9 — Verificação integrada e documentação

Executar, na ordem:

```powershell
dotnet restore VetCommission.sln
dotnet build VetCommission.sln --no-restore
dotnet test VetCommission.sln --no-build
cd frontend
fnm use
npm run lint
npm run build
```

Depois:

- iniciar PostgreSQL e verificar health;
- iniciar API e validar `/health`;
- iniciar Worker e confirmar start/stop limpo;
- iniciar frontend e verificar a página em desktop e mobile;
- revisar `git diff --check` e `git status --short`;
- registrar comandos, resultados e limitações na issue.

## Critério de conclusão

- Estrutura-alvo existe e compila.
- Testes-base comprovam results, validação e pipeline HTTP.
- Frontend passa lint/build e usa o design system aprovado.
- Scripts locais iniciam componentes sem segredos.
- Nenhuma entidade, tabela ou tela funcional foi antecipada.
- Próxima fatia pode criar identidade/tenant sem reorganizar a fundação.
