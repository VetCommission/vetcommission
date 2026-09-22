using MediatR; using VetCommission.Application.Common.Results;
namespace VetCommission.Application.Features.ProcedureCategories;
public sealed record ProcedureCategoryDto(Guid Id, Guid TenantId, Guid ClinicId, string Name, string? Description, bool Active);
public sealed record ListProcedureCategoriesQuery(int Page=1,int PageSize=20,string? Search=null) : IRequest<NotificationResult<PagedResult<ProcedureCategoryDto>>>;
public sealed record SaveProcedureCategoryCommand(Guid? Id,string Name,string? Description) : IRequest<NotificationResult<ProcedureCategoryDto>>;
public sealed record SetProcedureCategoryActiveCommand(Guid Id,bool Active) : IRequest<NotificationResult<ProcedureCategoryDto>>;
public interface IProcedureCategoryRepository { Task<PagedResult<ProcedureCategoryDto>> ListAsync(Guid t,Guid c,int p,int s,string? search,CancellationToken ct); Task<ProcedureCategoryDto?> GetAsync(Guid t,Guid c,Guid id,CancellationToken ct); Task<ProcedureCategoryDto?> SaveAsync(Guid t,Guid c,SaveProcedureCategoryCommand command,CancellationToken ct); Task<ProcedureCategoryDto?> SetActiveAsync(Guid t,Guid c,Guid id,bool active,CancellationToken ct); }
