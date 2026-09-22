using VetCommission.Application.Common.Results;

namespace VetCommission.Application.Features.ProfessionalMasterData;

public sealed record MasterDataItemDto(Guid Id, string Name, bool Active);
public sealed record ListProfessionalMasterDataQuery : MediatR.IRequest<NotificationResult<ProfessionalMasterDataDto>>;
public sealed record ProfessionalMasterDataDto(IReadOnlyCollection<MasterDataItemDto> Roles, IReadOnlyCollection<MasterDataItemDto> Specialties);

public interface IProfessionalMasterDataRepository
{
    Task<ProfessionalMasterDataDto> ListAsync(Guid tenantId, CancellationToken cancellationToken);
}
