# REQUISITOS_NEGOCIO_MVP

## 1. Visão Geral

O sistema tem como objetivo controlar, calcular, acompanhar e fechar as comissões de profissionais que realizam procedimentos em clínicas veterinárias.

O MVP deve permitir que a clínica cadastre sua estrutura básica, profissionais, procedimentos e regras de comissão, registre a produção realizada, calcule as comissões correspondentes, permita a conferência dos valores, ofereça um canal de contestação ao profissional e finalize o período por meio de um fechamento mensal.

O foco do MVP é validar o fluxo de negócio de ponta a ponta com simplicidade, rastreabilidade e transparência para a clínica e para os profissionais.

---

## 2. Objetivos do MVP

O MVP deverá atender aos seguintes objetivos de negócio:

- Centralizar o cadastro dos profissionais da clínica.
- Centralizar o cadastro dos procedimentos realizados pela clínica.
- Permitir a definição de regras simples de comissão.
- Registrar os procedimentos realizados pelos profissionais.
- Calcular automaticamente a comissão de cada lançamento.
- Permitir que a clínica acompanhe e confira os valores calculados.
- Permitir que o profissional acompanhe sua produção e suas comissões.
- Permitir que o profissional conteste um lançamento antes do fechamento do período.
- Permitir que a clínica analise e resolva contestações.
- Consolidar os valores em um fechamento mensal.
- Preservar o histórico dos valores, regras aplicadas e decisões tomadas.
- Fornecer informações resumidas e detalhadas para consulta e relatório.

---

## 3. Perfis de Usuário

### 3.1. Administrador da Clínica

Responsável pela configuração e gestão do processo de comissionamento.

Principais responsabilidades:

- Gerenciar profissionais.
- Gerenciar procedimentos.
- Configurar regras de comissão.
- Acompanhar a produção.
- Conferir comissões.
- Analisar contestações.
- Realizar o fechamento mensal.
- Consultar relatórios e indicadores.
- Corrigir informações enquanto o período estiver aberto.

---

### 3.2. Usuário Operacional

Responsável principalmente pelo registro da produção diária.

Principais responsabilidades:

- Registrar procedimentos realizados.
- Consultar lançamentos.
- Corrigir lançamentos enquanto permitido.
- Consultar profissionais e procedimentos necessários para o registro.

O acesso a configurações, regras de comissão, contestações e fechamento poderá ser restrito conforme a política definida pela clínica.

---

### 3.3. Profissional

Usuário que realiza procedimentos e recebe comissão.

Principais responsabilidades:

- Consultar sua produção.
- Consultar suas comissões.
- Consultar seus fechamentos.
- Consultar detalhes dos lançamentos.
- Abrir contestações.
- Acompanhar o andamento de suas contestações.
- Consultar seus próprios dados cadastrais.

O profissional não deverá visualizar informações financeiras ou produtivas de outros profissionais.

---

# 4. Fluxo Geral do Negócio

O fluxo principal do sistema deverá seguir esta sequência:

1. Cadastro da clínica.
2. Cadastro dos usuários.
3. Cadastro dos profissionais.
4. Cadastro dos procedimentos.
5. Definição das regras de comissão.
6. Registro da produção.
7. Cálculo da comissão.
8. Conferência dos lançamentos.
9. Disponibilização para o profissional.
10. Contestação, quando necessário.
11. Análise e resolução da contestação.
12. Fechamento mensal.
13. Consulta dos resultados e relatórios.

---

# 5. Etapa 1 — Cadastro da Clínica

## 5.1. Objetivo

Representar a organização responsável pelos profissionais, procedimentos, produções, regras e fechamentos.

## 5.2. Necessidades do Negócio

O sistema deverá permitir registrar os dados básicos da clínica.

Informações mínimas:

- Nome da clínica.
- Nome fantasia, quando aplicável.
- Documento de identificação da empresa.
- Telefone.
- E-mail.
- Endereço.
- Situação ativa ou inativa.

No MVP, cada operação deverá estar vinculada a uma clínica.

## 5.3. Entidades Envolvidas

### Clínica

Representa a empresa que utiliza o sistema.

Relacionamentos principais:

