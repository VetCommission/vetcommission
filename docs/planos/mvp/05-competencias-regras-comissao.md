# Competências e regras de comissão — plano da etapa

**Objetivo:** determinar, sem ambiguidade, qual regra se aplica a uma produção em uma data.

**Branch:** `<linear-id>-competencias-regras-comissao`  
**Dependência:** etapa 04 e decisões D03–D05.

## Escopo

- Competência mensal em aberto.
- Regra percentual ou fixa, geral ou específica por profissional.
- Vigência `[início, fim)`, prioridade da específica e bloqueio de sobreposição.
- Lista, criação/edição e prévia textual do cálculo.

## Tarefas pequenas

1. Testar limites de data, prioridade, ausência e conflito de regras.
2. Criar/aplicar script com constraints e índices iniciados por `tenant_id`.
3. Executar scaffold e implementar resolução de regra como serviço de domínio.
4. Implementar casos de uso de competência e regras.
5. Criar endpoint de prévia server-side sem persistir produção.
6. Implementar formulário guiado, campos condicionais e resumo “R$ X gera R$ Y”.
7. Testar concorrência de criação de regras sobrepostas.

## Conclusão

Para tenant, clínica, profissional, procedimento e data, o backend retorna uma regra única ou erro explícito; frontend nunca calcula a regra definitiva sozinho.
