# Arquitetura Global do Sistema

> Versao 3: arquitetura de aplicacao independente do fornecedor de banco de dados.

Este documento define o padrao global para softwares com API .NET, Worker .NET e frontend Next.js. Ele governa camadas, dependencias, seguranca, multitenancy, processamento, frontend, testes e processo de entrega sem acoplar a aplicacao a um mecanismo de banco especifico.

O objetivo e permitir criar um novo produto do zero sem reinventar estrutura, camadas, fluxo de dados, padroes de erro, autenticacao, autorizacao, services, forms e organizacao de features.

> Use este arquivo como contrato arquitetural principal. Toda tarefa de persistencia tambem deve ler o perfil de banco ativo indicado abaixo.

## Perfil de Banco Ativo

```text
ARQUITETURA_POSTGRES_DB.MD
```

Somente um perfil de banco pode estar ativo por projeto. Para trocar o mecanismo de persistencia, substitua o perfil ativo, por exemplo por `ARQUITETURA_SQLSERVER_DB.MD`, sem reescrever as regras de aplicacao deste documento.

## Stack Padrao

### API

- .NET 10
- ASP.NET Core Web API
- MediatR
- FluentValidation
- Entity Framework Core
- provider do EF Core definido pelo perfil de banco ativo
- JWT Bearer Authentication
- Authorization policies por recurso/permissao
- Serilog
- Swagger/Swashbuckle
- xUnit para testes unitarios e de integracao
- testes de integracao contra o mecanismo real definido pelo perfil ativo

### Processamento em Background

- .NET Worker Service como executavel separado da WebApi
- `BackgroundService` para busca e execucao de trabalhos pendentes
- banco relacional configurado como fila persistente na primeira versao
- padroes de idempotencia, retry controlado e Outbox
- compartilhamento de `Domain`, `Application` e `Infrastructure` com a WebApi

### Frontend

- Next.js com App Router
- React
- TypeScript strict
- Material UI
- TanStack React Query
- Axios
- React Hook Form
- Zod
- ESLint com `eslint-config-next/core-web-vitals`
- Node.js controlado por `.node-version`

### Persistencia e Automacao

- Database First: scripts SQL versionados como fonte principal de evolucao do schema
- scaffold do EF Core executado somente depois da alteracao do banco
- provider, tipos, comandos e ambiente local definidos pelo perfil de banco ativo
- PowerShell para execucao local
- tabela de controle de versao dos scripts do banco
- scripts de start para banco, API, Worker e frontend

## Estrutura Global do Repositorio

Use o nome real do produto no lugar de `<NomeProjeto>`.

Todos os scripts PowerShell do repositorio devem ficar exclusivamente em `scriptsPS/`. A pasta `database/scripts/` e reservada aos scripts SQL evolutivos e nao deve conter arquivos `.ps1`.

```text
<NomeProjeto>/
  src/
    <NomeProjeto>.Domain/
    <NomeProjeto>.Application/
    <NomeProjeto>.Infrastructure/
    <NomeProjeto>.WebApi/
    <NomeProjeto>.Worker/
  frontend/
    src/
      app/
      components/
      features/
      lib/
      providers/
      theme/
      types/
      utils/
    public/
  database/
    dbconfig.json
    scripts/
  docker-compose.yml
  .env.example
  scriptsPS/
    ExecutarScriptsDB.ps1
    Scaffold-EF.ps1
    Start-Api.ps1
    Start-Worker.ps1
    Start-Frontend.ps1
    Start-All.ps1
    Build-Frontend.ps1
  tests/
    <NomeProjeto>.UnitTests/
    <NomeProjeto>.IntegrationTests/
  README.md
  ARQUITETURA.MD
  ARQUITETURA_<BANCO>_DB.MD
```

## Direcao das Dependencias

```text
WebApi -> Application -> Domain
WebApi -> Infrastructure -> Application -> Domain
Worker -> Application -> Domain
Worker -> Infrastructure -> Application -> Domain
Infrastructure -> Domain
Tests -> projetos testados
Frontend -> API HTTP
```

Regras:

- `Domain` nao depende de nenhum outro projeto da solucao.
- `Application` depende de `Domain` e bibliotecas de aplicacao.
- `Infrastructure` implementa persistencia, repositorios, autenticacao tecnica e servicos externos.
- `WebApi` compoe a aplicacao e expoe HTTP.
- `Worker` executa processamentos demorados, retentativas e integracoes sem expor endpoints HTTP.
- `Frontend` consome a API por HTTP e mantem contratos TypeScript sincronizados manualmente.
- Nenhuma camada deve pular sua responsabilidade para "ganhar velocidade" no MVP.

---

# API

## Projetos da Solucao

### `<NomeProjeto>.Domain`

Responsavel por:

- entidades de dominio;
- enums;
- interfaces de repositorio;
- abstracoes compartilhadas;
- tipos que representam o nucleo do negocio.

Estrutura:

```text
Domain/
  Abstractions/
  Entities/
  Enums/
  Repositories/
  AssemblyReference.cs
```

Padrao de entidade:

```csharp
namespace <NomeProjeto>.Domain.Entities;

public class <Entidade>
{
    public Guid Id { get; set; }
    public Guid TenantId { get; set; }
    public string Nome { get; set; } = string.Empty;
    public bool Ativo { get; set; } = true;
    public DateTime CriadoEm { get; set; }
    public Guid? CriadoPorUsuarioId { get; set; }
    public DateTime? AtualizadoEm { get; set; }
    public Guid? AtualizadoPorUsuarioId { get; set; }
}
```

Use `TenantId` quando o produto for multiempresa, multiorganizacao ou multiunidade. Se o produto nao for multitenant, remova esse campo do padrao e tambem remova os validadores de contexto.

Regras de entidade:

- usar `Guid` como identificador;
- inicializar strings obrigatorias com `string.Empty`;
- marcar navegacoes opcionais com nullable reference types;
- inicializar colecoes com `new List<T>()`;
- nao colocar regra HTTP em entidade;
- nao colocar dependencia de EF, ASP.NET ou infraestrutura no dominio;
- manter enums no dominio quando eles representam regras de negocio.