- Possui usuários.
- Possui profissionais.
- Possui procedimentos.
- Possui regras de comissão.
- Possui produções.
- Possui comissões.
- Possui fechamentos.

---

# 6. Etapa 2 — Usuários e Acesso

## 6.1. Objetivo

Identificar as pessoas que podem acessar o sistema e definir o tipo de atuação de cada uma.

## 6.2. Necessidades do Negócio

Cada usuário deverá possuir:

- Nome.
- E-mail de acesso.
- Situação ativa ou inativa.
- Perfil de acesso.
- Vínculo com uma clínica.

Perfis mínimos:

- Administrador.
- Operacional.
- Profissional.

O profissional deverá possuir acesso apenas às informações relacionadas à própria atuação.

## 6.3. Entidades Envolvidas

### Usuário

Representa a pessoa que acessa o sistema.

### Perfil de Usuário

Representa o tipo de acesso e atuação do usuário.

### Profissional

Quando o usuário for um profissional comissionado, deverá existir um vínculo entre o usuário e seu cadastro profissional.

---

# 7. Etapa 3 — Cadastro de Profissionais

## 7.1. Objetivo

Manter o cadastro das pessoas que realizam procedimentos e podem receber comissão.

## 7.2. Necessidades do Negócio

O sistema deverá permitir:

- Cadastrar profissional.
- Editar dados do profissional.
- Ativar profissional.
- Inativar profissional.
- Consultar profissionais cadastrados.
- Consultar histórico de produção e comissão do profissional.

Informações mínimas:

- Nome.
- E-mail.
- Telefone, quando informado.
- Função ou cargo.
- Registro profissional, quando aplicável.
- Especialidade, quando aplicável.
- Situação ativa ou inativa.
- Clínica à qual pertence.

## 7.3. Regras de Negócio

- Profissionais inativos não deverão ser selecionados para novos lançamentos.
- Um profissional que já possua movimentações históricas não deverá ser removido do histórico.
- A inativação não deverá eliminar produções, comissões ou fechamentos anteriores.
- Cada produção deverá identificar claramente o profissional responsável.

## 7.4. Entidades Envolvidas

### Profissional

Representa a pessoa que realizou o procedimento.

### Clínica

Define a organização à qual o profissional pertence.

### Usuário

Permite que o profissional acesse seu portal, quando aplicável.

---

# 8. Etapa 4 — Cadastro de Procedimentos

## 8.1. Objetivo

Manter o catálogo de serviços ou atividades que podem gerar produção e comissão.

## 8.2. Necessidades do Negócio

O sistema deverá permitir:

- Cadastrar procedimento.
- Editar procedimento.
- Ativar procedimento.
- Inativar procedimento.
- Consultar procedimentos.
- Organizar procedimentos por categoria.

Informações mínimas:

- Nome.
- Categoria.
- Valor padrão.
- Descrição opcional.
- Situação ativa ou inativa.
- Clínica.

Exemplos:

- Consulta clínica.
- Vacinação.
- Cirurgia.
- Exame laboratorial.
- Procedimento estético.

## 8.3. Regras de Negócio

- Procedimentos inativos não deverão ser utilizados em novos lançamentos.
- O valor padrão servirá como sugestão para o lançamento.
- O valor do lançamento poderá ser diferente do valor padrão quando permitido pela clínica.
- Alterações no valor padrão não deverão alterar lançamentos históricos.
- Cada produção deverá preservar a quantidade, o valor unitário efetivamente praticado e o valor total do lançamento.

## 8.4. Entidades Envolvidas

### Procedimento

Representa o serviço realizado.

### Categoria de Procedimento

Agrupa procedimentos semelhantes.

### Clínica

Define a organização responsável pelo procedimento.

---

# 9. Etapa 5 — Regras de Comissão

## 9.1. Objetivo

Definir como o valor da comissão será calculado para cada procedimento.

## 9.2. Tipos de Regra no MVP

O MVP deverá suportar apenas dois tipos de regra:

### Percentual

A comissão corresponde a um percentual sobre o valor do procedimento.

Exemplo:

- Procedimento: R$ 150,00
- Percentual: 30%
- Comissão: R$ 45,00

### Valor Fixo

A comissão corresponde a um valor fixo por unidade ou procedimento realizado.

Exemplo:

