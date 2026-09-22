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

## Regra obrigatória de testes

Toda funcionalidade nova deve ser entregue com testes correspondentes:

- testes unitários para handlers, validações e regras de negócio;
- testes de integração para persistência, isolamento por tenant/clínica e endpoints HTTP;
- testes de aceitação/E2E quando houver uma jornada completa de frontend.

Uma funcionalidade não deve ser considerada concluída enquanto seu comportamento principal não possuir evidência automatizada adequada ao nível em que opera.

## Regra do modo de edição

O modo (`create`, `view` ou `edit`) deve ser mantido em estado local do formulário. O carregamento dos dados deve ocorrer por `defaultValues` quando a query estiver pronta ou por uma atualização explícita do formulário, sem reinitializar o componente nem alterar o modo atual. O botão `Editar` deve ser `type="button"`, para não submeter o formulário nem redirecionar a tela.
