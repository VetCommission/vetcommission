# UI_UX_MVP

## 1. Objetivo do Documento

Este documento define o padrão visual e estrutural do MVP do sistema de comissionamento para clínicas veterinárias, tomando como base o histórico aprovado nas conversas e os mockups já validados.

O objetivo é permitir que o Codex replique com fidelidade a experiência visual do produto, mantendo consistência entre telas administrativas e área do profissional.

Este documento descreve:

- identidade visual;
- paleta de cores;
- tipografia;
- estilos de componentes;
- organização dos layouts;
- posicionamento dos elementos principais por tela;
- comportamento visual esperado por contexto de uso;
- referência dos mockups já aprovados.

---

## 2. Referência Visual Oficial da V1

A implementação da V1 deve seguir como referência visual oficial os mockups já aprovados durante a definição do MVP.

### 2.1. Referência 1 — Visão geral do sistema

Arquivo de referência visual principal:

`/mnt/data/ghostwriter_images/context/5e02a0c6-e647-59ce-8552-2274ce291fae.png`

Essa referência contém:

1. Tela de aterrissagem / landing page.
2. Tela de login.
3. Paleta e elementos visuais.
4. Dashboard administrativo.
5. Lista de profissionais.
6. Cadastro de procedimento.
7. Regras de comissão.
8. Lançamento de produção.
9. Fechamento mensal.
10. Contestações.
11. Portal do profissional.
12. Tela de relatórios.

### 2.2. Referência 2 — Jornada do profissional

Arquivo de referência visual complementar:

`/mnt/data/a_clean_ui_ux_storyboard_infographic_for_a_veterin.png`

Essa referência contém o fluxo visual da área do profissional:

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
11. Fechamento mensal.
12. Meu perfil.

### 2.3. Regra de uso

Sempre que houver conflito entre interpretação textual e aparência visual, a implementação deverá preservar o padrão aprovado nos mockups, desde que não comprometa usabilidade, legibilidade ou consistência funcional.

---

## 3. Direção Visual do Produto

A identidade do produto deve transmitir:

- organização;
- transparência;
- leveza;
- confiança;
- proximidade com a área veterinária;
- experiência simples e amigável.

A interface não deve parecer hospitalar, fria ou excessivamente corporativa. O sistema deve equilibrar aparência profissional com sensação acolhedora.

### 3.1. Princípios visuais

1. Visual limpo.
2. Muito espaço em branco.
3. Baixa poluição visual.
4. Informações financeiras em destaque, mas sem exagero.
5. Navegação simples e previsível.
6. Cards e tabelas com leitura rápida.
7. Interface amigável para públicos administrativos e profissionais.
8. Portal do profissional com foco mobile-first.

---

## 4. Paleta de Cores Oficial

A paleta deve seguir o padrão validado nos mockups.

## 4.1. Cores principais

### Primária
- **Hex:** `#0F7056`
- Uso: marca, botões primários, destaques positivos, cabeçalhos de destaque, ícones principais.

### Secundária
- **Hex:** `#10B981`
- Uso: reforço visual, badges positivos, gráficos, indicadores e destaques complementares.

### Destaque / Ação
- **Hex:** `#F59E0B`
- Uso: alertas suaves, elementos de atenção, indicadores intermediários e ênfase pontual.

## 4.2. Cores neutras

### Texto principal
- **Hex:** `#1F2937`
- Uso: títulos, números, textos principais.

### Texto secundário
- **Hex:** `#667280`
- Uso: subtítulos, explicações, labels de apoio.

### Borda / cinza médio
- **Hex:** `#BFC7D0`
- Uso: bordas suaves, linhas divisórias, estados neutros.

### Fundo geral
- **Hex:** `#F5F7F8`
- Uso: fundo base da aplicação.

### Branco
- **Hex:** `#FFFFFF`
- Uso: cards, inputs, tabelas, modais e áreas de conteúdo.

## 4.3. Cores auxiliares de contexto

### Fundo suave verde
- **Hex:** `#F0FDF4`
- Uso: áreas leves de boas-vindas, blocos positivos, destaques suaves.

