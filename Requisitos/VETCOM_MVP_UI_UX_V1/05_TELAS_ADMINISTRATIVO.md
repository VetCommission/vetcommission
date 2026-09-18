# TELAS — ÁREA ADMINISTRATIVA

## 1. Dashboard

### Objetivo
Responder rapidamente:
- quanto foi produzido;
- quanto será pago em comissão;
- quantos procedimentos ocorreram;
- quantos profissionais estão ativos;
- existem pendências antes do fechamento?

### Estrutura
Topo:
- título Dashboard;
- período selecionado;
- ação opcional de registrar produção.

Primeira linha:
- Produção total;
- Comissão total;
- Procedimentos realizados;
- Profissionais ativos.

Segunda linha:
- gráfico Produção x Comissão;
- gráfico Procedimentos por Categoria.

Terceira linha:
- últimas produções;
- contestações pendentes ou situação do fechamento.

### Estados
- com dados;
- sem movimento;
- carregando;
- erro;
- período fechado.

## 2. Profissionais — Lista

Topo:
- título;
- descrição;
- botão Novo profissional.

Filtros:
- busca por nome;
- função;
- status.

Tabela:
- avatar;
- nome;
- função;
- registro profissional;
- status;
- ações.

Ações:
- visualizar;
- editar;
- ativar/inativar.

## 3. Profissional — Cadastro/Edição

Seções:
1. Dados pessoais.
2. Dados profissionais.
3. Contato.
4. Situação.
5. Vínculo de acesso, quando aplicável.

Ações:
- cancelar;
- salvar.

Em edição, mostrar histórico ou resumo financeiro apenas como atalho, não dentro do formulário.

## 4. Procedimentos — Lista

Topo:
- título;
- botão Novo procedimento.

Filtros:
- nome;
- categoria;
- status.

Tabela:
- nome;
- categoria;
- valor padrão;
- status;
- regra resumida;
- ações.

## 5. Procedimento — Cadastro/Edição

Campos na ordem:
1. nome;
2. categoria;
3. valor padrão;
4. descrição;
5. situação.

Rodapé:
- cancelar;
- salvar.

## 6. Regras de Comissão — Lista

Topo:
- título;
- explicação curta;
- botão Nova regra.

Filtros:
- procedimento;
- profissional;
- tipo;
- vigência;
- status.

Tabela:
- procedimento;
- escopo;
- tipo;
- valor/percentual;
- início;
- fim;
- status;
- ações.

## 7. Regra de Comissão — Cadastro/Edição

Fluxo visual:
1. escolher procedimento;
2. definir se é geral ou específica;
3. se específica, escolher profissional;
4. escolher tipo;
5. informar percentual ou valor fixo;
6. informar vigência;
7. revisar resumo;
8. salvar.

Mostrar uma prévia textual:
“Para uma produção de R$ X, esta regra resultaria em R$ Y.”

## 8. Produção — Novo Lançamento

Topo:
- título;
- contexto do período.

Campos:
- data de realização do procedimento;
- profissional;
- procedimento;
- quantidade;
- valor unitário;
- observação.

O valor padrão do procedimento deve preencher o valor unitário apenas como sugestão editável. O lançamento deve preservar a quantidade, o valor unitário efetivamente praticado e o valor total.

A data de realização determina a competência, a regra de comissão vigente e a validação de período aberto. A data de criação do registro é somente informação de auditoria.

Bloco de prévia:
- valor total;
- regra encontrada;
- comissão estimada.

Ações:
- cancelar;
- registrar produção.

## 9. Produção — Consulta

Filtros:
- período;
- profissional;
- procedimento;
- status.

Tabela:
- data;
- profissional;
- procedimento;
- quantidade;
- produção;
- comissão;
- status;
- ações.

Ações:
- detalhe;
- editar se permitido.

## 10. Produção — Detalhe

Blocos:
- identificação;
- dados da produção;
- regra aplicada;
- cálculo;
- comissão;
- histórico;
- contestação relacionada, se houver.

## 11. Comissões

Topo:
- período;
- filtros.

Resumo:
- total de produção;
- total de comissão;
- quantidade de profissionais.

Tabela:
- profissional;
- quantidade de procedimentos;
- produção;
- comissão;
- situação.

## 12. Contestações — Lista

Abas:
- Em análise;
- Aprovadas;
- Rejeitadas.

Tabela/lista:
- data;
- profissional;
- procedimento;
- motivo;
- valor;
- status;
- ação Ver.

## 13. Contestação — Análise

Cabeçalho:
- status;
- profissional;
- identificação do lançamento.

Comparativo:
- valor registrado;
- valor informado/esperado;
- motivo;
- descrição;
- evidências textuais e complementações.

Histórico:
- abertura;
- interações;
- decisão.

Enquanto não houver decisão final, profissional e clínica devem registrar novas mensagens, justificativas e evidências textuais na mesma contestação, em ordem cronológica. Não haverá anexo de arquivo no primeiro corte do MVP.

Ações:
- rejeitar;
- aprovar.

Toda decisão deve abrir espaço para justificativa.

## 14. Fechamentos — Lista

Mostrar competências:
- mês/ano;
- produção;
- comissão;
- profissionais;
- pendências;
- situação.

Ação:
- abrir fechamento.

## 15. Fechamento — Detalhe

Topo:
- competência;
- badge de situação.

Resumo:
- profissionais;
- procedimentos;
- produção;
- comissão;
- contestações pendentes.

Tabela:
- profissional;
- quantidade;
- produção;
- comissão;
- situação.

Ações:
- revisar pendências;
- finalizar período.

Antes de finalizar, mostrar confirmação clara e consequência do bloqueio.

## 16. Relatórios

Topo:
- título;
- filtros;
- indicação de consulta do período selecionado.

Conteúdo:
- resumo numérico;
- gráfico quando útil;
- tabela detalhada.

Relatórios iniciais:
- comissões por profissional;
- produção por profissional;
- produção por procedimento;
- resumo do período;
- extrato de comissão.

Os relatórios do primeiro corte são consultivos. A exportação de arquivos fica para evolução futura.

## 17. Configurações da Clínica

Blocos:
- identificação;
- contato;
- endereço;
- situação.

Evitar misturar configurações operacionais complexas neste MVP.

## 18. Perfil do Usuário

- nome;
- e-mail;
- perfil;
- clínica;
- alterar senha;
- sair.
