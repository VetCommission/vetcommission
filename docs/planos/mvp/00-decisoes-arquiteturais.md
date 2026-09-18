# Decisões arquiteturais bloqueantes do MVP

> Estado: revisão em andamento. D07, D09, D12, D13 e D14 foram definidas nesta revisão; D01–D06, D08, D10 e D11 permanecem mantidas e aguardam aprovação formal da página. Nenhum script de domínio ou contrato público deve ser criado antes dessa aprovação.

## Objetivo

Eliminar ambiguidades que alterariam tabelas, APIs, autorização ou histórico financeiro. Após aprovação, cada decisão relevante deve ser registrada como ADR em `docs/arquitetura/decisoes/`.

**Branch:** `<linear-id>-decisoes-arquiteturais`
**PR:** documental, contendo decisões aprovadas, ADRs e ajustes nos contratos afetados.

## Decisões em revisão

### D01 — Tenant, clínica e unidade

**Recomendação:** `Tenant` representa o cliente/assinante do SaaS. Um tenant possui uma ou mais `Clinic`; uma clínica poderá possuir unidades apenas em versão futura, salvo necessidade confirmada do piloto.

**Consequências:** todo dado isolado recebe `tenant_id`; dados operacionais também recebem `clinic_id` quando pertencem a uma clínica. O header seleciona tenant; seleção de clínica é contexto funcional separado. Um tenant com uma única clínica recebe essa clínica automaticamente.

### D02 — Cálculo síncrono e Worker

**Recomendação:** prévia e cálculo de uma produção são síncronos e executados pelo mesmo serviço de domínio. Worker fica preparado como executável, mas só processa recálculo/lote, recuperação de jobs e Outbox quando um caso real entrar no escopo.

**Consequências:** resposta imediata e transação simples para produção; nenhuma fila prematura.

### D03 — Competência

**Recomendação:** `ClosingPeriod` é criado sob demanda por tenant/clínica e mês, com intervalo fechado-aberto `[início, fim)`. A data do procedimento define a competência segundo o timezone configurado da clínica; persistência de instantes técnicos permanece UTC.

**Consequências:** unicidade por tenant, clínica, ano e mês; produção exige competência aberta.

### D04 — Vigência e sobreposição de regras

**Recomendação:** vigência também usa intervalo `[início, fim)`. Não pode haver sobreposição entre regras do mesmo tenant, clínica, procedimento e escopo. Regra específica do profissional prevalece sobre a geral.

**Consequências:** validação transacional e proteção no PostgreSQL; conflitos retornam `409 Conflict`.

### D05 — Precisão e arredondamento

**Recomendação:** valores monetários em `numeric(18,2)`; percentuais com quatro casas decimais; cálculo intermediário em `decimal`; resultado monetário arredondado para duas casas com `MidpointRounding.AwayFromZero` por lançamento.

**Consequências:** totais somam comissões já arredondadas por lançamento, evitando divergência entre extrato e fechamento.

### D06 — Snapshot e correção

**Recomendação:** produção e comissão preservam o snapshot aplicado. Correção em período aberto cria versão/auditoria antes-depois e recalcula na mesma transação; não apagar nem sobrescrever a evidência histórica. Período fechado não reabre no MVP.

### D07 — Identidade e primeiro acesso

**Decisão:** o MVP não possui autocadastro público. Somente usuários previamente cadastrados podem acessar o sistema. O administrador inicial continua sendo provisionado por processo operacional controlado, conforme os padrões definidos para o projeto. A recuperação automatizada de senha fica fora do primeiro corte do MVP e deverá ser implementada posteriormente.

**Consequências:** não criar fluxo público de cadastro nem funcionalidade completa de "Esqueci minha senha" no MVP. A autenticação somente será permitida para usuários previamente cadastrados e ativos. A tela de login não depende de recuperação automatizada de senha.

### D08 — Profissional e usuário

**Recomendação:** `Professional` é cadastro de negócio; `User` é identidade. O vínculo é opcional e único dentro do tenant. E-mails podem diferir. Um usuário profissional pode ter vínculos em tenants distintos, mas somente um profissional por tenant.

### D09 — Contestação

