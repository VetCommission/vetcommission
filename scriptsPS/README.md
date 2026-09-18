# Scripts PowerShell do VetCommission

Execute os scripts a partir da raiz do repositório. Pré-requisitos:

- .NET SDK 10;
- Node.js `24.17.0`;
- `nvm`, `fnm` ou `volta`, recomendado para ativar automaticamente a versao Node definida em `.node-version`;
- Docker Desktop;
- arquivo `.env` local baseado em `.env.example`.

## Iniciar os componentes

Para parar processos que estejam usando as portas da API e do frontend e iniciar novamente os dois servicos:

```powershell
.\scriptsPS\01-Restart-Backend-Frontend.ps1
```

O script encerra o processo que estiver escutando a porta `5077` (API) e a porta `3000` (frontend), garante o PostgreSQL local via Docker Compose e inicia os dois servicos em segundo plano.

Se o PostgreSQL ja estiver sendo executado por outro meio:

```powershell
.\scriptsPS\01-Restart-Backend-Frontend.ps1 -SkipDatabase
```

Em terminais separados:

```powershell
.\scriptsPS\Start-Api.ps1
.\scriptsPS\Start-Worker.ps1
.\scriptsPS\Start-Frontend.ps1
```

Para iniciar PostgreSQL, API, Worker e frontend de uma vez:

```powershell
.\scriptsPS\Start-All.ps1
```

O `Start-All.ps1` aguarda o health check do PostgreSQL antes de iniciar os demais componentes. A API usa `http://localhost:5077` e o frontend usa `http://localhost:3000`.

## Validar o frontend

```powershell
.\scriptsPS\Build-Frontend.ps1
```

O script executa lint e build usando a versão Node definida em `.node-version`. Quando `node_modules` nao existir ou estiver incompleto, executa `npm ci`; quando ja estiver consistente, reaproveita as dependencias locais. Para reinstalar dependencias, use:

```powershell
.\scriptsPS\Build-Frontend.ps1 -RefreshDependencies
```

Se a versao ativa do Node for diferente e `nvm`, `fnm` ou `volta` estiver instalado, o script tenta ativar `24.17.0` automaticamente. Com `nvm` ou `volta`, a versao tambem pode ser instalada automaticamente quando ainda nao existir localmente. Apos `nvm use`, os scripts atualizam o `PATH` do processo atual para reconhecer o novo `node`.

## Validar uma implementacao antes do commit/PR

```powershell
.\scriptsPS\Invoke-PostImplementationChecks.ps1
```

O script executa o processo padrao de pos-implementacao:

- `dotnet restore`, `dotnet build` e `dotnet test`;
- `npm ci` somente quando `node_modules` nao existir, estiver incompleto ou quando `-RefreshFrontendDependencies` for informado;
- `npm run lint` e `npm run build`;
- PostgreSQL via Docker Compose;
- health check da API em `http://localhost:5077/health`;
- start tecnico do Worker;
- start tecnico do frontend em `http://localhost:3000`;
- `git diff --check` e `git status --short`.

Use parametros para rodar partes do processo quando necessario:

```powershell
.\scriptsPS\Invoke-PostImplementationChecks.ps1 -SkipRuntime
.\scriptsPS\Invoke-PostImplementationChecks.ps1 -SkipFrontend
.\scriptsPS\Invoke-PostImplementationChecks.ps1 -SkipDotNet
.\scriptsPS\Invoke-PostImplementationChecks.ps1 -KeepRuntimeProcesses
.\scriptsPS\Invoke-PostImplementationChecks.ps1 -RefreshFrontendDependencies
```

Por padrao, processos iniciados pelo script sao encerrados ao final. Use `-KeepRuntimeProcesses` apenas quando quiser continuar testando manualmente a API, o Worker ou o frontend. Se uma porta necessaria estiver em uso, o script encerra o processo bloqueante; use `-KeepBlockingProcesses` para impedir esse comportamento e falhar com orientacao manual.

### Exemplos de uso

Validacao completa padrao:

```powershell
.\scriptsPS\Invoke-PostImplementationChecks.ps1
```

Validacao rapida sem subir PostgreSQL, API, Worker e frontend:

```powershell
.\scriptsPS\Invoke-PostImplementationChecks.ps1 -SkipRuntime
```

Validacao apenas de backend, ignorando frontend:

```powershell
.\scriptsPS\Invoke-PostImplementationChecks.ps1 -SkipFrontend
```

Validacao apenas de frontend, ignorando .NET:

```powershell
.\scriptsPS\Invoke-PostImplementationChecks.ps1 -SkipDotNet -SkipDocker -SkipRuntime
```

Validacao limpa reinstalando dependencias do frontend:

```powershell
.\scriptsPS\Invoke-PostImplementationChecks.ps1 -RefreshFrontendDependencies
```

Validacao mantendo API, Worker e frontend ligados ao final:

```powershell
.\scriptsPS\Invoke-PostImplementationChecks.ps1 -KeepRuntimeProcesses
```

Validacao sem encerrar processos que estejam usando portas necessarias:

```powershell
.\scriptsPS\Invoke-PostImplementationChecks.ps1 -KeepBlockingProcesses
```

Validacao ignorando apenas Docker/PostgreSQL, mas ainda tentando subir os runtimes:

```powershell
.\scriptsPS\Invoke-PostImplementationChecks.ps1 -SkipDocker
```

Build do frontend reaproveitando `node_modules` quando estiver consistente:

```powershell
.\scriptsPS\Build-Frontend.ps1
```

Build do frontend reinstalando dependencias:

```powershell
.\scriptsPS\Build-Frontend.ps1 -RefreshDependencies
```

Build do frontend sem encerrar processos na porta `3000`:

```powershell
.\scriptsPS\Build-Frontend.ps1 -KeepBlockingProcesses
```

## Diagnóstico rápido

```powershell
docker compose ps
docker logs vetcommission-postgres
dotnet --version
node --version
```

Os scripts falham cedo quando um executável, projeto ou versão obrigatória não está disponível. Nenhum segredo é armazenado neles.

Se a validacao parar no Docker, abra o Docker Desktop, aguarde o engine ficar ativo e execute novamente. Para rodar apenas build/test sem subir runtime local:

```powershell
.\scriptsPS\Invoke-PostImplementationChecks.ps1 -SkipRuntime
```

O `npm ci` limpa `node_modules`; por isso ele nao precisa rodar sempre. Use reinstalacao apenas quando `node_modules` nao existir, estiver incompleto, quando dependencias mudarem ou antes de uma validacao bem limpa de PR. Se houver frontend/`next dev` aberto na porta `3000`, os scripts encerram esse processo antes de reinstalar dependencias, salvo quando `-KeepBlockingProcesses` for usado.

## Banco de dados

Execute os scripts a partir da raiz do repositorio, com o Docker Desktop aberto e o container `vetcommission-postgres` em execucao.

## Sincronizar banco e scaffold EF Core

Para aplicar scripts SQL pendentes e, em seguida, regenerar o scaffold EF Core:

```powershell
.\scriptsPS\Sync-Database-And-Scaffold.ps1
```

O comando reaproveita a confirmacao manual do executor de banco. Quando solicitado, digite exatamente:

```text
ATUALIZAR vetcommission
```

Tambem e possivel informar arquivo de ambiente ou container:

```powershell
.\scriptsPS\Sync-Database-And-Scaffold.ps1 -EnvironmentFile ".\.env" -ContainerName "vetcommission-postgres"
```

Para rodar as etapas separadamente:

```powershell
.\scriptsPS\ExecutarScriptsDB.ps1
.\scriptsPS\Scaffold-EF.ps1
```

O scaffold le `.env`, monta a connection string local sem exibir senha, gera somente os schemas `core` e `business`, e usa `--no-onconfiguring` com `--force`. Depois de rodar, revise todo o diff gerado em `src/VetCommission.Infrastructure/Persistence/Generated`.

O scaffold usa ferramenta local versionada em `.config/dotnet-tools.json`. Se houver um `dotnet-ef` global antigo na maquina, ele sera ignorado pelo script. O script executa `dotnet tool restore` antes do scaffold.

## Gerar backup

```powershell
.\scriptsPS\Backup-VetCommission.ps1
```

O script cria na pasta `backup`:

- `vetcommission_yyyyMMdd_HHmmss.backup`
- `vetcommission_yyyyMMdd_HHmmss.backup.sha256`

O arquivo `.backup` contem a estrutura e os dados. O `.sha256` permite detectar alteracoes ou corrupcao.

## Restaurar o backup mais recente

```powershell
.\scriptsPS\Restore-VetCommission.ps1
```

O restore seleciona automaticamente o `.backup` mais recente e exige a confirmacao:

```text
RESTAURAR vetcommission
```

O banco atual e removido e recriado. Conexoes abertas no DBeaver serao encerradas.

## Restaurar um arquivo especifico

```powershell
.\scriptsPS\Restore-VetCommission.ps1 -BackupFile ".\backup\vetcommission_20260917_120000.backup"
```

## Automacao sem confirmacao interativa

Use `-Force` apenas em automacoes controladas:

```powershell
.\scriptsPS\Restore-VetCommission.ps1 -Force
```

## Preparacao do repositorio

Mantenha os backups fora do Git adicionando ao `.gitignore`:

```gitignore
backup/
```