### `<NomeProjeto>.Application`

Responsavel por:

- casos de uso;
- comandos e queries MediatR;
- handlers;
- validators FluentValidation;
- DTOs;
- mappers manuais;
- resultado padronizado;
- regras de aplicacao;
- interfaces de servicos externos;
- validacao de usuario e tenant ativo.

Estrutura:

```text
Application/
  Common/
    Auth/
    Behaviors/
    Context/
    Errors/
    Results/
  Features/
    <Modulo>/
      Criar<Entidade>/
      Atualizar<Entidade>/
      Remover<Entidade>/
      Listar<Entidade>/
      Obter<Entidade>/
      <Entidade>Dto.cs
      <Entidade>Mapper.cs
  DependencyInjection.cs
  AssemblyReference.cs
```

Padrao de feature:

```text
Features/
  Clientes/
    CriarCliente/
      CriarClienteCommand.cs
      CriarClienteCommandHandler.cs
      CriarClienteCommandValidator.cs
    AtualizarCliente/
      AtualizarClienteCommand.cs
      AtualizarClienteCommandHandler.cs
      AtualizarClienteCommandValidator.cs
    ListarClientes/
      ListarClientesQuery.cs
      ListarClientesQueryHandler.cs
    ObterCliente/
      ObterClienteQuery.cs
      ObterClienteQueryHandler.cs
    ClienteDto.cs
    ClienteMapper.cs
```

Nomenclatura:

```text
Criar<Entidade>Command
Atualizar<Entidade>Command
Remover<Entidade>Command
Listar<Entidade>Query
Obter<Entidade>Query
<Entidade>Dto
<Entidade>Mapper
<Entidade>Repository
I<Entidade>Repository
```

### `<NomeProjeto>.Infrastructure`

Responsavel por:

- `DbContext`;
- mapeamento EF Core;
- repositorios concretos;
- unidade de trabalho;
- hash de senha;
- geracao de JWT;
- integracoes externas;
- servicos que dependem de banco ou infraestrutura.

Estrutura:

```text
Infrastructure/
  Auth/
  Persistence/
    <NomeProjeto>DbContext.cs
    Generated/
      Entities/
    UnitOfWork.cs
    Mappers/
  Repositories/
  Services/
  DependencyInjection.cs
```

### `<NomeProjeto>.WebApi`

Responsavel por:

- `Program.cs`;
- controllers;
- middlewares;
- options;
- CORS;
- JWT;
- authorization policies;
- Swagger;
- leitura de usuario atual;
- leitura de tenant/contexto ativo;
- conversao de resultado de aplicacao para HTTP.

Estrutura:

```text
WebApi/
  Controllers/
  Extensions/
  Middlewares/
  Options/
  Services/
  Properties/
  Program.cs
  appsettings.json
  appsettings.Development.json
```

### `<NomeProjeto>.Worker`

Responsavel por:

- localizar processamentos pendentes no banco configurado;
- reservar um trabalho de maneira atomica;
- executar calculos em background;
- registrar progresso, conclusao e falha;
- aplicar retry somente para falhas transitorias;
- publicar mensagens pendentes da Outbox;
- enviar notificacoes externas, como Slack;
- manter logs com `TenantId`, `ProcessamentoId` e `CorrelationId`.

Estrutura:

```text
Worker/
  Jobs/
    ProcessarComissoesWorker.cs
    PublicarOutboxWorker.cs
  Options/
  Program.cs
  appsettings.json
  appsettings.Development.json
```

Regras:

- o Worker e outro executavel da mesma solucao, nao um microservico;
- nao duplicar regras de negocio no Worker;
- o Worker chama casos de uso da `Application`;
- a reserva de trabalho deve impedir processamento simultaneo do mesmo item;
- cada operacao precisa ser idempotente;
- falha permanente deve encerrar o trabalho como falho e preservar o diagnostico;
- notificacao ao Slack nunca deve desfazer um calculo concluido;
- `CancellationToken` deve ser propagado em toda operacao assincrona.

Fluxo padrao:

```text
Frontend -> WebApi -> registra Processamento pendente -> retorna 202 Accepted
Worker -> reserva Processamento -> calcula -> grava resultado -> registra Outbox
Worker -> publica Outbox no Slack -> marca mensagem como enviada
Frontend -> consulta status do Processamento
```

## Bootstrap da API

O `Program.cs` deve seguir esta ordem:

1. Configurar Serilog bootstrap.
2. Criar `WebApplicationBuilder`.
3. Configurar Serilog definitivo.
4. Registrar `AddApplication()`.
5. Registrar `AddInfrastructure(builder.Configuration)`.
6. Registrar controllers.
7. Registrar `IHttpContextAccessor`.
8. Registrar usuario atual.
9. Registrar contexto ativo, se houver multitenancy.
10. Registrar health checks.
11. Registrar Swagger.
12. Configurar CORS via options.
13. Configurar Data Protection se necessario.
14. Validar opcoes JWT no start.
15. Configurar JWT Bearer.
16. Configurar authorization policies.
17. Criar app.
18. Aplicar middlewares globais.
19. Habilitar Swagger em desenvolvimento.
20. Aplicar HTTPS, CORS, authentication e authorization.
21. Mapear controllers.
22. Mapear `/health`.
23. Executar `app.Run()`.

## Bootstrap do Worker

O `Program.cs` do Worker deve:

1. criar `HostApplicationBuilder`;
2. configurar Serilog;
3. registrar `AddApplication()`;
4. registrar `AddInfrastructure(builder.Configuration)`;
5. validar as opcoes de processamento e Slack no start;
6. registrar `ProcessarComissoesWorker`;
7. registrar `PublicarOutboxWorker`;
8. configurar Health Checks quando houver endpoint ou monitor externo;
9. construir e executar o host.

O Worker nao registra controllers, CORS, Swagger nem autenticacao HTTP. Seu contexto de tenant vem de cada registro de processamento reservado.