### Fundo suave azul
- **Hex:** `#EFF6FF`
- Uso: blocos informativos, filtros, indicadores neutros.

### Fundo suave amarelo
- **Hex:** `#FFF7E7`
- Uso: estados de atenção, aviso, pendência.

### Fundo suave cinza
- **Hex:** `#F8FAFC`
- Uso: superfícies auxiliares e diferenciação sutil de blocos.

## 4.4. Cores de status

### Sucesso / ativo / aprovado / fechado
- Cor base: `#10B981`
- Fundo suave: `#EAFBF3`

### Em análise / pendente / atenção
- Cor base: `#F59E0B`
- Fundo suave: `#FFF4D6`

### Erro / rejeitado / inativo crítico
- Cor base: `#DC2626`
- Fundo suave: `#FEECEC`

### Informativo
- Cor base: `#2563EB`
- Fundo suave: `#EAF1FF`

### Neutro / inativo
- Cor base: `#94A3B8`
- Fundo suave: `#F1F5F9`

## 4.5. Regras de aplicação da cor

- O verde escuro deve ser o principal elemento de identidade.
- O verde claro deve complementar, sem disputar com o primário.
- O amarelo deve ser usado com moderação.
- Vermelho deve ficar restrito a rejeições, alertas fortes e ações destrutivas.
- O fundo geral deve permanecer claro.
- A interface não deve usar blocos escuros em excesso.

---

## 5. Tipografia Oficial

A tipografia aprovada é **Inter**.

A tipografia deve transmitir clareza, modernidade e neutralidade.

## 5.1. Escala tipográfica

### Título principal de página
- Tamanho: 28 a 32 px
- Peso: 700
- Cor: `#1F2937`

### Título de seção
- Tamanho: 22 a 24 px
- Peso: 700
- Cor: `#1F2937`

### Subtítulo / título interno
- Tamanho: 18 a 20 px
- Peso: 600
- Cor: `#1F2937`

### Título de card
- Tamanho: 16 px
- Peso: 600
- Cor: `#1F2937`

### Texto principal
- Tamanho: 14 a 16 px
- Peso: 400
- Cor: `#1F2937`

### Texto auxiliar
- Tamanho: 12 a 13 px
- Peso: 400
- Cor: `#667280`

### Label de campo
- Tamanho: 13 a 14 px
- Peso: 500
- Cor: `#1F2937`

### Número de KPI
- Tamanho: 24 a 28 px
- Peso: 700
- Cor: `#1F2937`

### Valor financeiro em destaque
- Tamanho: 24 a 30 px
- Peso: 700
- Cor: `#0F7056`

## 5.2. Regras tipográficas

- Evitar excesso de pesos diferentes.
- Priorizar 400, 500, 600 e 700.
- Textos longos devem ter leitura confortável.
- Não utilizar fonte decorativa.
- Cabeçalhos e CTAs devem manter hierarquia forte e simples.

---

## 6. Espaçamento, Borda e Profundidade

## 6.1. Espaçamento

A interface deve seguir uma lógica consistente de espaçamento.

Sugestão de escala visual:

- 4 px: micro espaçamento.
- 8 px: espaçamento pequeno.
- 12 px: espaçamento interno reduzido.
- 16 px: espaçamento padrão entre elementos próximos.
- 24 px: espaçamento de blocos.
- 32 px: espaçamento entre seções.
- 40 px ou mais: respiro em áreas mais amplas.

## 6.2. Bordas

- Inputs, cards, tabelas e painéis com bordas suaves.
- Cantos arredondados médios.
- Padrão visual recomendado: 10 px a 16 px de raio, dependendo do componente.

## 6.3. Sombras

Sombras suaves e discretas.

O sistema não deve parecer “pesado”.

Aplicação recomendada:

- Card: sombra muito leve.
- Modal: sombra moderada.
- Dropdown e menus: sombra discreta.

---

## 7. Componentes Base

## 7.1. Botões

### Botão primário

Uso:
- salvar;
- entrar;
- criar novo item;
- confirmar ação principal.

Estilo:
- fundo verde primário;
- texto branco;
- cantos arredondados;
- peso de fonte médio;
- altura confortável.

