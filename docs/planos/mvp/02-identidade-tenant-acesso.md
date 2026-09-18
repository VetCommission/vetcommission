# Identidade, tenant e acesso — plano da etapa

**Objetivo:** autenticar os três perfis e garantir isolamento por tenant antes de qualquer dado de negócio.

**Branch:** `<linear-id>-identidade-tenant-acesso`  
**PR:** uma entrega vertical; base `main`; merge somente após testes de isolamento.

## Escopo

- Tabelas `core.tenant`, `core.user`, vínculos, grupos, recursos e permissões.
- Seed operacional do primeiro administrador conforme decisão aprovada.
- `POST /api/auth/login` e `GET /api/auth/me`.
- JWT, policies, `ITenantContext`, `ITenantContextValidator` e `X-Tenant-Id`.
- `AuthProvider`, `TenantProvider`, `RequireAuth`, `RequireAcesso`, login, logout e direcionamento por perfil.
- Catálogo de recursos sincronizado entre C# e TypeScript.

## Tarefas pequenas

1. Especificar contratos e escrever testes de login, usuário inativo e vínculo de tenant.
2. Criar/aplicar script PostgreSQL e validar constraints/índices.
3. Executar scaffold, revisar diff e criar mappers/repositórios.
4. Implementar autenticação e emissão de JWT.
5. Implementar tenant context, policies e testes contra acesso cruzado.
6. Implementar cliente HTTP, providers, guards, login e estados de UX.
7. Validar Administrador, Operacional e Profissional em desktop/mobile.

## Fora do escopo

Recuperação por e-mail, profissionais de negócio e permissões específicas de módulos futuros.

## Conclusão

Usuários válidos entram, selecionam contexto permitido e não acessam outro tenant; 401, 403 e ausência de tenant possuem respostas e UX distintas.