- Quantidade: 2
- Valor fixo de comissão: R$ 20,00
- Comissão: R$ 40,00

## 9.3. Escopo da Regra

Uma regra poderá ser:

- Geral para determinado procedimento.
- Específica para determinado profissional e procedimento.

## 9.4. Prioridade

Quando existir uma regra específica para o profissional, ela deverá prevalecer sobre a regra geral do procedimento.

## 9.5. Vigência

A regra deverá permitir informar:

- Data inicial de vigência.
- Data final de vigência, quando aplicável.

O sistema deverá considerar a regra válida na data em que o procedimento foi realizado.

## 9.6. Preservação do Histórico

O lançamento deverá preservar as informações da regra utilizada no momento do cálculo.

Alterações futuras na regra não deverão alterar automaticamente comissões já calculadas.

## 9.7. Entidades Envolvidas

### Regra de Comissão

Representa a forma de cálculo.

### Procedimento

Identifica o procedimento ao qual a regra se aplica.

### Profissional

Utilizado quando a regra for específica.

### Clínica

Define a organização responsável pela regra.

---

# 10. Etapa 6 — Registro de Produção

## 10.1. Objetivo

Registrar cada procedimento efetivamente realizado.

## 10.2. Informações do Lançamento

O lançamento deverá conter:

- Data de realização do procedimento.
- Profissional.
- Procedimento.
- Quantidade.
- Valor unitário.
- Valor total.
- Observação opcional.
- Usuário responsável pelo registro.
- Data e hora do registro.

## 10.3. Fluxo

Ao registrar uma produção:

1. O usuário seleciona a data em que o procedimento foi realizado.
2. Seleciona o profissional.
3. Seleciona o procedimento.
4. Informa a quantidade.
5. Confirma ou altera o valor unitário.
6. Informa observação, quando necessário.
7. O sistema verifica os dados informados.
8. O sistema identifica a regra de comissão válida.
9. O sistema calcula a comissão.
10. O lançamento é registrado.
11. A comissão fica disponível para conferência.

## 10.4. Regras de Negócio

- O profissional deverá estar ativo na data do novo lançamento.
- O procedimento deverá estar ativo.
- A quantidade deverá ser maior que zero.
- O valor deverá ser válido.
- A data de realização determina a competência, a regra de comissão vigente e a validação de período aberto.
- A data e hora de cadastro possuem finalidade de auditoria e não alteram a competência da produção.
- Deverá existir uma regra de comissão aplicável para realizar o cálculo.
- O lançamento deverá permanecer vinculado à regra efetivamente utilizada.
- Enquanto o período estiver aberto, o lançamento poderá ser corrigido por usuário autorizado.
- Após o fechamento, o lançamento não deverá ser alterado pelo fluxo normal.

## 10.5. Entidades Envolvidas

### Produção

Representa o procedimento realizado.

### Profissional

Responsável pela execução.

### Procedimento

Identifica o serviço realizado.

### Regra de Comissão

Define o cálculo.

### Comissão

Armazena o resultado calculado.

### Usuário

Identifica quem registrou a produção.

---

# 11. Etapa 7 — Cálculo da Comissão

## 11.1. Objetivo

Determinar o valor que o profissional deverá receber por cada lançamento.

## 11.2. Cálculo Percentual

Valor da comissão:

**Valor total da produção × percentual da regra**

## 11.3. Cálculo por Valor Fixo

Valor da comissão:

**Quantidade × valor fixo da regra**

## 11.4. Informações a Preservar

Cada comissão deverá registrar:

- Produção de origem.
- Profissional.
- Procedimento.
- Regra utilizada.
- Tipo de cálculo.
- Percentual aplicado, quando houver.
- Valor fixo aplicado, quando houver.
- Valor base.
- Valor calculado.
- Situação da comissão.
- Data do cálculo.

## 11.5. Regras de Negócio

- Cada produção deverá possuir o resultado da comissão correspondente.
- O cálculo deverá utilizar a regra válida na data do procedimento.
- O cálculo histórico deverá utilizar quantidade, valor unitário e valor total preservados na produção, sem depender do valor padrão atual do procedimento.
- Alterações futuras na regra não deverão modificar comissões históricas automaticamente.
- Quando um lançamento for corrigido antes do fechamento, a comissão poderá ser recalculada.
- Toda alteração relevante deverá manter rastreabilidade.