**Decisão:** cada produção poderá possuir uma contestação em andamento. A contestação funciona como um fluxo contínuo de análise e interação entre profissional e clínica até sua resolução final. Enquanto estiver em análise, novas informações, justificativas e evidências textuais deverão ser registradas no mesmo processo, preservando uma linha do tempo completa e ordenada.

A contestação somente será concluída quando for `Aprovada` ou `Rejeitada`.

**Consequências:** toda contestação não resolvida bloqueia o fechamento da competência. Não criar novas contestações para representar novas interações sobre o mesmo caso. Profissional e clínica complementam o processo original, e seu histórico permanece integralmente vinculado à contestação. Depois da resolução, ela deixa de bloquear o fechamento, mas o resultado e a justificativa final permanecem disponíveis. Arquivos anexados ficam fora do primeiro corte; evidências e complementações textuais fazem parte do MVP.

### D10 — Fechamento

**Recomendação:** fechamento cria snapshot consolidado dos totais por profissional e marca as produções/comissões participantes. A operação é transacional, idempotente e protegida contra concorrência. Não há reabertura no MVP.

**Compatibilidade com contestação:** qualquer contestação ainda não resolvida, isto é, sem decisão final `Aprovada` ou `Rejeitada`, impede o fechamento da competência.

### D11 — Exclusão

**Recomendação:** cadastros de negócio usam ativação/inativação. Exclusão física não é exposta pela API do MVP, mesmo antes de uso.

### D12 — Corte do primeiro MVP

**Decisão:** landing comercial, pagamentos, anexos, exportação, Slack e recuperação automatizada de senha ficam fora do primeiro corte operacional do MVP. Relatórios serão inicialmente consultivos.

**Consequências:** esses itens não bloqueiam a entrega nem a validação do fluxo principal do MVP e permanecem documentados como possibilidades de evoluções posteriores.

### D13 — Valor da produção

**Decisão:** o valor padrão do procedimento funciona apenas como sugestão para novos lançamentos. A produção preserva o valor unitário efetivamente utilizado, a quantidade e o valor total do lançamento.

**Consequências:** alterações posteriores no valor padrão do procedimento não modificam produções, comissões, contestações ou fechamentos já existentes. Todo cálculo histórico utiliza o valor preservado na produção. Em conjunto com D05 e D06, os valores preservados usam a precisão definida e integram o snapshot auditável do lançamento.

### D14 — Data efetiva da produção

**Decisão:** a data de realização do procedimento é a data efetiva de negócio da produção. Ela determina a competência, a regra de comissão aplicável, a validação de período aberto e a composição dos relatórios por competência. A data de criação do registro no sistema possui finalidade de auditoria e não altera a competência do lançamento.

**Consequências:** um procedimento realizado no último dia de uma competência e registrado posteriormente continua pertencendo à competência da data em que foi efetivamente realizado. A regra selecionada é a vigente nessa data, conforme os intervalos definidos em D03 e D04.

Exemplo:

```text
Procedimento realizado: 31/08
Produção cadastrada:    02/09
Competência:            Agosto
Regra aplicável:        vigente em 31/08
```

## Status desta revisão

| Decisão | Situação |
|---|---|
| D01–D06 | Mantidas; aguardam aprovação formal da página |
| D07 | Ajustada conforme decisão de produto |
| D08 | Mantida; aguarda aprovação formal da página |
| D09 | Ajustada para fluxo contínuo até resolução |
| D10–D11 | Mantidas; aguardam aprovação formal da página |
| D12 | Resolvida para o primeiro corte operacional |
| D13 | Criada: valor efetivamente praticado na produção |
| D14 | Criada: data de realização como data efetiva de negócio |

## Aprovação necessária

Para cada decisão, registrar uma das respostas:

```text
APROVADA
APROVADA COM AJUSTE: <decisão explícita>
REJEITADA: <alternativa escolhida>
```

Uma resposta vaga não libera a fundação dependente. Depois da aprovação, decisões com impacto duradouro viram ADRs numerados e este arquivo passa a apontar para elas.

## Critério de conclusão

- D01 a D14 possuem decisão explícita ou aprovação formal registrada.
- Não há contradição com requisitos ou contratos arquiteturais.
- Os nomes de conceitos em português e os nomes físicos em inglês estão definidos.
- Fundação, identidade e primeiro script de domínio podem ser planejados sem bifurcações.
