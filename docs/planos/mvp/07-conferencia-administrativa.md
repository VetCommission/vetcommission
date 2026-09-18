# Conferência administrativa — plano da etapa

**Objetivo:** permitir revisão e correção controlada antes do fechamento.

**Branch:** `<linear-id>-conferencia-administrativa`  
**Dependência:** etapa 06.

## Escopo

- Filtros por período, profissional, procedimento e situação.
- Detalhe com produção, regra, cálculo, comissão e histórico.
- Correção somente em competência aberta, com versão antes/depois e recálculo atômico.
- Policy específica para correção.

## Tarefas pequenas

1. Definir campos corrigíveis e transições permitidas.
2. Testar autorização, período fechado, concorrência e recálculo.
3. Evoluir banco apenas se a estratégia aprovada exigir versão adicional.
4. Implementar query paginada, detalhe e comando de correção.
5. Implementar filtros preservados na URL e retorno do detalhe.
6. Implementar confirmação, estados e histórico legível.
7. Validar que Operacional e Administrador respeitam recursos distintos.

## Conclusão

Uma correção autorizada preserva o valor anterior, recalcula e fica explicável; nenhuma correção normal atravessa tenant ou competência fechada.