## 11.6. Entidades Envolvidas

### Comissão

Representa o valor calculado.

### Produção

Origem da comissão.

### Regra de Comissão

Regra efetivamente utilizada.

### Profissional

Beneficiário da comissão.

---

# 12. Etapa 8 — Conferência

## 12.1. Objetivo

Permitir que a clínica revise os lançamentos antes do fechamento.

## 12.2. Necessidades do Negócio

O administrador deverá conseguir consultar:

- Produções do período.
- Comissões calculadas.
- Profissionais envolvidos.
- Valores de produção.
- Valores de comissão.
- Situação de cada lançamento.
- Pendências existentes.

Filtros mínimos:

- Período.
- Profissional.
- Procedimento.
- Situação.

## 12.3. Ações

Enquanto o período estiver aberto, o administrador poderá:

- Consultar lançamento.
- Corrigir lançamento.
- Conferir valores.
- Identificar inconsistências.
- Preparar o período para fechamento.

## 12.4. Entidades Envolvidas

### Produção

### Comissão

### Profissional

### Procedimento

### Período de Fechamento

---

# 13. Etapa 9 — Portal do Profissional

## 13.1. Objetivo

Dar transparência ao profissional sobre sua produção e seus valores.

## 13.2. Dashboard do Profissional

O profissional deverá visualizar:

- Quantidade de procedimentos no período.
- Valor total de produção.
- Comissão total do período.
- Evolução de suas comissões.
- Últimos procedimentos.
- Situação do período atual.

## 13.3. Meus Procedimentos

O profissional deverá consultar:

- Data.
- Procedimento.
- Quantidade.
- Valor do procedimento.
- Comissão calculada.
- Situação.

Deverá ser possível consultar o detalhe de cada lançamento.

## 13.4. Detalhe do Procedimento

O detalhe deverá apresentar:

- Procedimento.
- Data.
- Quantidade.
- Valor unitário.
- Valor total.
- Regra utilizada.
- Percentual ou valor fixo aplicado.
- Comissão calculada.
- Observações.
- Situação.

## 13.5. Minhas Comissões

O profissional deverá consultar:

- Comissão total do período.
- Comissão por procedimento.
- Histórico por período.
- Detalhamento dos lançamentos.

## 13.6. Extrato de Comissão

O profissional deverá possuir um extrato contendo:

- Data.
- Procedimento.
- Valor de produção.
- Valor da comissão.
- Situação.

## 13.7. Regras de Negócio

- O profissional visualizará somente seus próprios registros.
- O profissional não poderá alterar produções.
- O profissional poderá contestar um lançamento enquanto o período permitir.
- Lançamentos de períodos fechados serão apenas consultivos.

## 13.8. Entidades Envolvidas

### Profissional

### Produção

### Comissão

### Período de Fechamento

### Contestação

---

# 14. Etapa 10 — Contestação

## 14.1. Objetivo

Permitir que o profissional questione um lançamento ou comissão antes do fechamento.

## 14.2. Abertura da Contestação

O profissional deverá:

1. Selecionar um lançamento.
2. Informar o motivo.
3. Descrever a divergência.
4. Informar o valor esperado, quando aplicável.
5. Informar evidência textual, quando necessário.
6. Enviar a contestação.

## 14.3. Motivos Iniciais

O MVP poderá considerar motivos como:

- Valor do procedimento divergente.
- Procedimento incorreto.
- Quantidade incorreta.
- Comissão incorreta.
- Procedimento não registrado.
- Outro.

## 14.4. Situações da Contestação

Situações mínimas:

- Em análise.
- Aprovada.
- Rejeitada.

Enquanto estiver em análise, a contestação permanece aberta como um único fluxo contínuo. Profissional e clínica poderão complementar informações, justificativas e evidências textuais no mesmo processo, preservando uma linha do tempo completa e ordenada.

## 14.5. Análise

O administrador deverá consultar:

- Profissional.
- Lançamento contestado.
- Motivo.
- Descrição.
- Valor original.
- Valor informado pelo profissional.
- Evidências textuais e complementações.
- Histórico da solicitação.

