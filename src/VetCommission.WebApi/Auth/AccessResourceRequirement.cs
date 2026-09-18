using Microsoft.AspNetCore.Authorization;

namespace VetCommission.WebApi.Auth;

public sealed record AccessResourceRequirement(string Resource) : IAuthorizationRequirement;
