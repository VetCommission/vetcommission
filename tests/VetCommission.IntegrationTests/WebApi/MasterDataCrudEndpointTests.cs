using System.Net; using System.Net.Http.Headers; using System.Net.Http.Json; using System.Text.Json; using FluentAssertions;
namespace VetCommission.IntegrationTests.WebApi;
[Collection(WebApiCollection.Name)] public sealed class MasterDataCrudEndpointTests(WebApiTestFactory factory)
{
 const string Tenant="11111111-1111-1111-1111-111111111111"; const string Clinic="77777777-7777-7777-7777-777777777771";
 [Fact] public async Task Clinics_ListAndCreate_AreAvailable(){using var c=await Client();var list=await c.GetAsync("/api/clinicas");list.StatusCode.Should().Be(HttpStatusCode.OK);var create=await c.PostAsJsonAsync("/api/clinicas",new{name="Clinica de Teste"});create.StatusCode.Should().Be(HttpStatusCode.OK);}
 [Fact] public async Task Roles_ListAndCreate_AreScopedToClinic(){using var c=await Client();var list=await c.GetAsync("/api/funcoes-cargos");list.StatusCode.Should().Be(HttpStatusCode.OK);var create=await c.PostAsJsonAsync("/api/funcoes-cargos",new{name="Cargo de Teste"});create.StatusCode.Should().Be(HttpStatusCode.OK);}
 [Fact] public async Task Specialties_ListAndCreate_AreScopedToClinic(){using var c=await Client();var list=await c.GetAsync("/api/especialidades");list.StatusCode.Should().Be(HttpStatusCode.OK);var create=await c.PostAsJsonAsync("/api/especialidades",new{name="Especialidade de Teste"});create.StatusCode.Should().Be(HttpStatusCode.OK);}
 async Task<HttpClient> Client(){var c=factory.CreateClient();var login=await c.PostAsJsonAsync("/api/auth/login",new{email="admin@vetcommission.local",senha="qwas"});login.EnsureSuccessStatusCode();using var p=JsonDocument.Parse(await login.Content.ReadAsStringAsync());c.DefaultRequestHeaders.Authorization=new AuthenticationHeaderValue("Bearer",p.RootElement.GetProperty("accessToken").GetString());c.DefaultRequestHeaders.Add("X-Tenant-Id",Tenant);c.DefaultRequestHeaders.Add("X-Clinic-Id",Clinic);return c;}
}
