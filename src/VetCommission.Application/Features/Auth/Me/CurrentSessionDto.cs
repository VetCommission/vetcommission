namespace VetCommission.Application.Features.Auth.Me;

public sealed record CurrentSessionDto(
    AuthUserDto Usuario,
    IReadOnlyList<AuthTenantDto> Tenants);