## Configuracao

Arquivos padrao:

```text
src/<NomeProjeto>.WebApi/appsettings.json
src/<NomeProjeto>.WebApi/appsettings.Development.json
src/<NomeProjeto>.WebApi/Properties/launchSettings.json
```

Exemplo:

```json
{
  "AllowedHosts": "*",
  "ConnectionStrings": {
    "DefaultConnection": ""
  },
  "Jwt": {
    "Issuer": "<NomeProjeto>",
    "Audience": "<NomeProjeto>.Api",
    "SecretKey": "",
    "ExpirationMinutes": 60
  },
  "Cors": {
    "AllowedOrigins": []
  }
}
```

Em desenvolvimento:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": ""
  },
  "Jwt": {
    "Issuer": "<NomeProjeto>",
    "Audience": "<NomeProjeto>.Api",
    "SecretKey": "development-secret-key-<nomeprojeto>-0001",
    "ExpirationMinutes": 120
  },
  "Cors": {
    "AllowedOrigins": [
      "http://localhost:3000",
      "https://localhost:3000"
    ]
  }
}
```

Portas recomendadas:

```text
API: http://localhost:5077
Swagger: http://localhost:5077/swagger
Frontend: http://localhost:3000
```

Connection strings e segredos reais nao devem ser versionados. O perfil de banco define as variaveis locais necessarias. Em ambientes compartilhados, usar variaveis de ambiente ou um provedor de segredos.

## Resultado Padronizado

Todas as regras de negocio devem retornar `NotificationResult`.

```csharp
namespace <NomeProjeto>.Application.Common.Results;

public class NotificationResult
{
    protected NotificationResult(bool isSuccess, IReadOnlyCollection<NotificationError> errors)
    {
        IsSuccess = isSuccess;
        Errors = errors;
    }

    public bool IsSuccess { get; }

    public bool IsFailure => !IsSuccess;

    public IReadOnlyCollection<NotificationError> Errors { get; }

    public static NotificationResult Success()
    {
        return new NotificationResult(true, Array.Empty<NotificationError>());
    }

    public static NotificationResult Failure(params NotificationError[] errors)
    {
        return new NotificationResult(false, errors);
    }

    public static NotificationResult Failure(IEnumerable<NotificationError> errors)
    {
        return new NotificationResult(false, errors.ToArray());
    }
}

public sealed class NotificationResult<T> : NotificationResult
{
    private NotificationResult(bool isSuccess, T? value, IReadOnlyCollection<NotificationError> errors)
        : base(isSuccess, errors)
    {
        Value = value;
    }

    public T? Value { get; }

    public static NotificationResult<T> Success(T value)
    {
        return new NotificationResult<T>(true, value, Array.Empty<NotificationError>());
    }

    public new static NotificationResult<T> Failure(params NotificationError[] errors)
    {
        return new NotificationResult<T>(false, default, errors);
    }

