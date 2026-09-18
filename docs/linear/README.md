# Importação do roadmap no Linear


## Estratégia

Importar uma issue por etapa do roadmap. Cada issue representa uma branch e um Pull Request. O detalhamento técnico permanece nos arquivos `docs/planos/mvp/*.md` e é resumido na descrição da issue.

## Preparação no Linear

1. Criar ou escolher o time responsável pelo VetCommission.
2. Confirmar os status do workflow. O padrão do Linear é semelhante a `Backlog`, `Todo`, `In Progress`, `Done` e `Canceled`, mas o workspace pode personalizá-los.
3. Criar labels usadas pelo roadmap: `mvp`, `architecture`, `backend`, `frontend`, `database`, `ux`, `security`, `testing`.
4. Habilitar estimativas se desejar importar `Estimate`.
5. Como administrador, acessar **Settings → Administration → Import/Export**.
6. Exportar o template CSV do próprio Linear e manter seus cabeçalhos.
7. Copiar as linhas do arquivo a ser gerado a partir de `issues-roadmap.md` para esse template.
8. Executar primeiro um piloto com as etapas 00 e 01; conferir descrições, labels, status e estimativas antes do lote completo.

## Arquivos

- [`issues-roadmap.md`](issues-roadmap.md): conteúdo fonte e revisável das issues.
- `Modelo_Issues_Import_Linear.csv`: export real fornecido pelo workspace; deve permanecer inalterado como referência.
- `issues-roadmap-import.csv`: saída planejada com as 13 novas issues, mantendo exatamente as 34 colunas do modelo.
- `VetCommission_Ajustes_Documentais_Pos_Decisoes_Import_Linear.csv`: import incremental da issue documental 00.1, sem duplicar o roadmap já importado.

## Mapeamento confirmado no modelo

- No export-modelo, `Team` aparece como `VetCommission`; no arquivo de roadmap que importou com sucesso, a coluna ficou vazia e o time foi escolhido no assistente de importação. Novos arquivos incrementais seguem o formato que funcionou.
- `Project`: `VetCommission`
- Status existentes encontrados: `Backlog` e `Done`
- Prioridades existentes encontradas: `High`, `Medium` e `No priority`
- O modelo possui campos de relacionamento: `Parent issue`, `Related to` e `Blocked by`
- O modelo não possui estimativas preenchidas nas issues exportadas

As novas issues devem começar em `Backlog`. A coluna `ID` deve permanecer vazia para que o Linear gere os identificadores que serão usados nos nomes das branches. Dependências sequenciais podem ser descritas no texto durante a importação e vinculadas em `Blocked by` depois que os novos IDs existirem.

## Limitações do CSV

O importador CSV cria issues, mas não é o melhor mecanismo para criar projetos, relações complexas ou comentários. Dependências entre etapas permanecem descritas nas issues e podem ser conectadas no Linear após a importação.

## Fonte oficial

- https://linear.app/docs/cli-importer
- https://linear.app/docs/import-issues
- https://linear.app/docs/configuring-workflows
