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

## 4. Percentuais

- Exibir símbolo `%`.
- Explicar limites válidos quando houver.
- Não exigir que o usuário calcule manualmente a comissão.

## 5. Datas

- Exibir formato local.
- Em vigências, deixar claro “início” e “fim”.
- Em competência, preferir mês/ano quando dia não for relevante.

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
Evidência é opcional no MVP.

## 11. Fechamento

Antes de fechar:
- exibir resumo;
- listar pendências;
- pedir confirmação;
- explicar que lançamentos ficarão bloqueados pelo fluxo normal.