### Botão secundário

Uso:
- cancelar;
- voltar;
- ações auxiliares.

Estilo:
- fundo branco ou muito claro;
- borda suave;
- texto verde ou cinza escuro.

### Botão de perigo

Uso:
- excluir;
- sair;
- ação destrutiva.

Estilo:
- destaque em vermelho, preferencialmente com uso contido.

## 7.2. Inputs

Todos os campos devem seguir padrão uniforme:

- fundo branco;
- borda cinza suave;
- raio arredondado;
- label acima do campo;
- placeholder discreto;
- mensagens de erro abaixo do campo, quando aplicável.

## 7.3. Selects

Mesmo padrão visual dos inputs.

## 7.4. Cards

Uso amplo para:
- KPIs;
- blocos de resumo;
- blocos de fechamento;
- destaques financeiros;
- atalhos.

Estilo:
- fundo branco;
- borda muito suave ou sombra leve;
- cantos arredondados;
- cabeçalho simples;
- conteúdo com boa separação.

## 7.5. Tabelas

As tabelas devem seguir padrão limpo:

- cabeçalho claro e legível;
- linhas com boa altura;
- ações no final da linha;
- status com badges;
- busca e filtro acima;
- sem excesso de linhas pesadas.

## 7.6. Badges de status

Padrão de badge:
- cantos arredondados;
- cor de fundo suave;
- texto curto;
- tamanho compacto.

Exemplos:
- Ativo;
- Inativo;
- Em análise;
- Aprovada;
- Rejeitada;
- Fechado;
- Calculada.

## 7.7. Navegação

### Sidebar administrativa
- fixa à esquerda;
- clara;
- com ícones simples;
- item ativo destacado por preenchimento suave verde;
- logo na parte superior;
- informações da clínica no rodapé da sidebar.

### Topbar administrativa
- campo de busca;
- área de notificações;
- avatar/nome do usuário;
- seletor de período quando necessário.

### Navegação da área do profissional
- mobile-first;
- barra inferior com 5 itens principais;
- ícones simples;
- item ativo destacado.

---

## 8. Estrutura Global do Layout Administrativo

O layout administrativo deve seguir o padrão:

1. Sidebar à esquerda.
2. Área principal à direita.
3. Topbar no topo da área principal.
4. Conteúdo em cards, tabelas, filtros e formulários.

## 8.1. Sidebar

Itens esperados:

- Início
- Profissionais
- Procedimentos
- Produção
- Comissões
- Fechamentos
- Relatórios
- Configurações

Observações:

- O item ativo deve ter destaque visual.
- Os ícones devem ser discretos e lineares.
- A sidebar deve ser compacta, mas confortável.

## 8.2. Área principal

Ordem de leitura esperada:

1. Título da página.
2. Subtítulo explicativo curto.
3. Filtros ou ações principais.
4. Conteúdo principal.

## 8.3. Padrão de página administrativa

Toda página administrativa deve responder claramente:

- onde o usuário está;
- o que essa tela faz;
- qual a ação principal;
- quais dados estão disponíveis;
- como voltar ou editar.

---

## 9. Estrutura Global da Área do Profissional

A área do profissional deve ser simples, direta e prioritariamente orientada a consulta.

## 9.1. Padrão visual

- prioridade para leitura rápida;
- blocos resumidos;
- navegação simples;
- foco no que é “meu”;
- aparência acolhedora;
- layout otimizado para celular.

## 9.2. Navegação inferior esperada

Itens principais:

- Início
- Produções
- Comissões
- Contestações
- Perfil

## 9.3. Estrutura das páginas do profissional

1. Cabeçalho com logo ou nome do sistema.
2. Saudação ou título da página.
3. Filtro de período quando aplicável.
4. Resumos em cards.
5. Lista ou extrato.
6. Navegação inferior fixa.

---

## 10. Especificação Visual por Tela — Área Pública

## 10.1. Landing Page

### Objetivo visual
Apresentar o produto de forma amigável, leve e confiável.

### Estrutura

