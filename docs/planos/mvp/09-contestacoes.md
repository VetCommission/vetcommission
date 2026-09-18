# Contestações — plano da etapa

**Objetivo:** resolver divergências antes do fechamento com histórico e decisão justificada.

**Branch:** `<linear-id>-contestacoes`  
**Dependências:** etapas 07 e 08; decisão D09.

## Escopo

- Contestação e histórico nos estados Em análise, Aprovada e Rejeitada.
- Abertura a partir de produção própria.
- Listas e detalhes profissional/administrativo.
- Aprovação com eventual correção/recálculo; rejeição preserva valores.
- Evidência textual; arquivo permanece fora do corte inicial.

## Tarefas pequenas

1. Testar propriedade do lançamento, duplicidade ativa e estados finais.
2. Criar/aplicar script, scaffold e mapeamentos.
3. Implementar abertura e consultas pessoais.
4. Implementar fila administrativa e detalhe comparativo.
5. Implementar aprovar/rejeitar com justificativa obrigatória.
6. Executar correção + comissão + decisão + auditoria na mesma transação.
7. Implementar timeline, mensagens e estados responsivos.
8. Testar bloqueio posterior usado pelo fechamento.

## Conclusão

Toda decisão é atribuída, datada e explicada; aprovação mostra novo resultado e rejeição mantém o original.
