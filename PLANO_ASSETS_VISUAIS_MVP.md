# Plano de Assets Visuais do MVP

Este documento define os assets visuais necessários para o MVP do VetCommission, considerando os requisitos de UI/UX aprovados.

Ícones, estados de interface e gráficos devem continuar usando componentes MUI, CSS e recursos nativos sempre que possível. Imagens rasterizadas devem ser adicionadas somente quando contribuírem para a experiência visual.

## Momento recomendado

Os assets principais devem ser preparados após a conclusão da fundação de identidade e acesso e antes da implementação das telas de negócio das Issues 03 em diante.

Essa preparação não bloqueia o backend, mas evita que as telas administrativas e o portal profissional sejam construídos com identidade visual provisória.

## Assets prioritários do MVP operacional

### 1. Marca principal

Uso: cabeçalho, tela de login, área administrativa e portal profissional.

Arquivos:

- `vetcommission-logo.svg` — versão horizontal principal;
- `vetcommission-logo-dark.svg` — versão para fundos escuros;
- `vetcommission-mark.svg` — símbolo isolado;
- `favicon.svg` e `favicon-512.png` — ícone do navegador.

Requisitos:

- fundo transparente;
- versão horizontal e versão compacta;
- símbolo simples, profissional e acolhedor;
- predominância de verde;
- compatibilidade com uso em software B2B;
- evitar excesso de detalhes, degradês fortes e aparência infantil.

Prompt sugerido:

> Criar uma identidade visual para um sistema SaaS chamado VetCommission, voltado para gestão de comissões em clínicas veterinárias. Criar um símbolo simples, acolhedor e profissional que combine sutilmente elementos veterinários, como pata ou cruz veterinária, com organização, transparência e gestão. Usar verde como cor principal, com aparência moderna, limpa, minimalista e adequada para software B2B. Fundo transparente. Não usar excesso de detalhes, degradês fortes ou aparência infantil. Criar símbolo isolado e versão horizontal com o texto “VetCommission”.

Observação: para garantir a grafia correta, o ideal é gerar o símbolo e aplicar o texto posteriormente em SVG.

### 2. Ilustração da tela de login

Arquivo: `login-veterinary-illustration.webp`

Formato e dimensões:

- WebP;
- aproximadamente 1200 × 900 px;
- fundo transparente ou muito claro;
- composição horizontal;
- personagem posicionada preferencialmente à direita;
- espaço visual livre para o formulário.

Prompt sugerido:

> Criar uma ilustração editorial moderna e acolhedora para a tela de login de um sistema de gestão para clínicas veterinárias. Mostrar uma profissional veterinária em ambiente de clínica, com um cão ou gato, transmitindo confiança, organização e cuidado. Estilo clean SaaS, formas suaves, paleta predominante verde, branco e tons neutros. Composição horizontal, personagem posicionada mais à direita, espaço visual vazio à esquerda para integração com formulário. Sem textos, sem logotipos, sem números, sem elementos financeiros explícitos.

### 3. Ilustração de apoio para o portal profissional

Arquivo: `professional-portal-support.webp`

Formato e dimensões:

- WebP;
- aproximadamente 1000 × 700 px;
- imagem decorativa leve;
- adequada para desktop e redução em mobile;
- sem texto incorporado.

Prompt sugerido:

> Criar uma ilustração leve e profissional para um portal de acompanhamento de comissões de profissionais veterinários. Mostrar uma veterinária ou um veterinário consultando informações em um notebook ou tablet, em ambiente clínico acolhedor, com presença discreta de um animal de estimação. Estilo moderno, minimalista, amigável e responsivo, usando verde, azul suave, branco e cinza claro. Sem textos, sem gráficos legíveis, sem logotipos e sem números.

### 4. Ilustração de estado vazio genérico

Arquivo: `empty-state-veterinary.webp`

Formato e dimensões:

- WebP ou PNG;
- aproximadamente 640 × 480 px;
- fundo transparente;
- adequada para ausência de produções, contestações ou fechamentos.

Prompt sugerido:

> Criar uma ilustração minimalista para estado vazio de um sistema de gestão veterinária. Mostrar uma prancheta ou painel vazio acompanhado de um pequeno elemento veterinário, como uma pata ou estetoscópio. Transmitir “ainda não há registros”, sem transmitir erro ou problema. Estilo SaaS moderno, linhas suaves, poucos detalhes, paleta verde, cinza claro e branco. Fundo transparente. Sem texto.

### 5. Ilustração de acesso restrito ou tenant ausente

Arquivo: `access-restricted.webp`

Formato e dimensões:

- WebP ou PNG;
- aproximadamente 600 × 450 px;
- fundo transparente;
- uso em estados `403`, acesso bloqueado ou ausência de tenant ativo.

Prompt sugerido:

> Criar uma ilustração amigável para um sistema veterinário representando acesso restrito ou configuração incompleta. Mostrar uma porta, escudo ou painel protegido acompanhado de um elemento veterinário discreto. A imagem deve transmitir orientação e segurança, não punição. Estilo minimalista, profissional, acolhedor, com verde, âmbar suave e cinza. Fundo transparente. Sem texto, sem cadeados grandes ou aparência ameaçadora.

## Elementos que permanecem no MUI

Não é necessário gerar imagens para:

- ícones de navegação;
- usuários, clínicas, procedimentos e comissões;
- sucesso, alerta e erro;
- gráficos e indicadores;
- contestação e fechamento;
- avatares de profissionais;
- ícones de tabelas e ações.

Avatares podem usar as iniciais do usuário com o componente `Avatar` do MUI, evitando armazenamento de fotos no primeiro MVP.

## Assets futuros

Os itens abaixo ficam fora do primeiro corte operacional e devem ser tratados quando a landing page comercial for priorizada:

- imagem principal da landing page;
- imagem institucional com equipe veterinária;
- banners comerciais;
- ilustrações de benefícios;
- imagens para planos e pagamentos;
- materiais de marketing.

## Estrutura no projeto

```text
frontend/
└── public/
    └── assets/
        ├── brand/
        │   ├── vetcommission-logo.svg
        │   ├── vetcommission-logo-dark.svg
        │   ├── vetcommission-mark.svg
        │   └── favicon.svg
        ├── illustrations/
        │   ├── login-veterinary-illustration.webp
        │   ├── professional-portal-support.webp
        │   ├── empty-state-veterinary.webp
        │   └── access-restricted.webp
        └── README.md
```

## Ordem de criação

Os três primeiros assets recomendados são:

1. `vetcommission-mark.svg`;
2. `vetcommission-logo.svg`;
3. `login-veterinary-illustration.webp`.

Os demais devem ser criados conforme as telas reais forem implementadas, evitando gerar imagens que ainda não tenham uso definido.