#### Cabeçalho
Posicionamento:
- logo à esquerda;
- links de navegação no centro ou próximo do centro;
- botão “Entrar” no lado direito.

#### Conteúdo principal (hero)
Divisão em duas colunas:

**Coluna esquerda:**
- título principal forte;
- texto de apoio;
- dois botões principais.

**Coluna direita:**
- imagem principal com apelo emocional ligado ao universo veterinário.

### Elementos obrigatórios
- logo do produto;
- mensagem principal;
- CTA principal;
- CTA secundário;
- imagem com animais;
- visual acolhedor.

### Estilo
- fundo claro;
- grande respiro;
- cores suaves;
- hero limpo;
- sem excesso de informação.

---

## 10.2. Tela de Login

### Objetivo visual
Gerar confiança, simplicidade e fácil entrada no sistema.

### Estrutura
Layout em duas áreas verticais ou colunas:

**Área esquerda ou central:**
- logo;
- mensagem de boas-vindas;
- campos de login;
- ações de acesso.

**Área direita:**
- imagem humana/veterinária acolhedora;
- frase institucional.

### Elementos obrigatórios
- e-mail;
- senha;
- lembrar de mim;
- esqueci minha senha;
- botão entrar.

### Observação
Se login social for usado futuramente, ele não deve quebrar o padrão visual aprovado.

---

## 11. Especificação Visual por Tela — Administrativo

## 11.1. Dashboard Administrativo

### Objetivo visual
Apresentar rapidamente a situação da operação.

### Estrutura da tela

#### Linha 1
- título “Dashboard” à esquerda;
- filtro de período à direita.

#### Linha 2 — KPIs
Exibir quatro cards lado a lado:
- faturamento total;
- total de comissões;
- procedimentos realizados;
- profissionais ativos.

#### Linha 3 — Gráficos
Dois blocos principais:
- gráfico de barras para faturamento x comissão;
- gráfico circular para procedimentos por categoria.

### Estilo
- cards brancos;
- números em destaque;
- gráficos simples;
- sem poluição.

---

## 11.2. Lista de Profissionais

### Objetivo visual
Permitir consulta rápida e gestão do cadastro.

### Estrutura da tela

Parte superior:
- título da página;
- texto curto de apoio;
- botão “Novo profissional”.

Abaixo:
- tabela principal.

### Colunas esperadas
- foto/avatar pequeno;
- nome;
- função;
- CRMV, quando houver;
- status;
- ações.

### Ações visuais
- editar;
- visualizar;
- ativar/inativar.

---

## 11.3. Cadastro de Procedimento

### Objetivo visual
Tela simples, objetiva e orientada a formulário.

### Estrutura da tela

- breadcrumb ou contexto acima do formulário;
- título da página;
- formulário central;
- ações ao final.

### Ordem dos campos
1. Nome do procedimento.
2. Categoria.
3. Valor padrão.
4. Descrição.
5. Status.

### Rodapé de ação
- botão cancelar;
- botão salvar.

---

## 11.4. Regras de Comissão

### Objetivo visual
Tornar fácil a leitura das regras configuradas.

### Estrutura da tela

Topo:
- título;
- subtítulo;
- botão “Nova regra”.

Conteúdo:
- tabela com regras cadastradas.

### Colunas esperadas
- procedimento;
- profissional/função;
- tipo;
- valor;
- status.

### Observação visual
Valores percentuais e valores fixos devem ser facilmente distinguíveis.

---

## 11.5. Lançamento de Produção

### Objetivo visual
Permitir preenchimento rápido e seguro.

### Estrutura da tela

Campos em formulário vertical ou em duas colunas suaves:
- data;
- profissional;
- procedimento;
- quantidade;
- valor;
- observação.

### Rodapé de ação
- cancelar;
- salvar.

### Comportamento visual desejado
- clareza no fluxo;
- possibilidade de leitura imediata do que está sendo registrado;
- aparência administrativa leve.

---

## 11.6. Fechamento Mensal

### Objetivo visual
Consolidar informações do período de maneira segura e clara.

### Estrutura da tela

Topo:
- título da tela;
- seletor do período;
- botão “Finalizar período”.

