# CENÁRIOS DE ACEITE POR JORNADA

## Jornada A — Primeira configuração

Dado que a clínica iniciou o uso,
quando cadastrar profissional, procedimento e regra,
então deve conseguir chegar ao registro de produção sem ambiguidades.

Aceite UX:
- cada passo possui ação clara;
- pré-requisito ausente é explicado;
- usuário sabe qual próximo passo executar.

## Jornada B — Registrar produção

Dado um profissional e procedimento válidos,
quando o usuário preencher a produção,
então deve ver o valor total e uma prévia da comissão antes de confirmar.

Aceite UX:
- produção e comissão são diferenciadas;
- regra utilizada é identificável;
- a data de realização determina a competência, a regra aplicável e a validação do período aberto;
- o valor padrão do procedimento é apenas uma sugestão editável;
- quantidade, valor unitário efetivamente praticado e valor total ficam preservados no lançamento;
- confirmação de sucesso é exibida.

## Jornada C — Profissional consulta comissão

Dado que existem produções calculadas,
quando o profissional acessar seu dashboard,
então deve encontrar sua comissão do período rapidamente.

Aceite UX:
- comissão aparece em primeiro nível;
- período é claro;
- há acesso ao extrato.

## Jornada D — Entender um cálculo

Dado um lançamento,
quando abrir o detalhe,
então o profissional deve compreender de onde surgiu a comissão.

Aceite UX:
- base;
- regra;
- cálculo;
- resultado.

## Jornada E — Contestar

Dado um lançamento contestável,
quando o profissional escolher contestar,
então o contexto do lançamento deve acompanhar o formulário.

Aceite UX:
- não precisa redigitar dados do lançamento;
- motivo e descrição são claros;
- envio gera confirmação;
- status pode ser acompanhado;
- complementações e evidências textuais permanecem na mesma contestação até a decisão final;
- anexos de arquivos não são exigidos nem oferecidos no primeiro corte.

## Jornada F — Analisar contestação

Dado uma contestação em análise,
quando o administrador abrir o detalhe,
então deve comparar registro e solicitação e tomar decisão justificada.

Aceite UX:
- valores comparáveis;
- evidências textuais e complementações acessíveis em ordem cronológica;
- novas interações permanecem vinculadas à contestação original;
- aprovar/rejeitar claramente separados;
- justificativa registrada.

## Jornada G — Fechar período

Dado que o período está em aberto,
quando o administrador iniciar o fechamento,
então pendências devem aparecer antes da confirmação.

Aceite UX:
- resumo do período;
- todas as contestações ainda não resolvidas são apresentadas e bloqueiam o fechamento;
- inconsistências;
- consequência do fechamento;
- confirmação explícita.

## Jornada H — Consultar período fechado

Dado um período fechado,
quando admin ou profissional acessá-lo,
então deve perceber que é histórico e não editável pelo fluxo normal.

Aceite UX:
- badge Fechado;
- campos somente leitura;
- totais finais;
- extrato disponível.
