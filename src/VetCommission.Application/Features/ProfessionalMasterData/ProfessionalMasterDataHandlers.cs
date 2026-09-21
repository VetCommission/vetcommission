using MediatR;
using VetCommission.Application.Common.Errors;
using VetCommission.Application.Common.Results;
using VetCommission.Application.Features.Auth.Tenant;

namespace VetCommission.Application.Features.ProfessionalMasterData;

public sealed class ListProfessionalMasterDataHandler(IProfessionalMasterDataRepository repository, ITenantContext tenant) : IRequestHandler<ListProfessionalMasterDataQuery, NotificationResult<ProfessionalMasterDataDto>>
{
    public async Task<NotificationResult<ProfessionalMasterDataDto>> Handle(ListProfessionalMasterDataQuery request, CancellationToken cancellationToken)
    {
        if (tenant.TenantId is not Guid tenantId) return NotificationResult<ProfessionalMasterDataDto>.Failure(new NotificationError(ErrorCodes.Forbidden, "Tenant ativo obrigatorio."));
        return NotificationResult<ProfessionalMasterDataDto>.Success(await repository.ListAsync(tenantId, cancellationToken));
    }
}
