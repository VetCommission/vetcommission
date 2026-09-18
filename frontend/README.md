# Frontend VetCommission

Aplicacao web do VetCommission para o MVP, criada em Next.js App Router.

Nesta etapa de identidade e acesso, o frontend possui o fluxo inicial autenticado:

- login de usuarios previamente cadastrados;
- restauracao de sessao via `GET /api/auth/me`;
- Bearer token no cliente HTTP;
- tenant ativo via `X-Tenant-Id`;
- guards de autenticacao e permissao;
- logout;
- direcionamento inicial por recurso/perfil.

## Estrutura

```text
src/app/
src/components/
src/features/
src/lib/
src/providers/
src/theme/
src/types/
src/utils/
```

Principais arquivos da etapa de identidade:

```text
src/features/auth/AuthProvider.tsx
src/features/auth/TenantProvider.tsx
src/features/auth/RequireAuth.tsx
src/features/auth/RequireAcesso.tsx
src/features/auth/LoginPage.tsx
src/features/auth/AuthenticatedHome.tsx
src/lib/api/apiClient.ts
```

## Requisitos locais

- Node.js `24.17.0`, conforme `.node-version`.
- API local em `http://localhost:5077`, quando houver integracao com backend.

## Configuracao

Por padrao, o cliente HTTP usa:

```text
http://localhost:5077
```

Para apontar para outra API:

```powershell
$env:NEXT_PUBLIC_API_URL = "http://localhost:5077"
npm run dev
```

## Comandos

```powershell
npm ci
npm run dev
npm run lint
npm run build
```

Ou, a partir da raiz do repositorio:

```powershell
.\scriptsPS\Start-Frontend.ps1
.\scriptsPS\Build-Frontend.ps1
```

## Fluxo implementado

- `/login`: formulario de entrada.
- `/`: redireciona usuario autenticado conforme recurso do tenant ativo.
- `/app`: area autenticada administrativa, exige `admin.dashboard`.
- `/portal`: area autenticada do profissional, exige `professional.portal`.

O MVP nao possui autocadastro publico nem recuperacao automatizada de senha.

## Sessao e tenant

- O token e persistido em `localStorage` para restaurar a sessao.
- O tenant ativo e persistido em `localStorage`.
- O cliente HTTP injeta:
  - `Authorization: Bearer <token>`;
  - `X-Tenant-Id: <tenantId>`.
- `AuthProvider` controla login, logout e restauracao de sessao.
- `TenantProvider` controla o tenant ativo e impede selecao de tenant fora da sessao.

## Observacoes de arquitetura

- Nao criar landing page comercial nesta etapa.
- Nao criar autocadastro publico.
- Nao criar recuperacao automatizada de senha no MVP.
- Recursos de acesso devem permanecer sincronizados entre `src/VetCommission.Application/Features/Auth/Access/AccessResources.cs` e `frontend/src/features/auth/accessResources.ts`.
- `AGENTS.md` e um arquivo gerado pelo Next.js 16 para orientar ferramentas agenticas sobre mudancas da versao. Ele deve permanecer versionado para evitar sujeira recorrente no workspace.
