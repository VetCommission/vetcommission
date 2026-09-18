# Produção e cálculo — plano da etapa

**Objetivo:** registrar produção e gerar comissão auditável na mesma transação.

**Branch:** `<linear-id>-producao-calculo`  
**Dependência:** etapa 05 e decisões financeiras aprovadas.

## Escopo

- `business.production`, `business.commission` e snapshot da regra.
- Prévia, criação, lista e detalhe.
- Quantidade, valores, arredondamento e autoria.
- Auditoria de criação desde esta etapa.

## Tarefas pequenas

1. Escrever testes puros dos cálculos percentual/fixo e arredondamento.
2. Escrever testes de período, cadastros ativos, tenant e regra vigente.
3. Criar/aplicar script e validar integridade/snapshot.
4. Implementar gravação atômica por handler + Unit of Work.
5. Expor prévia e criação revalidando os mesmos dados.
6. Implementar formulário com valor sugerido, prévia, sucesso e erros acionáveis.
7. Implementar lista/detalhe com base + regra + cálculo + resultado.
8. Testar que mudanças posteriores de regra/procedimento não alteram o lançamento.

## Conclusão

Cada produção confirmada possui exatamente uma comissão e memória suficiente para reproduzir o resultado histórico.
