using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using FluentAssertions;

namespace VetCommission.IntegrationTests.WebApi;

[Collection(WebApiCollection.Name)]
public sealed class MultiTenantIsolationTests(WebApiTestFactory factory)
{
    private const string TenantA = "11111111-1111-1111-1111-111111111111";
    private const string TenantB = "11111111-1111-1111-1111-111111111112";
    private const string ProfessionalB = "66666666-6666-6666-6666-666666666662";
    private const string TenantBOnlyUser = "22222222-2222-2222-2222-222222222224";

    [Fact]
    public async Task UserWithAccessInTenantA_CanListTenantAProfessionals()
    {
        using var client = await CreateAuthenticatedClientAsync(
            "admin@vetcommission.local",
            TenantA);

        var response = await client.GetAsync("/api/profissionais");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task UserFromTenantA_CannotUseTenantBHeader()
    {
        using var client = await CreateAuthenticatedClientAsync(
            "admin@vetcommission.local",
            TenantB);

        var response = await client.GetAsync("/api/profissionais");

        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task UserWithoutRequiredResourceInTenantB_CannotReuseResourceFromTenantA()
    {
        using var client = await CreateAuthenticatedClientAsync(
            "multitenant@vetcommission.local",
            TenantB);

        var response = await client.GetAsync("/api/profissionais");

        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task TenantA_CannotDiscoverProfessionalFromTenantBById()
    {
        using var client = await CreateAuthenticatedClientAsync(
            "admin@vetcommission.local",
            TenantA);

        var response = await client.GetAsync($"/api/profissionais/{ProfessionalB}");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        (await response.Content.ReadAsStringAsync()).Should().NotContain("Profissional B");
    }

    [Fact]
    public async Task TenantA_CannotChangeProfessionalFromTenantBById()
    {
        using var client = await CreateAuthenticatedClientAsync(
            "admin@vetcommission.local",
            TenantA);
        using var update = new HttpRequestMessage(HttpMethod.Put, $"/api/profissionais/{ProfessionalB}")
        {
            Content = JsonContent.Create(new
            {
                name = "Alterado",
                role = "Veterinario",
            }),
        };

        var updateResponse = await client.SendAsync(update);
        var activateResponse = await client.PostAsync(
            $"/api/profissionais/{ProfessionalB}/ativar",
            content: null);
        var deactivateResponse = await client.PostAsync(
            $"/api/profissionais/{ProfessionalB}/inativar",
            content: null);

        updateResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
        activateResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
        deactivateResponse.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task TenantA_CannotAssociateProfessionalWithUserExclusiveToTenantB()
    {
        using var client = await CreateAuthenticatedClientAsync(
            "admin@vetcommission.local",
            TenantA);

        var response = await client.PostAsJsonAsync("/api/profissionais", new
        {
            name = "Associacao indevida",
            role = "Veterinario",
            userId = TenantBOnlyUser,
        });

        response.StatusCode.Should().BeOneOf(HttpStatusCode.BadRequest, HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task TenantScopedEndpoint_RejectsMissingTenantHeader()
    {
        using var client = await CreateAuthenticatedClientAsync(
            "admin@vetcommission.local",
            tenantId: null);

        var response = await client.GetAsync("/api/profissionais");

        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task TenantScopedEndpoint_RejectsInvalidTenantHeader()
    {
        using var client = await CreateAuthenticatedClientAsync(
            "admin@vetcommission.local",
            tenantId: "not-a-guid");

        var response = await client.GetAsync("/api/profissionais");

        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task CurrentSession_CanBeRestoredBeforeSelectingTenant()
    {
        using var client = await CreateAuthenticatedClientAsync(
            "admin@vetcommission.local",
            tenantId: null);

        var response = await client.GetAsync("/api/auth/me");

        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task ProfessionalMasterData_RequireTheirSpecificResources()
    {
        using var rolesClient = await CreateAuthenticatedClientAsync(
            "funcoes@vetcommission.local",
            TenantA);
        using var professionalsClient = await CreateAuthenticatedClientAsync(
            "profissionais@vetcommission.local",
            TenantA);

        var authorizedResponse = await rolesClient.GetAsync("/api/funcoes-cargos");
        var forbiddenResponse = await professionalsClient.GetAsync("/api/funcoes-cargos");
        var authorizedSpecialtiesResponse = await rolesClient.GetAsync("/api/especialidades");
        var forbiddenSpecialtiesResponse = await professionalsClient.GetAsync("/api/especialidades");

        authorizedResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        forbiddenResponse.StatusCode.Should().Be(HttpStatusCode.Forbidden);
        authorizedSpecialtiesResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        forbiddenSpecialtiesResponse.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    private async Task<HttpClient> CreateAuthenticatedClientAsync(string email, string? tenantId)
    {
        var client = factory.CreateClient();
        var loginResponse = await client.PostAsJsonAsync("/api/auth/login", new
        {
            email,
            senha = "qwas",
        });
        loginResponse.EnsureSuccessStatusCode();

        using var payload = JsonDocument.Parse(await loginResponse.Content.ReadAsStringAsync());
        var accessToken = payload.RootElement.GetProperty("accessToken").GetString();
        accessToken.Should().NotBeNullOrWhiteSpace();

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        if (tenantId is not null)
        {
            client.DefaultRequestHeaders.Add("X-Tenant-Id", tenantId);
        }

        return client;
    }
}
