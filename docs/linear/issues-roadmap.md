# Issues do roadmap MVP para o Linear

## 00 — Aprovar decisões arquiteturais bloqueantes

**Prioridade:** Urgent  
**Estimativa:** 3  
**Labels:** mvp, architecture, documentation  
**Plano:** `docs/planos/mvp/00-decisoes-arquiteturais.md`

Revisar e decidir D01–D12. Registrar decisões duradouras como ADRs. Esta issue é documental e bloqueia todas as implementações.

## 01 — Criar fundação da solução

**Prioridade:** High  
**Estimativa:** 8  
**Labels:** mvp, architecture, backend, frontend, testing  
**Plano:** `docs/planos/mvp/01-fundacao-da-solucao.md`

Criar solution .NET, frontend Next.js, testes-base, padrões transversais e automação sem antecipar domínio.

## 02 — Implementar identidade, tenant e acesso

**Prioridade:** High  
**Estimativa:** 8  
**Labels:** mvp, backend, frontend, database, security  
**Plano:** `docs/planos/mvp/02-identidade-tenant-acesso.md`

Entregar login, JWT, tenant ativo, permissões, guards, menus e testes de isolamento.

## 03 — Implementar profissionais

**Prioridade:** High  
**Estimativa:** 5  
**Labels:** mvp, backend, frontend, database, ux  
**Plano:** `docs/planos/mvp/03-profissionais.md`

Entregar cadastro, consulta e inativação de profissionais ponta a ponta.

## 04 — Implementar categorias e procedimentos

**Prioridade:** High  
**Estimativa:** 5  
**Labels:** mvp, backend, frontend, database, ux  
**Plano:** `docs/planos/mvp/04-categorias-procedimentos.md`

Entregar catálogo com valor padrão, status e preservação histórica.

## 05 — Implementar competências e regras de comissão

**Prioridade:** High  
**Estimativa:** 8  
**Labels:** mvp, backend, frontend, database, financial  
**Plano:** `docs/planos/mvp/05-competencias-regras-comissao.md`

Entregar competência aberta, vigência, prioridade, prevenção de sobreposição e prévia server-side.

## 06 — Implementar produção e cálculo

**Prioridade:** Urgent  
**Estimativa:** 8  
**Labels:** mvp, backend, frontend, database, financial, audit  
**Plano:** `docs/planos/mvp/06-producao-calculo.md`

Gravar produção, comissão, snapshot e auditoria atomicamente.

## 07 — Implementar conferência administrativa

**Prioridade:** High  
**Estimativa:** 5  
**Labels:** mvp, backend, frontend, ux, audit  
**Plano:** `docs/planos/mvp/07-conferencia-administrativa.md`

Entregar filtros, detalhe, correção controlada e recálculo auditado.

## 08 — Implementar portal do profissional

**Prioridade:** High  
**Estimativa:** 8  
**Labels:** mvp, frontend, backend, mobile, security, ux  
**Plano:** `docs/planos/mvp/08-portal-profissional.md`

Entregar dashboard pessoal, procedimentos, comissões e extrato mobile-first.

## 09 — Implementar contestações

**Prioridade:** High  
**Estimativa:** 8  
**Labels:** mvp, backend, frontend, database, workflow, audit  
**Plano:** `docs/planos/mvp/09-contestacoes.md`

Entregar abertura, acompanhamento, decisão justificada e recálculo quando aprovado.

## 10 — Implementar fechamento mensal

**Prioridade:** Urgent  
**Estimativa:** 8  
**Labels:** mvp, backend, frontend, database, financial, concurrency  
**Plano:** `docs/planos/mvp/10-fechamento.md`

Entregar diagnóstico, snapshot consolidado, concorrência segura, bloqueio e consulta histórica.

## 11 — Implementar dashboard e relatórios

**Prioridade:** Medium  
**Estimativa:** 8  
**Labels:** mvp, backend, frontend, reporting, ux  
**Plano:** `docs/planos/mvp/11-dashboard-relatorios.md`

Entregar KPIs e relatórios mínimos reconciliados com extrato e fechamento.

## 12 — Consolidar MVP para piloto

**Prioridade:** High  
**Estimativa:** 8  
**Labels:** mvp, testing, security, ux, accessibility, documentation  
**Plano:** `docs/planos/mvp/12-consolidacao-piloto.md`

Validar jornadas A–H, segurança, auditoria, UX, backup/restore e documentação operacional.