Abaixo:
- cards de resumo do fechamento.

Resumo esperado:
- número de profissionais;
- número de procedimentos;
- total de comissões;
- situação do período.

Abaixo dos cards:
- tabela por profissional.

### Observação visual
A ação de fechar período deve ter destaque, mas sem parecer agressiva.

---

## 11.7. Contestações

### Objetivo visual
Facilitar triagem e decisão.

### Estrutura da tela

Topo:
- título;
- explicação curta.

Conteúdo:
- abas por status (em aberto, aprovadas, rejeitadas);
- tabela/lista de contestações.

### Colunas esperadas
- data;
- profissional;
- procedimento;
- motivo;
- status.

### Estilo
- destaque visual para situação da contestação;
- leitura rápida;
- fácil identificação do que precisa de ação.

---

## 11.8. Tela de Relatórios

### Objetivo visual
Permitir consulta consolidada e exportação.

### Estrutura da tela

Topo:
- título;
- filtros principais.

Centro:
- gráfico principal ou resumo visual;
- tabela complementar.

Rodapé superior ou lateral de ação:
- exportar PDF;
- exportar Excel.

### Observação visual
A tela deve parecer limpa e analítica.

---

## 12. Especificação Visual por Tela — Área do Profissional

## 12.1. Login do Profissional

Deve seguir o mesmo padrão de identidade do produto, porém com foco mais pessoal.

Elementos:
- logo;
- título de boas-vindas;
- e-mail;
- senha;
- lembrar de mim;
- recuperar senha;
- botão entrar;
- imagem leve de apoio na área inferior.

---

## 12.2. Primeiro Acesso / Boas-vindas

### Objetivo visual
Receber o profissional de forma amigável.

### Estrutura
- foto/avatar do profissional;
- saudação personalizada;
- resumo do que poderá fazer no portal;
- botão “Começar”.

### Estilo
- amigável;
- acolhedor;
- focado em onboarding.

---

## 12.3. Dashboard do Profissional

### Objetivo visual
Resumo pessoal do período.

### Estrutura

Topo:
- saudação;
- seletor de período.

KPIs:
- quantidade de procedimentos;
- comissão do período;
- produção total;
- ticket médio.

Bloco adicional:
- gráfico de evolução;
- lista de últimos procedimentos.

### Observação
O tom da interface deve ser positivo e transparente.

---

## 12.4. Meus Procedimentos

### Objetivo visual
Lista clara dos procedimentos realizados.

### Estrutura
- título da página;
- seletor de período;
- lista de lançamentos.

### Cada item deve exibir
- data;
- nome do procedimento;
- valor da comissão;
- resumo do valor do procedimento;
- status.

---

## 12.5. Detalhe do Procedimento

### Objetivo visual
Mostrar claramente como aquele valor foi gerado.

### Estrutura
Blocos informativos contendo:
- nome do procedimento;
- data;
- quantidade;
- valor unitário;
- valor total;
- regra de comissão;
- comissão calculada;
- observações;
- status;
- data de registro.

### Importante
Essa tela deve reforçar transparência.

---

## 12.6. Minhas Comissões

### Objetivo visual
Apresentar visão resumida e por composição.

### Estrutura
Topo:
- filtro de período.

Bloco principal:
- total de comissões do período.

Abaixo:
- distribuição por procedimento;
- composição financeira detalhada.

---

## 12.7. Extrato de Comissão

### Objetivo visual
Exibir histórico em formato de extrato.

### Estrutura
- seletor de período;
- tabela ou lista detalhada.

### Colunas esperadas
- data;
- procedimento;
- valor da produção;
- valor da comissão.

---

## 12.8. Nova Contestação

### Objetivo visual
Facilitar abertura de uma solicitação.

### Estrutura
Campos:
- procedimento/lançamento;
- motivo;
- descrição;
- anexo opcional.

### Rodapé de ação
- enviar contestação.

### Requisito visual
A tela deve ser simples e segura, sem intimidar o usuário.

---

## 12.9. Minhas Contestações

### Objetivo visual
Mostrar andamento das solicitações.

### Estrutura
- abas por situação;
- lista das contestações.

