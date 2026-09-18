# Fechamento — plano da etapa

**Objetivo:** consolidar uma competência com validação, concorrência segura e resultado somente leitura.

**Branch:** `<linear-id>-fechamento`  
**Dependência:** etapa 09 e decisão D10.

## Escopo

- Diagnóstico de pendências e transições Em aberto → Em fechamento → Fechado.
- Snapshot consolidado por profissional.
- Lista/detalhe administrativo e consulta pessoal do resultado final.
- Confirmação explícita e bloqueio de alterações normais.

## Tarefas pequenas

1. Definir/testar todas as condições impeditivas.
2. Criar/aplicar estruturas de snapshot e índices.
3. Implementar diagnóstico idempotente.
4. Implementar fechamento transacional com controle de concorrência.
5. Testar duas tentativas simultâneas e repetição do comando.
6. Implementar lista, detalhe, resumo e confirmação administrativa.
7. Completar telas de fechamento do portal profissional.
8. Testar bloqueio de produção, correção e contestação após fechamento.

## Conclusão

Fechamento só ocorre sem impedimentos, não duplica totais, resiste a concorrência e preserva consulta histórica.
