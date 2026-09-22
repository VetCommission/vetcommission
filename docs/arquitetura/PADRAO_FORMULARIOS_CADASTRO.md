# Padrão compartilhado de formulários

Todos os cadastros devem usar `frontend/src/components/forms/CrudFormShell.tsx`.

O componente centraliza:

- modos `create`, `view` e `edit`;
- botão Editar sem submit;
- botão Salvar somente nos modos de criação/edição;
- retorno para a lista;
- prevenção de submit acidental ao editar;
- layout comum dos formulários.

Cada domínio continua responsável por seus campos, validações, mutation e invalidação de cache. O shell deve ser a única fonte da lógica dos botões e da transição visual entre consulta e edição.
