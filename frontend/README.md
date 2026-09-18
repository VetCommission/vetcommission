# Frontend VetCommission

Aplicacao web do VetCommission para o MVP, criada como fundacao tecnica em Next.js App Router. Nesta etapa nao existem telas funcionais de negocio, login, tenant ou permissoes.

## Escopo desta fundacao

- Next.js com App Router, React e TypeScript strict.
- MUI, Emotion e tema base alinhado ao design system do MVP.
- TanStack Query para cache de requisicoes.
- Axios com cliente HTTP centralizado.
- React Hook Form, Zod e resolver Zod preparados para formularios futuros.
- Providers globais de cache, tema, baseline visual e feedback.
- Pagina tecnica minima indicando que a fundacao esta pronta.

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

`components` e `features` permanecem vazios nesta issue para evitar antecipar telas ou fluxos de negocio. As proximas etapas devem criar componentes e features conforme o plano aprovado de cada fatia.

## Requisitos locais

- Node.js `24.17.0`, conforme `.node-version`.
- API local em `http://localhost:5077`, quando houver integracao com backend.

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

## Observacoes de arquitetura

- Nao criar landing page comercial nesta etapa.
- Nao implementar autenticacao, autocadastro, recuperacao de senha, tenant ou permissoes nesta issue.
- O cliente HTTP ainda nao injeta Bearer token nem cabecalhos de tenant; esses pontos entram na etapa de identidade e acesso.
- `AGENTS.md` e um arquivo gerado pelo Next.js 16 para orientar ferramentas agenticas sobre mudancas da versao. Ele deve permanecer versionado para evitar sujeira recorrente no workspace.
