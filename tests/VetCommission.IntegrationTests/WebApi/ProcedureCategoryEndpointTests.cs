using System.Net; using System.Net.Http.Headers; using System.Net.Http.Json; using System.Text.Json; using FluentAssertions;
namespace VetCommission.IntegrationTests.WebApi;
[Collection(WebApiCollection.Name)] public sealed class ProcedureCategoryEndpointTests(WebApiTestFactory factory)
{
 const string Tenant="11111111-1111-1111-1111-111111111111"; const string Clinic="77777777-7777-7777-7777-777777777771";
 [Fact] public async Task CanCreateAndListCategory(){using var c=await Client();c.DefaultRequestHeaders.Add("X-Clinic-Id",Clinic);var create=await c.PostAsJsonAsync("/api/categorias-procedimentos",new{name="Exames",description="Procedimentos diagnosticos"});create.StatusCode.Should().Be(HttpStatusCode.OK);var list=await c.GetAsync("/api/categorias-procedimentos?page=1&pageSize=20");list.StatusCode.Should().Be(HttpStatusCode.OK);(await list.Content.ReadAsStringAsync()).Should().Contain("Exames");}
 [Fact] public async Task RequiresClinicContext(){using var c=await Client();var response=await c.GetAsync("/api/categorias-procedimentos");response.StatusCode.Should().Be(HttpStatusCode.Forbidden);}
 async Task<HttpClient> Client(){var c=factory.CreateClient();var login=await c.PostAsJsonAsync("/api/auth/login",new{email="admin@vetcommission.local",senha="qwas"});login.EnsureSuccessStatusCode();using var p=JsonDocument.Parse(await login.Content.ReadAsStringAsync());c.DefaultRequestHeaders.Authorization=new AuthenticationHeaderValue("Bearer",p.RootElement.GetProperty("accessToken").GetString());c.DefaultRequestHeaders.Add("X-Tenant-Id",Tenant);return c;}
}