O administrador poderá:

- Aprovar.
- Rejeitar.
- Registrar justificativa.
- Solicitar ou registrar complementações textuais enquanto a contestação estiver em análise.

## 14.6. Contestação Aprovada

Quando uma contestação aprovada exigir alteração do lançamento:

1. O lançamento deverá ser ajustado.
2. A comissão deverá ser recalculada.
3. O valor anterior deverá permanecer rastreável.
4. A decisão deverá ser registrada.
5. O profissional deverá visualizar o novo resultado.

## 14.7. Contestação Rejeitada

A contestação deverá ser encerrada mantendo:

- Valores originais.
- Justificativa da rejeição.
- Data da decisão.
- Responsável pela decisão.

## 14.8. Regras de Negócio

- Somente lançamentos do próprio profissional poderão ser contestados.
- Cada produção poderá possuir somente uma contestação em andamento.
- Novas interações sobre o mesmo caso deverão ser registradas na contestação original, sem criar outra contestação.
- Não será permitida nova alteração normal em período já fechado.
- O fechamento não deverá ocorrer enquanto existir qualquer contestação ainda não resolvida.
- A contestação somente deixa de bloquear o fechamento depois da decisão final `Aprovada` ou `Rejeitada`.
- Arquivos anexados não fazem parte do primeiro corte do MVP.
- Toda decisão deverá manter histórico.

## 14.9. Entidades Envolvidas

### Contestação

### Produção

### Comissão

### Profissional

### Usuário

### Histórico da Contestação

---

# 15. Etapa 11 — Fechamento Mensal

## 15.1. Objetivo

Consolidar os valores de produção e comissão de determinado período.

## 15.2. Período

O fechamento deverá representar um intervalo de competência, normalmente mensal.

Exemplo:

- Abril/2026.
- Maio/2026.

## 15.3. Visão do Fechamento

O administrador deverá visualizar:

- Profissionais envolvidos.
- Quantidade de procedimentos por profissional.
- Valor total produzido.
- Valor total de comissão.
- Pendências.
- Contestações abertas.
- Situação do período.

## 15.4. Situações do Período

Situações mínimas:

- Em aberto.
- Em fechamento.
- Fechado.

## 15.5. Regras para Fechar

Antes do fechamento, o sistema deverá verificar:

- Se existem lançamentos pendentes.
- Se existem inconsistências identificadas.
- Se existem contestações em aberto.
- Se as comissões estão calculadas.

A clínica deverá confirmar explicitamente o fechamento.

## 15.6. Após o Fechamento

Após o fechamento:

- Produções ficam bloqueadas para alterações normais.
- Comissões ficam bloqueadas para alterações normais.
- O período passa a ser apenas consultivo.
- O profissional poderá visualizar o resultado final.
- O histórico deverá ser preservado.

## 15.7. Entidades Envolvidas

### Período de Fechamento

### Produção

### Comissão

### Profissional

### Contestação

### Clínica

---

# 16. Etapa 12 — Registro de Pagamento

## 16.1. Escopo do MVP

O registro de pagamento não faz parte do primeiro corte operacional do MVP. Esta seção permanece documentada apenas como evolução futura.

## 16.2. Informações Mínimas

- Profissional.
- Período.
- Valor.
- Data do pagamento.
- Observação.
- Comprovante opcional.

## 16.3. Importante

O sistema não realizará:

- Transferência bancária.
- Folha de pagamento.
- Emissão fiscal.
- Cálculo tributário.

## 16.4. Entidades Envolvidas

### Pagamento

### Profissional

### Período de Fechamento

### Comissão

---

# 17. Etapa 13 — Dashboard Administrativo

## 17.1. Objetivo

Apresentar uma visão resumida da operação da clínica.

## 17.2. Indicadores Principais

O dashboard deverá apresentar, para o período selecionado:

- Valor total de produção.
- Valor total de comissões.
- Quantidade de procedimentos.
- Quantidade de profissionais ativos.
- Situação do fechamento.
- Quantidade de contestações pendentes.

## 17.3. Análises

O MVP deverá apresentar visões como:

- Produção ao longo do período.
- Comissão ao longo do período.
- Procedimentos por categoria.
- Comissões por profissional.
- Últimos lançamentos.