    public new static NotificationResult<T> Failure(IEnumerable<NotificationError> errors)
    {
        return new NotificationResult<T>(false, default, errors.ToArray());
    }
}
```

Erro:

```csharp
public sealed record NotificationError(string Code, string Message, string? Field = null);
```

Codigos padrao:

```csharp
public static class ErrorCodes
{
    public const string Validation = "Validation";
    public const string NotFound = "NotFound";
    public const string Conflict = "Conflict";
    public const string Unauthorized = "Unauthorized";
    public const string Forbidden = "Forbidden";
}
```

Conversao HTTP:

```text
Success sem valor -> 204 NoContent
Success com valor -> 200 OK
Created -> usar callback no controller
NotFound -> 404
Conflict -> 409
Unauthorized -> 401
Forbidden -> 403
Validation/demais -> 400
```

Formato HTTP de erro:

```json
{
  "errors": [
    {
      "code": "Validation",
      "message": "Mensagem de erro",
      "field": "CampoOpcional"
    }
  ]
}
```

## Validacao

Use FluentValidation para requests de entrada.

Padrao:

```csharp
public sealed class CriarClienteCommandValidator : AbstractValidator<CriarClienteCommand>
{
    public CriarClienteCommandValidator()
    {
        RuleFor(command => command.Nome).NotEmpty().MaximumLength(200);
        RuleFor(command => command.Email).EmailAddress().MaximumLength(250);
    }
}
```

O pipeline `ValidationBehavior<TRequest, TResponse>` deve:

- localizar validators do request;
- executar validacoes antes do handler;
- converter erros em `NotificationError`;
- retornar `NotificationResult` ou `NotificationResult<T>`;
- evitar exception para erro de negocio esperado.

## Padrao de Handler

Fluxo obrigatorio:

```text
Controller -> IMediator.Send(command/query)
Handler -> valida usuario/contexto
Handler -> valida existencia e regras
Handler -> usa repository
Handler -> usa UnitOfWork.SaveChangesAsync
Handler -> retorna NotificationResult
Controller -> ToActionResult()
```

Exemplo:

```csharp
public sealed class CriarClienteCommandHandler
    : IRequestHandler<CriarClienteCommand, NotificationResult<ClienteDto>>
{
    private readonly ITenantContextValidator _tenantContextValidator;
    private readonly IClienteRepository _clienteRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CriarClienteCommandHandler(
        ITenantContextValidator tenantContextValidator,
        IClienteRepository clienteRepository,
        IUnitOfWork unitOfWork)
    {
        _tenantContextValidator = tenantContextValidator;
        _clienteRepository = clienteRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<NotificationResult<ClienteDto>> Handle(
        CriarClienteCommand request,
        CancellationToken cancellationToken)
    {
        var tenantResult = await _tenantContextValidator.ObterTenantAtivoAsync(cancellationToken);
        if (tenantResult.IsFailure)
        {
            return NotificationResult<ClienteDto>.Failure(tenantResult.Errors);
        }

        var cliente = new Cliente
        {
            Id = Guid.NewGuid(),
            TenantId = tenantResult.Value,
            Nome = request.Nome,
            CriadoEm = DateTime.UtcNow
        };

        await _clienteRepository.AdicionarAsync(cliente, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return NotificationResult<ClienteDto>.Success(ClienteMapper.ToDto(cliente));
    }
}
```

## Controllers

Controllers devem ser finos.

Padrao:

```csharp
[ApiController]
[Authorize]
[Route("api/clientes")]
public sealed class ClientesController : ControllerBase
{
    private readonly IMediator _mediator;

    public ClientesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> Listar([FromQuery] string? search, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new ListarClientesQuery(search), cancellationToken);
        return result.ToActionResult();
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> Obter(Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new ObterClienteQuery(id), cancellationToken);
        return result.ToActionResult();
    }

    [HttpPost]
    public async Task<IActionResult> Criar(CriarClienteCommand command, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(command, cancellationToken);
        return result.ToActionResult(cliente => CreatedAtAction(nameof(Obter), new { id = cliente.Id }, cliente));
    }
}
```

Regras:

- nao colocar regra de negocio no controller;
- nao acessar DbContext no controller;
- nao retornar exception para validacao esperada;
- usar records para requests locais quando o payload HTTP nao for igual ao command;
- usar `[Authorize(Policy = AccessPolicies.<Recurso>)]` em acoes protegidas por permissao especifica.

## Autenticacao

Endpoints padrao:

```text
POST /api/auth/login
GET  /api/auth/me
```

Payload de login:

```json
{
  "email": "admin@local",
  "senha": "Admin@123"
}
```

Resposta de login:

```json
{
  "accessToken": "...",
  "expiraEm": "2026-01-01T00:00:00Z",
  "usuario": {
    "id": "...",
    "nome": "Administrador",
    "email": "admin@local"
  },
  "tenants": [
    {
      "tenantId": "...",
      "nome": "Empresa Principal",
      "grupoAcessoId": "...",
      "grupoAcessoNome": "Administrador",
      "recursos": ["menu.dashboard"]
    }
  ]
}
```

Claims do JWT:

```text
sub
email
ClaimTypes.NameIdentifier
ClaimTypes.Email
ClaimTypes.Name
acesso
```

## Multi-Tenant Obrigatorio no VetCommission

No VetCommission o multi-tenant e obrigatorio. Cada cliente do SaaS corresponde a um `Tenant`, que pode possuir uma ou mais clinicas/unidades. Use um header padrao:

```text
X-Tenant-Id: {tenantId}
```

Componentes:

```text
ITenantContext
ITenantContextValidator
HeaderTenantContext
```

Validacoes:

1. usuario autenticado;
2. header `X-Tenant-Id` presente;
3. usuario possui vinculo ativo com o tenant;
4. entidade consultada pertence ao tenant ativo.
5. `TenantId` do payload nunca substitui o tenant obtido do contexto autenticado;
6. indices unicos de dados isolados incluem `TenantId`;
7. jobs do Worker sempre sao reservados e processados dentro de um tenant;
8. logs tecnicos incluem `TenantId`, sem expor dados sensiveis.

Defesa em profundidade:

- filtros globais do EF Core aplicam o tenant nas consultas comuns;
- repositorios continuam recebendo `tenantId` explicitamente nos casos sensiveis;
- `SaveChangesInterceptor` valida entidades isoladas antes do commit;
- testes de integracao comprovam que um tenant nao le ou altera dados de outro;
- recursos nativos de isolamento oferecidos pelo perfil de banco podem ser usados como camada adicional, sem substituir as validacoes da aplicacao.

## Autorizacao por Recursos

Padrao:

```text
Application/Common/Auth/AccessCatalog.cs
Application/Common/Auth/AccessPolicies.cs
```

Exemplo:

```csharp
public static class AccessCatalog
{
    public const string MenuDashboard = "menu.dashboard";
    public const string MenuClientes = "menu.clientes";
    public const string ClientesGerenciar = "clientes.gerenciar";
}

public static class AccessPolicies
{
    public const string Clientes = AccessCatalog.MenuClientes;

    public static readonly IReadOnlyDictionary<string, string[]> Requisitos =
        new Dictionary<string, string[]>(StringComparer.OrdinalIgnoreCase)
        {
            [Clientes] = [AccessCatalog.MenuClientes],
        };
}
```

Regras:

- recursos sao strings estaveis;
- backend e frontend devem ter o mesmo catalogo;
- menus usam recursos `menu.<modulo>`;
- acoes sensiveis usam recursos `<modulo>.<acao>`;
- policies sao criadas no bootstrap da API.

## Persistencia

`Infrastructure` concentra o provider do EF Core, o `DbContext`, modelos gerados, mapeamentos, repositorios e unidade de trabalho. `Domain` e `Application` nao dependem do mecanismo de banco.

### Estrategia Database First

O banco e a fonte de verdade estrutural da persistencia. A sequencia obrigatoria e:

```text
criar novo script evolutivo
-> revisar constraints, indices e isolamento de tenant
-> aplicar no banco
-> validar a estrutura
-> executar scaffold do EF Core
-> revisar todo o diff gerado
-> ajustar mappers e repositorios
-> executar build e testes
```

Regras globais:

- scripts aplicados sao imutaveis;
- correcoes usam uma nova versao;
- o mecanismo de controle de versoes e definido pelo perfil ativo;
- arquivos gerados ficam em `Infrastructure/Persistence/Generated`;
- arquivos gerados nao recebem regras de negocio nem alteracoes manuais;
- entidades de dominio permanecem em `Domain`;
- extensoes usam arquivos separados ou pontos parciais;
- a connection string nao pode ser gravada no codigo gerado;
- sempre executar scaffold sem gerar configuracao de conexao no `DbContext`;
- revisar o diff para detectar remocoes ou mudancas inesperadas;
- nao usar `EnsureCreated`, `EnsureDeleted` ou EF Migrations para evoluir o schema normal.

### Organizacao Logica dos Schemas

- `core`: objetos reutilizaveis praticamente sem alteracao em outros produtos;
- `business`: objetos que existem por causa do dominio do produto;
- objetos da aplicacao nao devem ser criados no schema padrao do fornecedor;
- tabelas usam nome singular, em ingles e `snake_case`;
- consultas e mapeamentos qualificam o schema explicitamente.

O perfil ativo define a sintaxe fisica, tipos, convencoes de constraints e implementacao desses schemas.
## Repositorios

Interface no dominio:

```csharp
public interface IClienteRepository
{
    Task<IReadOnlyCollection<Cliente>> ListarAsync(
        Guid tenantId,
        string? search,
        CancellationToken cancellationToken);

    Task<Cliente?> ObterPorIdAsync(Guid tenantId, Guid id, CancellationToken cancellationToken);
    Task<bool> ExisteAsync(Guid tenantId, Guid id, CancellationToken cancellationToken);
    Task AdicionarAsync(Cliente cliente, CancellationToken cancellationToken);
}
```

Implementacao na infraestrutura:

```csharp
public sealed class ClienteRepository : IClienteRepository
{
    private readonly <NomeProjeto>DbContext _dbContext;

    public ClienteRepository(<NomeProjeto>DbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyCollection<Cliente>> ListarAsync(
        Guid tenantId,
        string? search,
        CancellationToken cancellationToken)
    {
        var query = _dbContext.Clientes
            .AsNoTracking()
            .Where(cliente => cliente.TenantId == tenantId);

        if (!string.IsNullOrWhiteSpace(search))
        {
            query = query.Where(cliente => cliente.Nome.Contains(search));
        }

        return await query
            .OrderBy(cliente => cliente.Nome)
            .ToListAsync(cancellationToken);
    }
}
```

Regras:

- repositorio nao chama `SaveChangesAsync`;
- commit acontece no handler por `IUnitOfWork`;
- queries multi-tenant recebem `tenantId`;
- nao expor `IQueryable` para Application;
- manter includes apenas quando necessarios ao caso de uso.

## Perfil de Banco de Dados

As regras fisicas de persistencia ficam exclusivamente no perfil ativo. Para o VetCommission, leia `ARQUITETURA_POSTGRES_DB.MD` antes de:

- criar ou alterar scripts SQL;
- configurar o provider do EF Core;
- executar scaffold;
- implementar reserva concorrente de jobs;
- criar testes de integracao com banco;
- analisar indices e planos de execucao;
- executar backup ou restore.

Se o perfil ativo e este documento divergirem, interrompa a implementacao e corrija a documentacao antes de alterar codigo ou banco.
## Middlewares

Padroes globais:

```text
CorrelationIdMiddleware
ExceptionHandlingMiddleware
```

Correlation:

- header `X-Correlation-Id`;
- se nao vier, gerar `Guid.NewGuid().ToString("N")`;
- devolver o header na response;
- adicionar propriedade no log.

Exception:

- capturar excecoes nao tratadas;
- logar erro;
- retornar JSON com `title`, `status` e `traceId`;
- nao vazar stack trace ao cliente.

## Testes da API

Projetos:

```text
tests/<NomeProjeto>.UnitTests
tests/<NomeProjeto>.IntegrationTests
```

Unitarios:

- testar regras puras;
- testar validadores quando a regra for relevante;
- testar resolvers/helpers;
- evitar banco real.

Integracao:

- usar `WebApplicationFactory<Program>`;
- usar o banco real definido pelo perfil ativo em ambiente isolado;
- seed controlado;
- helper para cliente autenticado;
- helper para aplicar tenant ativo;
- testar status HTTP e contratos principais.

Padrao de factory:

```text
<NomeProjeto>WebApplicationFactory
CreateAuthenticatedClientAsync(...)
Seed(...)
```

Nao usar EF Core InMemory para validar comportamento de persistencia, SQL, constraints, concorrencia, transacoes ou isolamento multi-tenant. Esses testes precisam executar contra o mecanismo real do perfil ativo.

---

# Frontend

## Estrutura

```text
frontend/src/
  app/
  components/
    branding/
    feedback/
    layout/
    navigation/
  features/
    auth/
    clientes/
  lib/
    api/
    auth/
    navigation/
    query/
    validation/
  providers/
  theme/
  types/
  utils/
```

Alias:

```json
{
  "@/*": ["./src/*"]
}
```

Cada import interno deve usar `@/...` quando fizer sentido.

## App Router

Padrao de rotas:

```text
app/
  page.tsx
  layout.tsx
  login/
    page.tsx
  dashboard/
    page.tsx
  clientes/
    page.tsx
    [id]/
      page.tsx
```

Regras:

- `app/page.tsx` pode redirecionar para `/login` ou `/dashboard`;
- paginas devem compor guardas e conteudo;
- logica de tela fica em `features/<modulo>/components`;
- layout global fica em `app/layout.tsx`;
- providers globais ficam em `providers/AppProviders.tsx`.

## Providers

Ordem padrao:

```text
QueryClientProvider
ThemeProvider
CssBaseline
ToastProvider
AuthProvider
TenantProvider
children
```

Se o produto nao for multitenant, remova `TenantProvider`.

## AuthProvider

Responsavel por:

- armazenar sessao;
- persistir sessao em localStorage;
- expor `session`, `setSession`, `logout`, `isAuthenticated`, `isReady`;
- carregar sessao somente no cliente.

Storage:

```text
<nomeprojeto>.auth.session
```

## TenantProvider

Responsavel por:

- armazenar tenant ativo;
- persistir tenant em localStorage;
- limpar cache do React Query ao trocar tenant.

Storage:

```text
<nomeprojeto>.tenant.ativo
```

## ToastProvider

Responsavel por snackbar global.

Severidades:

```text
success
info
warning
error
```

Uso:

```ts
const { showToast } = useToast();
showToast('Registro salvo com sucesso.', 'success');
```

## Cliente HTTP

Arquivo:

```text
frontend/src/lib/api/apiClient.ts
```

Base URL:

```text
process.env.NEXT_PUBLIC_API_BASE_URL ?? 'http://localhost:5077'
```

Headers automaticos:

```text
Authorization: Bearer {accessToken}
X-Tenant-Id: {tenantId}
```

Rotas sem tenant:

```text
/api/auth/login
/api/auth/me
/health
```

Em 401:

- limpar sessao;
- limpar tenant ativo;
- redirecionar para `/login`.

Erro padrao:

```ts
export class ApiError extends Error {
  public readonly status?: number;
  public readonly errors: ApiErrorItem[];
}

export function getApiErrorMessage(error: unknown) {
  if (error instanceof ApiError) {
    return error.errors[0]?.message ?? error.message;
  }

  if (error instanceof Error) {
    return error.message;
  }

  return 'Erro inesperado.';
}
```

## Contratos

Arquivos:

```text
frontend/src/types/contracts.ts
frontend/src/types/enums.ts
frontend/src/types/api.ts
frontend/src/types/common.ts
```

Regras:

- DTOs globais da API em `contracts.ts`;
- enums numericos sincronizados com backend em `enums.ts`;
- tipos simples compartilhados em `common.ts`;
- erro padrao em `api.ts`;
- tipos locais da feature em `features/<modulo>/types.ts`.

## Endpoints

Arquivo:

```text
frontend/src/lib/api/endpoints.ts
```

Padrao:

```ts
export const endpoints = {
  auth: {
    login: '/api/auth/login',
    me: '/api/auth/me',
  },
  clientes: '/api/clientes',
  health: '/health',
} as const;
```

## React Query

Arquivo:

```text
frontend/src/lib/query/queryClient.ts
```

Configuracao:

```ts
export function createQueryClient() {
  return new QueryClient({
    defaultOptions: {
      queries: {
        staleTime: 30_000,
        refetchOnWindowFocus: false,
        retry: 1,
      },
      mutations: {
        retry: false,
      },
    },
  });
}
```

Query keys por feature:

```ts
export const clienteKeys = {
  all: (tenantId: string | null | undefined) => ['clientes', tenantId] as const,
  list: (tenantId: string | null | undefined, filters: ClienteFilters) => (
    [...clienteKeys.all(tenantId), 'list', filters] as const
  ),
  detail: (tenantId: string | null | undefined, id: string) => (
    [...clienteKeys.all(tenantId), 'detail', id] as const
  ),
};
```

Regras:

- incluir tenant/contexto nas query keys quando houver isolamento;
- usar `enabled: Boolean(tenantId)` em dados dependentes de tenant;
- invalidar pelo prefixo `featureKeys.all(tenantId)`;
- limpar cache ao trocar tenant.

## Estrutura de Feature Frontend

```text
features/clientes/
  components/
    ClientesPageContent.tsx
    ClienteFormDialog.tsx
    ClienteDetailPageContent.tsx
  schemas/
    clienteSchema.ts
  services/
    clientesService.ts
  clienteQueryKeys.ts
  types.ts
  index.ts
```

Responsabilidades:

```text
page.tsx                 -> guardas e PageContent
PageContent.tsx          -> estado de tela, queries, mutations, tabela e filtros
FormDialog.tsx           -> formulario, reset e conversao para payload
DetailPageContent.tsx    -> tela de detalhe
schemas/*.ts             -> Zod + default values
services/*.ts            -> chamadas HTTP
*QueryKeys.ts            -> chaves React Query
types.ts                 -> filtros, payloads e aliases locais
contracts.ts             -> DTOs globais vindos da API
```

## Services

Padrao:

```ts
export const clientesService = {
  async listar(filters: ClienteFilters = {}) {
    const response = await apiClient.get<ClienteDto[]>(endpoints.clientes, { params: filters });
    return response.data;
  },
  async obter(id: string) {
    const response = await apiClient.get<ClienteDto>(`${endpoints.clientes}/${id}`);
    return response.data;
  },
  async criar(payload: CriarClienteRequest) {
    const response = await apiClient.post<ClienteDto>(endpoints.clientes, payload);
    return response.data;
  },
  async atualizar(id: string, payload: AtualizarClienteRequest) {
    await apiClient.put(`${endpoints.clientes}/${id}`, payload);
  },
  async remover(id: string) {
    await apiClient.delete(`${endpoints.clientes}/${id}`);
  },
};
```

Regras:

- service nao contem estado de UI;
- service nao exibe toast;
- service nao navega;
- service retorna `response.data` quando API retorna corpo;
- mutations sem corpo apenas aguardam a chamada.

## Forms

Padrao:

- React Hook Form;
- Zod resolver;
- `defaultValues` exportados com o schema;
- `TextField` MUI;
- `Controller` para selects ou campos controlados;
- `useEffect` para reset ao abrir dialog;
- conversores `toCriar<Entidade>Request` e `toAtualizar<Entidade>Request`.

Schema:

```ts
export const clienteSchema = z.object({
  nome: z.string().min(1, 'Informe o nome.').max(200, 'Use no maximo 200 caracteres.'),
  email: z.string().email('Informe um e-mail valido.').max(250, 'Use no maximo 250 caracteres.'),
});

export type ClienteFormValues = z.infer<typeof clienteSchema>;

export const clienteFormDefaultValues: ClienteFormValues = {
  nome: '',
  email: '',
};
```

Conversao:

```ts
export function toCriarClienteRequest(values: ClienteFormValues) {
  return {
    nome: values.nome,
    email: values.email || null,
  };
}
```

Regras:

- campos opcionais vazios viram `null`;
- enums em select podem ficar como string no form e virar numero no payload;
- mensagens de validacao ficam no schema;
- botao salva deve respeitar `isSaving`;
- dialog nao fecha durante salvamento.

## Paginas de Listagem

Padrao visual:

- raiz com `Stack spacing={3}`;
- cabecalho com `Typography h5`;
- descricao curta em `Typography body2`;
- botao principal com icone e `variant="contained"`;
- filtros dentro de `Paper`;
- tabela dentro de `Paper` + `TableContainer`;
- loading com `CircularProgress`;
- erro com `Alert`;
- vazio com mensagem centralizada;
- linha clicavel navega para detalhe;
- acoes por linha usam `IconButton` + `Tooltip`;
- celula de acoes chama `event.stopPropagation()`;
- exclusoes/cancelamentos usam confirm dialog.

## Confirm Dialog

Padrao:

```ts
const { confirm, ConfirmDialog } = useConfirmDialog();

const handleRemove = async (item: ClienteDto) => {
  if (await confirm({
    title: 'Remover cliente',
    message: `Deseja remover "${item.nome}"?`,
    confirmLabel: 'Remover',
    color: 'error',
  })) {
    removerMutation.mutate(item.id);
  }
};
```

O componente `{ConfirmDialog}` deve estar no JSX da pagina.

## Layout Administrativo

Componentes:

```text
components/layout/AdminLayout.tsx
components/layout/AdminPage.tsx
components/navigation/RequireAuth.tsx
components/navigation/RequireAcesso.tsx
```

Fluxo:

```text
Page -> RequireAcesso -> AdminPage -> RequireAuth -> AdminLayout -> PageContent
```

Layout deve conter:

- sidebar fixa no desktop;
- drawer temporario no mobile;
- topbar;
- seletor de tenant se multitenant;
- area de usuario;
- botao home;
- botao logout;
- menus filtrados por recurso.

## Tema

Arquivo:

```text
frontend/src/theme/theme.ts
```

Padrao:

```ts
export const theme = createTheme({
  palette: {
    mode: 'light',
    primary: {
      main: '#1f5eff',
    },
    secondary: {
      main: '#344054',
    },
    background: {
      default: '#f6f8fb',
      paper: '#ffffff',
    },
  },
  shape: {
    borderRadius: 8,
  },
  typography: {
    fontFamily: ['Inter', 'Roboto', 'Arial', 'sans-serif'].join(','),
    h3: {
      letterSpacing: 0,
    },
    h5: {
      letterSpacing: 0,
    },
  },
  components: {
    MuiButton: {
      defaultProps: {
        disableElevation: true,
      },
      styleOverrides: {
        root: {
          textTransform: 'none',
          fontWeight: 600,
        },
      },
    },
    MuiPaper: {
      styleOverrides: {
        root: {
          backgroundImage: 'none',
        },
      },
    },
  },
});
```

CSS global:

```css
html,
body {
  max-width: 100vw;
  min-height: 100vh;
  margin: 0;
}

body {
  background: #f6f8fb;
}

a {
  color: inherit;
  text-decoration: none;
}

* {
  box-sizing: border-box;
}
```

## Labels e Formatadores

Arquivos:

```text
frontend/src/utils/enumLabels.ts
frontend/src/utils/formatters.ts
frontend/src/lib/navigation/navigationLabels.ts
```

Regras:

- labels de enums em `Record<Enum, string>`;
- labels de menu centralizados;
- formatacao de data com `Intl.DateTimeFormat('pt-BR')`;
- helper `toOptions` para selects.

---

# Receita Para Criar Novo Modulo

## API

Para uma entidade chamada `<Entidade>` dentro de `<Modulo>`:

1. Criar o novo script SQL numerado conforme o perfil ativo.
2. Revisar constraints, FKs, indices e isolamento por `tenant_id`.
3. Aplicar o script com `ExecutarScriptsDB.ps1`.
4. Validar a estrutura diretamente no banco.
5. Executar `Scaffold-EF.ps1`.
6. Revisar todo o diff gerado no `DbContext` e nos modelos de persistencia.
7. Criar ou atualizar a entidade em `Domain/Entities/<Entidade>.cs`.
8. Criar enums necessarios em `Domain/Enums`.
9. Criar interface `Domain/Repositories/I<Entidade>Repository.cs`.
10. Criar ou atualizar o mapper entre persistencia e dominio.
11. Criar repositorio concreto em `Infrastructure/Repositories/<Entidade>Repository.cs`.
12. Registrar repositorio em `Infrastructure/DependencyInjection.cs`.
13. Criar feature em `Application/Features/<Modulo>`.
14. Criar `<Entidade>Dto` e `<Entidade>Mapper`.
15. Criar commands, queries, handlers e validators.
16. Validar tenant/contexto.
17. Retornar `NotificationResult` ou `NotificationResult<T>`.
18. Criar controller `<Modulo>Controller` e usar `ToActionResult()`.
19. Adicionar policy se houver permissao especifica.
20. Criar testes unitarios ou de integracao conforme risco.
21. Executar build, testes e nova validacao do schema em banco vazio.

## Frontend

Para a mesma entidade:

1. Criar DTO em `frontend/src/types/contracts.ts`.
2. Criar enum em `frontend/src/types/enums.ts`, se necessario.
3. Criar labels em `frontend/src/utils/enumLabels.ts`, se necessario.
4. Adicionar endpoint em `frontend/src/lib/api/endpoints.ts`.
5. Criar pasta `frontend/src/features/<modulo>`.
6. Criar `types.ts`.
7. Criar `services/<modulo>Service.ts`.
8. Criar `<modulo>QueryKeys.ts`.
9. Criar `schemas/<entidade>Schema.ts`.
10. Criar `components/<Modulo>PageContent.tsx`.
11. Criar `components/<Entidade>FormDialog.tsx`.
12. Criar `components/<Entidade>DetailPageContent.tsx`, se houver detalhe.
13. Criar rota em `frontend/src/app/<modulo>/page.tsx`.
14. Criar rota `frontend/src/app/<modulo>/[id]/page.tsx`, se houver detalhe.
15. Envolver pagina com `RequireAcesso` e `AdminPage`.
16. Adicionar item no `AdminLayout`.
17. Adicionar recurso no catalogo backend e frontend.
18. Usar `showToast` e `getApiErrorMessage`.
19. Invalidar queries no sucesso de mutation.

## Checklist API + Front

Antes de considerar o modulo pronto:

- DTO C# e TypeScript estao sincronizados;
- enums C# e TypeScript possuem os mesmos valores;
- endpoint existe no controller e em `endpoints.ts`;
- payload do form bate com command/request da API;
- rota exige tenant apenas quando necessario;
- policy backend tem recurso correspondente no frontend;
- query key inclui tenant/contexto quando necessario;
- erros usam `NotificationResult`;
- frontend trata erro com `getApiErrorMessage`;
- sucesso de mutation invalida queries corretas;
- build backend compila;
- lint/build frontend passam.

---

# Padroes de Nomenclatura

## Backend

```text
Projeto:
  <NomeProjeto>.Domain
  <NomeProjeto>.Application
  <NomeProjeto>.Infrastructure
  <NomeProjeto>.WebApi

Entidade:
  Cliente
  ClienteDto
  ClienteMapper
  IClienteRepository
  ClienteRepository

Command:
  CriarClienteCommand
  CriarClienteCommandHandler
  CriarClienteCommandValidator

Query:
  ListarClientesQuery
  ListarClientesQueryHandler
  ObterClienteQuery
  ObterClienteQueryHandler

Controller:
  ClientesController

Tabela:
  Cliente

Indice:
  IX_Cliente_TenantId
  UX_Cliente_TenantId_Codigo
```

## Frontend

```text
Feature:
  features/clientes

Service:
  clientesService

Query Keys:
  clienteKeys

Components:
  ClientesPageContent
  ClienteFormDialog
  ClienteDetailPageContent

Schema:
  clienteSchema
  ClienteFormValues
  clienteFormDefaultValues

Routes:
  /clientes
  /clientes/[id]

Types:
  ClienteFilters
  CriarClienteRequest
  AtualizarClienteRequest
```

## Rotas HTTP

```text
GET    /api/clientes
GET    /api/clientes/{id}
POST   /api/clientes
PUT    /api/clientes/{id}
DELETE /api/clientes/{id}
```

Para acoes de negocio:

```text
POST /api/clientes/{id}/ativar
POST /api/clientes/{id}/inativar
POST /api/pedidos/{id}/aprovar
POST /api/pedidos/{id}/cancelar
```

---

# Comandos Padrao

## Processo de Entrega

Toda alteracao deve estar vinculada a uma issue do Linear e seguir:

```text
Ready -> In Progress -> branch -> commit -> Pull Request -> Testing -> merge -> Done
```

Regras:

- branch parte da `main` atualizada e usa o nome sugerido pelo Linear;
- titulo do commit e do PR inclui o identificador da issue;
- PR tem `main` como base e descreve alteracoes e validacoes;
- merge ocorre somente depois de revisar diff, build e testes aplicaveis;
- branch remota e local sao removidas depois do merge;
- conhecimento relevante e registrado na issue, preferencialmente em Problema, Causa e Solucao;
- Linear e a fonte de controle do trabalho;
- Slack centraliza comunicacao e visibilidade, mas nao substitui a issue nem a documentacao;
- notificacoes automaticas devem ser acionaveis e evitar ruido;
- integracao propria do Worker com Slack so e criada quando existirem eventos reais de negocio ou operacao.

## Rodar Tudo

```powershell
.\scriptsPS\Start-All.ps1
```

O script inicia o banco do perfil ativo, API, Worker e frontend, validando antes se a persistencia esta saudavel.

## Rodar API

```powershell
.\scriptsPS\Start-Api.ps1
```

## Rodar Worker

```powershell
.\scriptsPS\Start-Worker.ps1
```

Equivalente:

```powershell
dotnet run --project src\<NomeProjeto>.Worker\<NomeProjeto>.Worker.csproj
```

Equivalente:

```powershell
dotnet run --project src\<NomeProjeto>.WebApi\<NomeProjeto>.WebApi.csproj --launch-profile http
```

## Rodar Frontend

```powershell
.\scriptsPS\Start-Frontend.ps1
```

## Banco

Consulte a secao de comandos do perfil de banco ativo.

## Validacao

Backend:

```powershell
dotnet build <NomeProjeto>.sln --no-restore
dotnet test <NomeProjeto>.sln --no-build
```

Frontend:

```powershell
cd frontend
fnm use
npm run lint
npm run build
```

---

# Principios Obrigatorios

- API deve ser separada em Domain, Application, Infrastructure e WebApi.
- Controllers devem ser finos.
- Casos de uso ficam em handlers MediatR.
- Validacao de entrada fica em FluentValidation.
- Erro de negocio esperado usa `NotificationResult`.
- Exception e reservada para falha inesperada.
- Repositorios nao chamam `SaveChangesAsync`.
- Commit acontece via `IUnitOfWork`.
- No VetCommission, o tenant deve ser validado em todos os handlers e processamentos com dados isolados.
- Frontend nao acessa API fora de services.
- Frontend nao duplica chamadas HTTP dentro de componentes.
- React Query gerencia cache de dados remotos.
- Forms usam React Hook Form + Zod.
- DTOs e enums devem ser sincronizados entre backend e frontend.
- Permissoes devem ter catalogo equivalente no backend e frontend.
- Database First e obrigatorio: primeiro alterar o banco por script versionado, depois atualizar o EF Core por scaffold.
- Scripts SQL do perfil ativo sao a fonte de verdade estrutural do banco.
- Arquivos gerados pelo scaffold nao recebem regras de negocio nem alteracoes manuais.
- Processamentos demorados nao executam dentro da requisicao HTTP.
- WebApi e Worker sao executaveis separados da mesma solucao.
- Jobs devem ser persistentes, idempotentes e seguros para concorrencia.
- Notificacoes externas usam Outbox e nao controlam a transacao de negocio.
- Cada modulo novo deve seguir a mesma estrutura de arquivos e nomenclatura.

## O Que Nao Entra Neste Padrao Global

- Regras especificas de um produto.
- Nomes de entidades de um dominio real.
- Fluxos operacionais particulares.
- Integracoes externas especificas.
- Layouts de marca especificos.
- Seeds de negocio especificos.
- Rotas que so fazem sentido para um produto.

Esses elementos devem ser definidos no projeto concreto, mantendo a arquitetura deste documento como base.

---
