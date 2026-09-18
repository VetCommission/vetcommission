# Dashboard e relatórios — plano da etapa

**Objetivo:** permitir gestão e consulta do ciclo completo sem planilha paralela.

**Branch:** `<linear-id>-dashboard-relatorios`  
**Dependência:** etapa 10.

## Escopo

- Dashboard administrativo por competência.
- Produção total, comissão total, procedimentos, profissionais ativos, fechamento e contestações.
- Relatórios mínimos: comissão/profissional, produção/profissional, produção/procedimento, resumo e extrato.
- Consulta em tela; exportação somente se posteriormente aprovada.

## Tarefas pequenas

1. Definir contratos agregados e consultas determinísticas.
2. Escrever testes reconciliando KPIs com lançamentos e fechamento.
3. Implementar queries `AsNoTracking` e índices baseados nos planos reais.
4. Expor dashboard e relatórios com filtros/paginação.
5. Implementar KPIs antes dos gráficos e manter valores textuais equivalentes.
6. Implementar tabelas/listas responsivas e estados sem movimento/sem resultado.
7. Validar nomenclatura inequívoca de produção versus comissão.

## Conclusão

Totais do dashboard, relatórios, extrato e fechamento reconciliam; gráficos não são a única forma de compreender os números.