### Cada item deve exibir
- data;
- procedimento;
- motivo resumido;
- status.

---

## 12.10. Detalhe da Contestação

### Objetivo visual
Dar clareza sobre o andamento.

### Estrutura
Bloco superior:
- procedimento contestado;
- data;
- status.

Bloco central:
- valor original;
- valor informado;
- motivo;
- descrição;
- anexo.

Bloco inferior:
- histórico da contestação.

---

## 12.11. Fechamento Mensal do Profissional

### Objetivo visual
Permitir consulta dos períodos consolidados.

### Estrutura
Lista de competências com cards resumidos.

Cada card deve mostrar:
- mês/ano;
- status do período;
- comissão total;
- link “ver detalhes”.

---

## 12.12. Meu Perfil

### Objetivo visual
Permitir consulta dos dados do próprio usuário.

### Estrutura
- avatar/foto;
- nome;
- função/especialidade;
- registro profissional;
- e-mail;
- telefone;
- ações de alteração de senha e sair da conta.

---

## 13. Ícones e Estilo de Ilustração

## 13.1. Ícones

Padrão visual esperado:
- ícones lineares;
- simples;
- discretos;
- consistentes entre si.

Áreas típicas:
- dashboard;
- produção;
- comissão;
- contestação;
- fechamento;
- perfil;
- configurações.

## 13.2. Imagens e apoio visual

Apenas áreas institucionais ou de acolhimento devem usar imagens mais emocionais, como:
- landing page;
- login;
- primeiro acesso.

Áreas operacionais devem focar dados e clareza.

---

## 14. Comportamento Responsivo Esperado

## 14.1. Administrativo

Em telas médias e menores:
- a sidebar poderá colapsar;
- filtros poderão quebrar linha;
- tabelas poderão ter rolagem horizontal controlada;
- cards podem empilhar verticalmente.

## 14.2. Profissional

A experiência deve nascer com prioridade em mobile.

Em desktop:
- manter leitura agradável;
- não descaracterizar o fluxo pensado para celular.

---

## 15. Regras Visuais de Consistência

1. Toda tela precisa ter título claro.
2. Toda tela precisa deixar evidente a ação principal.
3. Todos os formulários devem manter o mesmo padrão de inputs.
4. Todos os estados devem seguir a mesma lógica de cor.
5. A linguagem visual deve ser uniforme entre administrativo e profissional.
6. O sistema deve parecer parte de uma mesma família, mesmo com jornadas diferentes.
7. Não misturar estilos de borda, ícone ou tipografia.
8. O layout deve evitar excesso de elementos por tela.

---

## 16. Checklist para Replicação pelo Codex

Ao implementar, o Codex deve garantir:

- uso da paleta oficial;
- uso da tipografia Inter;
- sidebar administrativa clara e fixa;
- topbar limpa com busca e perfil;
- cards de KPI com destaque financeiro;
- tabelas simples com badges de status;
- formulários limpos e bem espaçados;
- dashboard administrativo no padrão aprovado;
- portal do profissional com foco mobile-first;
- barra inferior na área do profissional;
- uso consistente de bordas arredondadas;
- uso moderado de sombras;
- mesma linguagem visual nas telas aprovadas.

---

## 17. Entrega Esperada da Camada Visual

A camada visual do MVP será considerada aderente quando:

1. As telas implementadas forem reconhecíveis em relação aos mockups aprovados.
2. A navegação administrativa seguir o padrão lateral esquerdo + área principal.
3. A área do profissional mantiver experiência simples, clara e amigável.
4. As cores, tipografia, estilos de botão, cards e tabelas forem consistentes.
5. O resultado final transmitir organização, transparência e proximidade com o universo veterinário.

---

## 18. Resumo Executivo

O padrão visual aprovado para o MVP do sistema VetCom deve seguir um design limpo, leve e profissional, com predominância de tons verdes, tipografia Inter, estrutura administrativa com sidebar lateral e portal do profissional orientado para mobile.

Os mockups já aprovados devem ser tratados como base visual oficial da V1, e este documento deve servir como guia textual para garantir fidelidade visual durante a implementação.
