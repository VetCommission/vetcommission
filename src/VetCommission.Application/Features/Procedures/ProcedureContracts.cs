using MediatR; using VetCommission.Application.Common.Results;
namespace VetCommission.Application.Features.Procedures;
public sealed record ProcedureDto(Guid Id,Guid TenantId,Guid ClinicId,Guid CategoryId,string? Code,string Name,string? Description,decimal DefaultValue,bool Active);
public sealed record ListProceduresQuery(int Page=1,int PageSize=20,string? Search=null,Guid? CategoryId=null):IRequest<NotificationResult<PagedResult<ProcedureDto>>>;
public sealed record GetProcedureQuery(Guid Id):IRequest<NotificationResult<ProcedureDto>>;
public sealed record SaveProcedureCommand(Guid? Id,Guid CategoryId,string? Code,string Name,string? Description,decimal DefaultValue):IRequest<NotificationResult<ProcedureDto>>;
public sealed record SetProcedureActiveCommand(Guid Id,bool Active):IRequest<NotificationResult<ProcedureDto>>;
public interface IProcedureRepository{Task<PagedResult<ProcedureDto>> ListAsync(Guid t,Guid c,int p,int s,string? search,Guid? category,CancellationToken ct);Task<ProcedureDto?> GetAsync(Guid t,Guid c,Guid id,CancellationToken ct);Task<ProcedureDto?> SaveAsync(Guid t,Guid c,SaveProcedureCommand v,CancellationToken ct);Task<ProcedureDto?> SetActiveAsync(Guid t,Guid c,Guid id,bool active,CancellationToken ct);}
