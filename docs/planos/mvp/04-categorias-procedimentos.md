# Categorias e procedimentos — plano da etapa

**Objetivo:** disponibilizar o catálogo versionável usado pelas regras e produções.

**Branch:** `<linear-id>-categorias-procedimentos`  
**Dependência:** etapa 03.

## Escopo

- `business.procedure_category` e `business.procedure`.
- CRUD lógico, busca, categoria, valor padrão e status.
- Lista, formulário e detalhe administrativo.
- Recursos de consulta e gerenciamento.

## Tarefas pequenas

1. Definir limites, unicidade por tenant/clínica e política de inativação.
2. Escrever testes de constraints, tenant e valor monetário.
3. Criar/aplicar script, validar objetos e executar scaffold.
4. Implementar mappers, repositórios, handlers e validators.
5. Expor endpoints e contratos paginados/filtros.
6. Implementar feature frontend, campos monetários e lista responsiva.
7. Confirmar que alteração de valor padrão não altera qualquer snapshot futuro/histórico.

## Conclusão

Catálogo ativo pode ser usado por etapas seguintes; itens inativos permanecem consultáveis e não aparecem em novos seletores operacionais.
