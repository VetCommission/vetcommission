# Vetcom — pacote de identidade V3

## Conteúdo

- `brand/vetcom-logo.svg`: logo horizontal, verde #0F7056 e grafite #1F2937, para fundos claros.
- `brand/vetcom-logo-dark.svg`: versão para fundos escuros, verde claro #34D399 e branco.
- `brand/vetcom-mark.svg`: símbolo isolado em verde, fundo transparente.
- `brand/favicon.svg`: símbolo com enquadramento quadrado para navegador.
- `brand/favicon-512.png`: exportação 512 × 512 com transparência.
- `brand/favicon-32.png`: exportação 32 × 32.
- `brand/favicon-16.png`: exportação 16 × 16.
- `brand/vetcom-logo-approved.png`: arte raster original aprovada, preservada como referência.
- `images/login-veterinary.webp`: imagem fotorrealista gerada por IA, 1200 × 900.

## Vetorização

Os SVGs contêm contornos vetoriais reais, sem bitmaps incorporados, fontes externas ou texto dependente de fonte. O lettering aprovado foi vetorizado junto ao símbolo. As cores foram uniformizadas e pequenos ruídos do PNG removidos. A vetorização é uma aproximação dos contornos da arte raster, não um redesenho geométrico exato.

## Uso

Copie `assets` para `frontend/public/`. URLs públicas começam com `/assets/`. Prefira o logo SVG na interface e o PNG original apenas como referência. Mantenha a proporção e espaço de respiro. Use `alt="Vetcom"` no logo e `alt=""` na foto decorativa. A interface pode continuar usando Inter.

O SVG dark é transparente: o fundo escuro é fornecido pelo componente/página. Para logo horizontal, prefira largura de pelo menos 160 px; para símbolo detalhado, 48 px ou mais. Em 16–32 px os detalhes internos dos animais se reduzem, e a leitura principal passa a ser a silhueta do V. Os favicons preservam a marca, sem adicionar um novo desenho simplificado.

Formulário e textos de login permanecem em HTML/MUI, nunca na foto. Em mobile, pode ocultar a foto. Não confundir a mudança de nome visual com renomeação de namespaces, repositório ou banco de dados.

Este pacote completa as variantes de marca pendentes. Imagens de apoio do portal e estados vazios continuam fora deste corte, conforme o plano de criação por demanda.
