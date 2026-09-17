# FLUXOS E NAVEGAÇÃO

## 1. Fluxo administrativo principal

`Dashboard → Profissionais/Procedimentos → Regras → Produção → Comissões → Contestações → Fechamento → Relatórios`

O usuário não deve precisar decorar caminhos alternativos.

## 2. Fluxo de preparação inicial

1. Acessar clínica.
2. Cadastrar profissional.
3. Cadastrar procedimento.
4. Criar regra de comissão.
5. Registrar primeira produção.

Quando um pré-requisito faltar, a interface deve orientar o usuário ao item ausente.

## 3. Fluxo de produção

1. Abrir Nova Produção.
2. Selecionar data.
3. Selecionar profissional.
4. Selecionar procedimento.
5. Informar quantidade e valor.
6. Visualizar prévia.
7. Confirmar.
8. Receber confirmação.
9. Ter acesso direto ao detalhe do lançamento.

## 4. Fluxo de contestação

### Profissional
`Procedimento → Detalhe → Contestar → Formulário → Enviar → Minhas Contestações`

### Administrativo
`Contestações → Em análise → Detalhe → Revisar → Aprovar/Rejeitar → Justificar → Concluir`

## 5. Fluxo de fechamento

`Fechamentos → Competência → Revisão → Pendências → Resolver → Confirmar fechamento → Resultado fechado`

Não permitir que a interface dê impressão de fechamento concluído antes da confirmação final.

## 6. Breadcrumbs

Usar no administrativo quando a navegação possuir profundidade:
- Profissionais / Maria Silva
- Procedimentos / Consulta Clínica
- Contestações / #123
- Fechamentos / Agosto 2026

Evitar breadcrumb no portal do profissional quando a barra inferior e o botão voltar forem suficientes.

## 7. Voltar

- Preservar contexto de filtros quando o usuário retorna de um detalhe.
- Não usar o botão voltar como única forma de sair de formulários.
- Se houver alteração não salva, alertar antes de sair.

## 8. Links contextuais

Exemplos:
- do profissional para suas produções;
- do procedimento para suas regras;
- da comissão para a produção de origem;
- da contestação para o lançamento;
- do fechamento para o extrato.

## 9. Navegação por estado

Um item pode abrir uma ação diferente conforme estado:
- contestação em análise → analisar;
- contestação decidida → visualizar;
- período aberto → revisar;
- período fechado → consultar.

## 10. Regras para não perder contexto

Ao abrir detalhe e voltar:
- filtros permanecem;
- paginação permanece;
- período selecionado permanece;
- aba selecionada permanece.