## 17.4. Entidades Envolvidas

O dashboard utilizará informações consolidadas de:

- Produção.
- Comissão.
- Profissional.
- Procedimento.
- Categoria de Procedimento.
- Contestação.
- Período de Fechamento.

---

# 18. Etapa 14 — Relatórios

## 18.1. Objetivo

Permitir que a clínica consulte informações consolidadas e detalhadas.

No primeiro corte, os relatórios serão consultivos. Exportação de arquivos será tratada em evolução futura.

## 18.2. Relatórios do MVP

### Comissões por Profissional

Apresentar:

- Profissional.
- Quantidade de procedimentos.
- Valor produzido.
- Valor de comissão.

### Produção por Profissional

Apresentar os procedimentos realizados por determinado profissional.

### Produção por Procedimento

Apresentar volume, valores e profissionais relacionados a determinado procedimento.

### Resumo do Período

Apresentar:

- Total de produção.
- Total de comissão.
- Profissionais.
- Procedimentos.
- Contestações.
- Situação do fechamento.

### Extrato de Comissão

Apresentar o detalhamento dos lançamentos que compõem a comissão.

## 18.3. Filtros

Filtros mínimos:

- Período.
- Profissional.
- Procedimento.
- Situação.

## 18.4. Entidades Envolvidas

### Produção

### Comissão

### Profissional

### Procedimento

### Contestação

### Período de Fechamento

---

# 19. Histórico e Auditoria

## 19.1. Objetivo

Preservar a rastreabilidade das operações que influenciam valores financeiros.

## 19.2. Operações Relevantes

O sistema deverá manter histórico de ações como:

- Inclusão de produção.
- Alteração de produção.
- Alteração que gere recálculo.
- Abertura de contestação.
- Aprovação de contestação.
- Rejeição de contestação.
- Fechamento de período.
- Registro de pagamento, somente em evolução futura.

## 19.3. Informações Mínimas

Para cada ação relevante, deverá ser possível identificar:

- O que foi alterado.
- Valor anterior, quando aplicável.
- Novo valor, quando aplicável.
- Usuário responsável.
- Data e hora.
- Motivo ou justificativa, quando aplicável.

## 19.4. Entidade Envolvida

### Histórico / Auditoria

Representa a rastreabilidade das operações relevantes.

---

# 20. Estados Principais do Negócio

## 20.1. Produção

Estados sugeridos:

- Registrada.
- Calculada.
- Conferida.
- Em contestação.
- Aprovada.
- Fechada.

Nem toda produção precisará passar explicitamente por todos os estados.

---

## 20.2. Contestação

- Em análise.
- Aprovada.
- Rejeitada.

---

## 20.3. Fechamento

- Em aberto.
- Em fechamento.
- Fechado.

---

## 20.4. Profissional

- Ativo.
- Inativo.

---

## 20.5. Procedimento

- Ativo.
- Inativo.

---

## 20.6. Regra de Comissão

- Ativa.
- Inativa.
- Encerrada por vigência.

---

# 21. Entidades do MVP

Abaixo está o conjunto principal de entidades do MVP.

## 21.1. Clínica

Representa a empresa usuária do sistema.

---

## 21.2. Usuário

Representa a pessoa com acesso ao sistema.

---

## 21.3. Perfil de Usuário

Define o tipo de atuação do usuário.

---

## 21.4. Profissional

Representa o profissional que realiza procedimentos.

---

## 21.5. Categoria de Procedimento

Organiza o catálogo de procedimentos.

---

## 21.6. Procedimento

Representa o serviço realizado.

---

## 21.7. Regra de Comissão

Define como a comissão será calculada.

---

## 21.8. Produção

Representa um procedimento efetivamente realizado.

---

## 21.9. Comissão

Representa o resultado financeiro calculado para uma produção.

---

## 21.10. Contestação

Representa um questionamento realizado pelo profissional.

---

## 21.11. Histórico da Contestação

Representa a evolução e as decisões de uma contestação.

---

## 21.12. Período de Fechamento

Representa o agrupamento mensal dos lançamentos.

---

## 21.13. Pagamento

Entidade prevista somente para evolução futura e fora do primeiro corte operacional do MVP.

