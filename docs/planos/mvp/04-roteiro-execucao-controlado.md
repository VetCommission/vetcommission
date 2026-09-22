# Etapa 04 — Roteiro controlado de execução

## Objetivo

Implementar os cadastros de categorias e procedimentos seguindo o padrão visual, funcional e arquitetural consolidado na etapa 03, já vinculados à clínica ativa do tenant.

## Padrão obrigatório reutilizado

Cada domínio deve possuir agrupamento próprio, sem misturar regras de negócio:

- `procedure-categories`;
- `procedures`.

Cada domínio terá seus próprios contratos, handlers, repositórios, controllers, serviços frontend, query keys, telas e testes. Componentes compartilhados serão apenas visuais ou utilitários.

Todas as telas devem seguir o mesmo fluxo:

1. Home autenticada com card de acesso;
2. navbar com retorno para Home;
3. listagem paginada e responsiva;
4. busca e filtros;
5. duplo clique para visualizar detalhes;
6. botão explícito Visualizar;
7. formulário em modo somente leitura;
8. botão Editar habilita os campos;
9. Salvar atualiza o domínio e invalida a query da lista;
10. ativar/inativar com atualização automática;
11. erro permanece no formulário;
12. estado vazio, carregamento e erro tratados.

## Roteiro por etapas

### 04.1 — Fechamento de regras

- confirmar limites de nome, descrição e categoria;
- definir unicidade por tenant;
- definir política de inativação;
- definir formato monetário e precisão do valor padrão;
- confirmar que procedimento inativo não aparece em novos seletores;
- confirmar que itens inativos continuam consultáveis no administrativo;
- registrar dúvidas e decisões antes do SQL.

**Critério de aceite:** regras aprovadas e sem ambiguidade para banco, API e UX.

### 04.2 — Modelo de dados e migração

- criar `business.procedure_category`;
- criar `business.procedure`;
- adicionar tenant, status, auditoria e timestamps;
- criar unicidade e índices;
- criar recursos de acesso específicos;
- aplicar script versionado;
- validar hash e histórico;
- executar scaffold EF.

**Critério de aceite:** banco atualizado, scaffold concluído e entidades conferidas.

### 04.3 — Testes de persistência e isolamento

- testar unicidade dentro do tenant;
- testar acesso entre tenants;
- testar valor monetário inválido;
- testar categoria inexistente ou inativa;
- testar inativação lógica;
- testar preservação do valor padrão;
- testar consulta de itens inativos no administrativo.

**Critério de aceite:** testes de banco/domínio cobrindo regras críticas.

### 04.4 — Backend de categorias

- contratos paginados;
- validators;
- handlers;
- repository;
- controller fino;
- endpoints de lista, detalhe, criação, edição, ativação e inativação;
- policy própria de categoria;
- fallback para ambiente sem banco;
- testes unitários e de integração.

**Critério de aceite:** categorias funcionais isoladamente e protegidas por tenant/policy.

### 04.5 — Backend de procedimentos

- contratos paginados e filtros;
- vínculo obrigatório com categoria ativa na criação/edição;
- valor padrão monetário;
- validators;
- handlers;
- repository;
- controller fino;
- endpoints completos;
- policy própria de procedimentos;
- testes de autorização, valor e inativação.

**Critério de aceite:** procedimentos funcionais sem depender diretamente de implementação interna de categorias.

### 04.6 — Frontend de categorias

- card na Home;
- navbar;
- lista paginada;
- busca;
- detalhe somente leitura;
- edição;
- criação;
- ativação/inativação;
- query key por tenant/domínio;
- atualização automática após mutação;
- estados loading, vazio e erro.

**Critério de aceite:** fluxo administrativo completo conforme padrão da etapa 03.

### 04.7 — Frontend de procedimentos

- card na Home após categorias;
- lista com categoria, valor padrão e status;
- filtro por categoria/status;
- campo monetário com máscara e valor normalizado;
- detalhe e edição;
- seleção apenas de categorias ativas em novos cadastros;
- atualização da lista após salvar;
- duplo clique para detalhes;
- responsividade.

**Critério de aceite:** procedimento pode ser criado, consultado, editado e inativado sem inconsistência visual ou monetária.

### 04.8 — Integração e proteção histórica

- confirmar que alteração do valor padrão afeta somente novos lançamentos;
- não alterar produções ou snapshots existentes;
- validar contrato para etapas futuras de produção e comissão;
- revisar nomenclatura VetCom na UX;
- revisar acessibilidade básica e navegação.

**Critério de aceite:** D13 preservada e nenhuma alteração retroativa silenciosa.

### 04.9 — Validação final e entrega

- executar script pós-implementação;
- revisar build, lint e testes;
- validar manualmente os fluxos completos;
- revisar `git diff` e arquivos temporários;
- atualizar README/documentação;
- criar commit;
- abrir PR;
- atualizar Linear;
- marcar issue como Done após merge.

## Ordem dos cards na Home

1. Categorias;
2. Procedimentos;
3. Profissionais;
4. Demais módulos futuros.

Procedimentos dependem de categorias, e as etapas posteriores de produção dependerão de procedimentos.

## Bloqueios para avançar

Não iniciar a etapa seguinte se houver:

- regra de valor monetário indefinida;
- ausência de isolamento por tenant;
- procedimento selecionando categoria inativa;
- alteração retroativa de valor histórico;
- tela sem detalhe, edição ou estado de erro;
- lista sem atualização após salvar;
- domínio acessando diretamente o repository de outro domínio.
