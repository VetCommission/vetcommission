# FORMULÁRIOS E VALIDAÇÕES DE UX

## 1. Princípios

- Validar próximo do campo.
- Explicar como corrigir.
- Não apagar dados após erro.
- Não mostrar erro antes de o usuário interagir, exceto em tentativa de envio.
- Diferenciar campo obrigatório de opcional.

## 2. Campos obrigatórios

Usar indicador consistente.
Não depender apenas de asterisco; quando útil, informar “Obrigatório”.

## 3. Valores monetários

- Exibir no padrão monetário local.
- Alinhar valores de forma consistente.
- Evitar ambiguidade entre valor unitário, total e comissão.
- Em cálculo, mostrar memória simples quando isso aumentar transparência.
- No lançamento de produção, tratar o valor padrão do procedimento somente como sugestão editável.
- Preservar no lançamento a quantidade, o valor unitário efetivamente praticado e o valor total, sem depender de alterações futuras no cadastro do procedimento.

## 4. Percentuais

- Exibir símbolo `%`.
- Explicar limites válidos quando houver.
- Não exigir que o usuário calcule manualmente a comissão.

## 5. Datas

- Exibir formato local.
- Em vigências, deixar claro “início” e “fim”.
- Em competência, preferir mês/ano quando dia não for relevante.
- Identificar a data de realização do procedimento como data de negócio da produção.
- Usar a data de criação do registro apenas como informação de auditoria.

## 6. Selects

- Busca quando houver listas maiores.
- Não permitir selecionar inativos para novos lançamentos.
- Mostrar contexto suficiente para diferenciar itens homônimos.

## 7. Mensagem de erro

Estrutura:
**o que aconteceu + como corrigir**

Evitar:
- “Erro inválido”
- “Falha 400”
- códigos internos

Preferir:
- “Selecione um profissional para continuar.”
- “Informe um valor maior que zero.”
- “Não existe regra de comissão válida para a data selecionada.”

## 8. Salvamento

Ao salvar:
- indicar processamento;
- impedir duplo envio;
- confirmar sucesso;
- direcionar para próximo contexto apropriado.

## 9. Cancelamento

Se nada foi alterado:
- sair diretamente.

Se houve alteração:
- confirmar descarte.

## 10. Formulário de contestação

A descrição deve aceitar explicação suficiente.
Motivo deve ser selecionável.
Valor esperado é contextual, não obrigatório em todos os motivos.
Evidência textual é opcional no MVP; anexos de arquivos ficam fora do primeiro corte.
Enquanto estiver em análise, novas mensagens, justificativas e complementações textuais devem ser adicionadas à mesma contestação e exibidas em ordem cronológica.
Não permitir uma segunda contestação em andamento para a mesma produção.

## 11. Fechamento

Antes de fechar:
- exibir resumo;
- listar pendências;
- pedir confirmação;
- explicar que lançamentos ficarão bloqueados pelo fluxo normal.