---

## 21.14. Auditoria

Representa o histórico das operações relevantes.

---

# 22. Relações Principais entre as Entidades

Visão simplificada:

```text
Clínica
 ├── Usuários
 ├── Profissionais
 ├── Procedimentos
 │    └── Categoria de Procedimento
 ├── Regras de Comissão
 ├── Produções
 │    ├── Profissional
 │    ├── Procedimento
 │    └── Comissão
 ├── Contestações
 │    └── Histórico da Contestação
 ├── Períodos de Fechamento
 │    └── Comissões / Produções
 ├── Pagamentos (evolução futura)
 └── Auditoria
```

Fluxo principal das entidades:

```text
Profissional
     +
Procedimento
     +
Regra de Comissão
     ↓
Produção
     ↓
Comissão
     ↓
Conferência
     ↓
Contestação (opcional)
     ↓
Fechamento
     ↓
Pagamento (evolução futura; fora do primeiro corte)
```

---

# 23. Telas do Administrador

O MVP deverá possuir as seguintes áreas administrativas:

1. Login.
2. Dashboard.
3. Lista de profissionais.
4. Cadastro/edição de profissional.
5. Lista de procedimentos.
6. Cadastro/edição de procedimento.
7. Lista de regras de comissão.
8. Cadastro/edição de regra de comissão.
9. Registro de produção.
10. Consulta de produções.
11. Consulta de comissões.
12. Lista de contestações.
13. Detalhe e análise da contestação.
14. Fechamento mensal.
15. Detalhe do fechamento.
16. Relatórios.
17. Configurações básicas da clínica.
18. Dados do usuário.

---

# 24. Telas do Profissional

O MVP deverá possuir as seguintes áreas para o profissional:

1. Login.
2. Primeiro acesso / boas-vindas.
3. Dashboard do profissional.
4. Meus procedimentos.
5. Detalhe do procedimento.
6. Minhas comissões.
7. Extrato de comissão.
8. Nova contestação.
9. Minhas contestações.
10. Detalhe da contestação.
11. Fechamentos mensais.
12. Meu perfil.

---

# 25. Regras Gerais Obrigatórias

## RN-001 — Isolamento por Clínica

As informações de uma clínica não deverão ser visíveis para outra clínica.

---

## RN-002 — Histórico Financeiro

Alterações em cadastros ou regras não poderão modificar automaticamente resultados financeiros históricos.

---

## RN-003 — Regra Vigente

A comissão deverá utilizar a regra válida na data em que o procedimento foi realizado.

---

## RN-004 — Prioridade da Regra Específica

Quando existir regra específica para o profissional, ela deverá ter prioridade sobre a regra geral do procedimento.

---

## RN-005 — Profissional Inativo

Profissionais inativos não poderão receber novos lançamentos.

---

## RN-006 — Procedimento Inativo

Procedimentos inativos não poderão ser utilizados em novos lançamentos.

---

## RN-007 — Período Aberto

Somente lançamentos pertencentes a períodos abertos poderão ser alterados pelo processo normal.

---

## RN-008 — Período Fechado

Um período fechado será somente consultivo no fluxo normal.

---

## RN-009 — Contestação

O profissional somente poderá contestar lançamentos próprios. Cada caso deverá permanecer em uma única contestação contínua até sua resolução final, com todas as interações registradas no mesmo histórico.

---

## RN-010 — Contestação e Fechamento

Toda contestação ainda não resolvida bloqueará a conclusão do fechamento. Somente decisões finais `Aprovada` ou `Rejeitada` removem esse bloqueio.

---

## RN-011 — Recalculo

Quando uma alteração válida modificar os dados utilizados no cálculo, a comissão deverá ser recalculada antes do fechamento.

---

## RN-012 — Rastreabilidade

Alterações financeiras relevantes deverão manter histórico.

---

## RN-013 — Exclusão de Dados Históricos

Cadastros já utilizados em movimentações não deverão ser removidos de forma que o histórico fique incompleto.

---

## RN-014 — Visibilidade do Profissional

O profissional visualizará somente seus próprios lançamentos, comissões, contestações e fechamentos.

---

## RN-015 — Fechamento

O fechamento deverá consolidar os valores calculados do período e preservar esse resultado para consulta histórica.

