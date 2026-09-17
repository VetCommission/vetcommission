# DESIGN SYSTEM MVP

## 1. Direção visual

A identidade deve transmitir organização, confiança, transparência e proximidade com o universo veterinário.

A aparência deve ser moderna e acolhedora, evitando:

- excesso de elementos;
- cores muito saturadas;
- estética hospitalar fria;
- visual financeiro agressivo;
- excesso de sombras ou ornamentos;
- componentes diferentes para a mesma finalidade.

## 2. Paleta oficial

### 2.1. Primária
- Verde principal: `#0F7056`
- Uso: ação principal, identidade, item ativo, destaques financeiros positivos, links relevantes.

### 2.2. Secundária
- Verde complementar: `#10B981`
- Uso: sucesso, gráficos, badges positivos, realce secundário.

### 2.3. Destaque
- Amarelo/âmbar: `#F59E0B`
- Uso: atenção, pendência, aviso, estados intermediários.

### 2.4. Neutros
- Texto principal: `#1F2937`
- Texto secundário: `#667280`
- Cinza de borda: `#BFC7D0`
- Fundo principal: `#F5F7F8`
- Fundo auxiliar: `#F8FAFC`
- Branco: `#FFFFFF`

### 2.5. Fundos contextuais
- Verde suave: `#F0FDF4`
- Azul suave: `#EFF6FF`
- Amarelo suave: `#FFF7E7`
- Vermelho suave: `#FEECEC`
- Cinza suave: `#F1F5F9`

### 2.6. Estados
- Sucesso: `#10B981`
- Informação: `#2563EB`
- Atenção: `#F59E0B`
- Erro/rejeição: `#DC2626`
- Inativo/neutro: `#94A3B8`

## 3. Tipografia

Família visual oficial: **Inter**.

### Escala
- H1: 28–32 px / 700
- H2: 22–24 px / 700
- H3: 18–20 px / 600
- Título de card: 16 px / 600
- Corpo: 14–16 px / 400
- Label: 13–14 px / 500
- Auxiliar: 12–13 px / 400
- KPI: 24–28 px / 700
- Valor financeiro em destaque: 24–30 px / 700

### Regras
- Máximo de quatro pesos: 400, 500, 600, 700.
- Não usar fonte decorativa.
- Títulos não devem ocupar mais espaço que o conteúdo.
- Valores financeiros precisam ser visualmente reconhecíveis sem depender apenas de cor.

## 4. Escala de espaçamento

Usar múltiplos previsíveis:
- 4 px: microespaço;
- 8 px: proximidade;
- 12 px: agrupamento compacto;
- 16 px: padrão interno;
- 24 px: separação de blocos;
- 32 px: separação de seções;
- 40/48 px: grande respiro.

## 5. Bordas e raios

- Campo: 8–10 px.
- Botão: 8–10 px.
- Card: 12–16 px.
- Modal: 16 px.
- Badge: formato cápsula ou raio alto.
- Bordas discretas; evitar contornos escuros.

## 6. Sombras

- Cards: sombra mínima ou apenas borda.
- Menus suspensos: sombra suave.
- Modal: sombra média.
- Não usar sombras profundas em elementos comuns.

## 7. Botões

### Primário
- Fundo verde principal.
- Texto branco.
- Ícone opcional à esquerda.
- Altura confortável.
- Estado hover visualmente perceptível.
- Estado desabilitado com menor contraste e sem parecer clicável.

### Secundário
- Fundo branco/claro.
- Borda neutra.
- Texto escuro ou verde.
- Não competir com ação primária.

### Terciário/textual
- Sem preenchimento.
- Usar para ações de menor prioridade.

### Destrutivo
- Vermelho.
- Usar somente quando realmente destrutivo.
- Exigir confirmação quando houver perda de informação.

## 8. Campos de formulário

Todo campo deve possuir:
- label persistente acima;
- área clicável confortável;
- placeholder apenas como exemplo, nunca como substituto do label;
- ajuda contextual quando necessário;
- erro abaixo do campo;
- estado foco claro;
- estado desabilitado reconhecível.

## 9. Cards

### KPI
- label no topo;
- valor em destaque;
- informação complementar abaixo;
- ícone discreto opcional.

### Resumo
- título;
- conteúdo;
- ação opcional alinhada ao topo direito.

### Listagem móvel
- título principal;
- metadados;
- valor;
- status;
- ação de detalhamento.

## 10. Tabelas

- Cabeçalho com contraste leve.
- Linhas com altura confortável.
- Hover suave em desktop.
- Ações na última coluna.
- Status representado por badge.
- Valores numéricos alinhados de forma consistente.
- Estado vazio dentro da própria região da tabela.
- Não exibir colunas sem utilidade prática.

## 11. Badges

Usar linguagem curta:
- Ativo
- Inativo
- Calculada
- Conferida
- Em análise
- Aprovada
- Rejeitada
- Em aberto
- Fechado

Nunca usar somente cor para comunicar estado.

## 12. Abas

- Indicador ativo claro.
- Poucas abas por tela.
- Preferir estados ou agrupamentos naturais.
- Conteúdo deve atualizar mantendo título e contexto.

## 13. Modal

Usar para:
- confirmação;
- decisão curta;
- visualização rápida;
- ação que não justifique mudança de página.

Evitar formulário longo dentro de modal.

## 14. Toast / alerta transitório

Usar para:
- confirmação de salvamento;
- confirmação de envio;
- falha de ação;
- mensagem breve após ação.

Não usar toast como única forma de apresentar erro crítico.

## 15. Ícones

- Linha simples.
- Consistência de espessura.
- Sempre que o significado não for óbvio, acompanhar de texto.
- Não criar ícones decorativos em excesso.

## 16. Gráficos

- Poucas séries.
- Legendas claras.
- Valores relevantes disponíveis também em texto.
- Cores coerentes com a paleta.
- Evitar 3D, gradientes agressivos e excesso de categorias.
