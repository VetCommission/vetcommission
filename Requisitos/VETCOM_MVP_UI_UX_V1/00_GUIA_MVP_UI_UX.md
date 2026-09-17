# GUIA DO PACOTE — MVP UI/UX

## 1. Finalidade

Este pacote é a fonte de referência para requisitos de produto, experiência, interface, navegação, posicionamento, comportamento visual e critérios de aceite do MVP do sistema de comissionamento para clínicas veterinárias.

Ele foi consolidado a partir de todo o alinhamento realizado anteriormente e dos mockups aprovados.

O objetivo é reduzir interpretações durante o desenvolvimento. Sempre que uma tela, estado, fluxo ou componente for construído, deve ser possível localizar neste pacote:

- qual problema a tela resolve;
- quem utiliza a tela;
- quais informações aparecem;
- em qual ordem aparecem;
- qual é a ação principal;
- quais ações secundárias existem;
- quais estados visuais devem existir;
- como a tela se comporta em tamanhos diferentes;
- quais mensagens devem ser exibidas;
- quais critérios precisam ser atendidos para considerar a experiência concluída.

## 2. O que este pacote NÃO define

Este pacote não define arquitetura técnica, persistência, integrações, banco de dados, bibliotecas, frameworks, linguagem de programação, infraestrutura ou estratégia de publicação.

Esses temas devem permanecer nos documentos técnicos existentes do projeto.

## 3. Ordem recomendada de leitura

1. `01_REQUISITOS_NEGOCIO_MVP.md`
2. `02_BASE_UI_UX_APROVADA.md`
3. `03_DESIGN_SYSTEM_MVP.md`
4. `04_LAYOUT_E_POSICIONAMENTO.md`
5. `05_TELAS_ADMINISTRATIVO.md`
6. `06_TELAS_PROFISSIONAL.md`
7. `07_FLUXOS_E_NAVEGACAO.md`
8. `08_FORMULARIOS_E_VALIDACOES_UX.md`
9. `09_ESTADOS_DA_INTERFACE.md`
10. `10_FEEDBACK_E_MENSAGENS.md`
11. `11_RESPONSIVIDADE_E_MOBILE.md`
12. `12_ACESSIBILIDADE_E_USABILIDADE.md`
13. `13_CRITERIOS_DE_ACEITE_VISUAL.md`
14. `14_MAPA_TELAS_COMPONENTES.md`
15. `15_REFERENCIAS_VISUAIS_MVP.md`
16. `16_CHECKLIST_REVISAO_UI_UX.md`
17. `17_GLOSSARIO_E_CONVENCOES.md`
18. `18_MATRIZ_PERFIS_TELAS_ACOES.md`
19. `19_CENARIOS_DE_ACEITE_POR_JORNADA.md`
20. `20_PROMPT_CODEX_REPLICACAO_UI_UX.md`

## 4. Regra de precedência

Quando houver dúvida:

1. Regra de negócio explícita prevalece sobre conveniência visual.
2. Mockup aprovado orienta composição e linguagem visual.
3. Documento especializado prevalece sobre uma descrição genérica.
4. Critérios de acessibilidade e legibilidade prevalecem sobre fidelidade pixel a pixel.
5. O desenvolvimento não deve criar novos fluxos de negócio sem que estejam documentados.

## 5. Princípio central

O produto deve ser percebido como:

**simples para operar, transparente para conferir e confiável para fechar comissões.**

A interface deve reduzir esforço mental e explicar os valores ao usuário, especialmente ao profissional que recebe comissão.