---

# 26. Fora do Escopo do MVP

Os itens abaixo não fazem parte do MVP inicial:

- Integração com sistemas veterinários externos.
- Integração com prontuário ou sistema hospitalar.
- Integração com folha de pagamento.
- Transferência bancária.
- Cálculo de impostos.
- Emissão de nota fiscal.
- Emissão de recibo fiscal.
- Regras de comissão por metas.
- Regras progressivas ou escalonadas.
- Regras baseadas em equipe.
- Rateio complexo entre vários profissionais.
- Campanhas de bonificação.
- Programa de metas.
- Aplicativo móvel nativo.
- Notificações por WhatsApp.
- Marketplace.
- Agenda veterinária.
- Prontuário de animais.
- Cadastro clínico de animais.
- Estoque.
- Faturamento completo da clínica.
- Contabilidade.

---

# 27. Fluxo de Validação do MVP

Para validar o MVP, deverá ser possível executar integralmente o seguinte cenário:

1. Cadastrar uma clínica.
2. Cadastrar um administrador.
3. Cadastrar um profissional.
4. Cadastrar um procedimento.
5. Configurar uma regra de comissão.
6. Registrar um procedimento realizado.
7. Calcular automaticamente a comissão.
8. Visualizar o lançamento na área administrativa.
9. Visualizar o mesmo lançamento na área do profissional.
10. Permitir que o profissional conteste o lançamento.
11. Permitir que o administrador analise a contestação.
12. Corrigir e recalcular o lançamento quando necessário.
13. Resolver a contestação.
14. Consolidar todos os lançamentos do período.
15. Realizar o fechamento mensal.
16. Permitir que o profissional consulte o resultado fechado.
17. Consultar o resultado por meio dos relatórios.

Se esse fluxo puder ser executado de ponta a ponta com segurança, transparência e histórico preservado, o objetivo principal do MVP estará atendido.

---

# 28. Sequência Recomendada de Construção Funcional

Esta sequência representa dependência de negócio, não uma definição tecnológica.

## Fase Funcional 1 — Base Cadastral

- Clínica.
- Usuários.
- Profissionais.
- Categorias.
- Procedimentos.

Resultado esperado:

A clínica possui a estrutura mínima para iniciar a configuração do comissionamento.

---

## Fase Funcional 2 — Comissão

- Regras de comissão.
- Vigência.
- Regra geral.
- Regra específica por profissional.

Resultado esperado:

O sistema consegue determinar como cada procedimento deverá gerar comissão.

---

## Fase Funcional 3 — Produção

- Registro de produção.
- Consulta de lançamentos.
- Correção de lançamentos.
- Cálculo automático da comissão.

Resultado esperado:

A produção diária passa a gerar valores de comissão.

---

## Fase Funcional 4 — Área do Profissional

- Dashboard.
- Meus procedimentos.
- Minhas comissões.
- Extrato.

Resultado esperado:

O profissional passa a acompanhar os valores calculados para ele.

---

## Fase Funcional 5 — Contestação

- Abertura.
- Consulta.
- Análise.
- Aprovação.
- Rejeição.
- Ajuste e recálculo.

Resultado esperado:

Divergências podem ser resolvidas antes do fechamento.

---

## Fase Funcional 6 — Fechamento

- Consolidação mensal.
- Validação de pendências.
- Fechamento.
- Consulta histórica.

Resultado esperado:

A clínica consegue concluir formalmente um período de comissão.

---

## Fase Funcional 7 — Gestão e Relatórios

- Dashboard administrativo.
- Indicadores.
- Relatórios.
- Extratos.
- Histórico e auditoria.

Resultado esperado:

A clínica consegue acompanhar a operação e consultar resultados históricos.

---

# 29. Critério de Sucesso do MVP

O MVP será considerado funcionalmente validado quando a clínica conseguir controlar um ciclo completo de comissão sem depender de planilhas paralelas para o processo principal.

O ciclo esperado é:

**Cadastrar → Configurar → Registrar → Calcular → Conferir → Contestar → Resolver → Fechar → Consultar**

A prioridade do MVP deve permanecer na simplicidade do fluxo, transparência dos cálculos e preservação do histórico.
