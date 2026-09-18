# Profissionais — plano da etapa

**Objetivo:** permitir cadastro, consulta e inativação de profissionais dentro do tenant/clínica ativa.

**Branch:** `<linear-id>-profissionais`  
**Dependência:** etapa 02.

## Escopo

- `business.professional` e vínculo opcional com usuário.
- Listar, obter, criar, atualizar, ativar e inativar.
- Busca por nome e filtros por função/status.
- Lista, formulário e detalhe administrativo.
- Recursos `menu.profissionais` e `profissionais.gerenciar`.

## Tarefas pequenas

1. Fixar DTOs, limites dos campos e casos de autorização.
2. Escrever testes de tenant, unicidade e inativação.
3. Criar/aplicar script, validar e executar scaffold.
4. Implementar domínio, repositório e casos de uso MediatR.
5. Expor controllers finos e documentar contratos no Swagger.
6. Implementar service, query keys, schema Zod, lista, formulário e detalhe.
7. Cobrir loading, vazio, erro, sem permissão, filtros e responsividade.

## Conclusão

Administrador gerencia profissionais; Operacional vê conforme policy; inativação preserva o registro e impede seleção futura quando a produção existir.
