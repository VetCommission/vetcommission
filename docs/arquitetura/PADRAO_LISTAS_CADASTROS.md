# Padrão de atualização das listas de cadastros

Todo cadastro deve invalidar a consulta da lista após uma criação, edição ou alteração de status.

## Regra

1. O formulário salva pela API.
2. No sucesso, invalida a chave completa do tenant e do domínio:
   `['tenant', activeTenantId, '<recurso>']`.
3. Redireciona para a tela de listagem.
4. A lista é recarregada pelo React Query e exibe o registro atualizado.
5. Em erro, permanece no formulário e apresenta a mensagem ao usuário.

Nunca usar apenas `['tenant', '<recurso>']`, pois essa chave não corresponde às consultas paginadas que incluem o tenant ativo.

O mesmo padrão deve ser aplicado a clínicas, funções/cargos, especialidades, profissionais e aos próximos cadastros.
