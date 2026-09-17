# ESTADOS DA INTERFACE

## 1. Obrigatoriedade

Toda área que consulta ou altera dados deve possuir estados explícitos.

## 2. Carregando

- Usar skeleton quando a estrutura da tela for conhecida.
- Usar indicador simples para ações curtas.
- Evitar tela branca.
- Botões de envio mostram estado de processamento.

## 3. Sem dados

Estrutura:
- ícone ou ilustração discreta;
- título;
- explicação;
- próxima ação quando aplicável.

Exemplos:
“Você ainda não cadastrou profissionais.”
“Não há produções neste período.”
“Você não possui contestações.”

## 4. Sem resultado de filtro

Diferente de sem dados.

Mensagem:
“Nenhum resultado encontrado com os filtros selecionados.”

Ação:
“Limpar filtros”.

## 5. Erro de carregamento

- título claro;
- mensagem humana;
- ação Tentar novamente;
- não expor detalhes internos.

## 6. Acesso não permitido

- explicar que o usuário não possui acesso à área;
- oferecer retorno seguro.

## 7. Registro inativo

Mostrar badge Inativo.
Em detalhe, manter histórico disponível.
Não oferecer ações de uso operacional que contrariem a regra.

## 8. Período fechado

- badge Fechado;
- aviso visual não agressivo;
- campos editáveis tornam-se somente leitura;
- esconder ou desabilitar ações inválidas;
- explicar por que não pode editar.

## 9. Contestação em análise

- badge amarelo;
- linha do tempo;
- nenhuma promessa de aprovação.

## 10. Contestação aprovada

- badge verde;
- decisão;
- justificativa;
- novo valor quando houver alteração.

## 11. Contestação rejeitada

- badge vermelho suave;
- justificativa visível;
- valor original preservado.

## 12. Sucesso

Após ação relevante:
- confirmação curta;
- informação do que ocorreu;
- próximo passo opcional.

## 13. Alerta

Usar para situação que merece atenção sem impedir a continuidade.

## 14. Bloqueio

Usar quando a ação realmente não puder prosseguir.
A mensagem precisa dizer o que falta resolver.
