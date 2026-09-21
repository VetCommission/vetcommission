using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using Testcontainers.PostgreSql;

namespace VetCommission.IntegrationTests.WebApi;

public sealed class WebApiTestFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly PostgreSqlContainer _postgres = new PostgreSqlBuilder("postgres:17-alpine")
        .WithDatabase("vetcommission_tests")
        .WithUsername("postgres")
        .WithPassword("postgres")
        .Build();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseSetting("ConnectionStrings:DefaultConnection", _postgres.GetConnectionString());
        builder.ConfigureServices(services =>
            services.AddDataProtection().UseEphemeralDataProtectionProvider());
    }

    public async Task InitializeAsync()
    {
        await _postgres.StartAsync();
        await ApplyDatabaseScriptsAsync();
        await SeedIsolationScenarioAsync();
    }

    async Task IAsyncLifetime.DisposeAsync()
    {
        await DisposeAsync();
        await _postgres.DisposeAsync();
    }

    private async Task ApplyDatabaseScriptsAsync()
    {
        await using var connection = new NpgsqlConnection(_postgres.GetConnectionString());
        await connection.OpenAsync();

        var scriptsDirectory = Path.GetFullPath(
            Path.Combine(AppContext.BaseDirectory, "../../../../../database/scripts"));

        foreach (var path in Directory.EnumerateFiles(scriptsDirectory, "V*.sql").OrderBy(path => path))
        {
            await using var command = new NpgsqlCommand(await File.ReadAllTextAsync(path), connection);
            await command.ExecuteNonQueryAsync();
        }
    }

    private async Task SeedIsolationScenarioAsync()
    {
        const string sql = """
            INSERT INTO core.tenant (id, name, slug, timezone, active)
            VALUES ('11111111-1111-1111-1111-111111111112', 'Tenant B', 'tenant-b', 'America/Sao_Paulo', true);

            INSERT INTO core."user" (id, name, email, normalized_email, password_hash, active)
            SELECT
                '22222222-2222-2222-2222-222222222223',
                'Usuario Multitenant',
                'multitenant@vetcommission.local',
                'MULTITENANT@VETCOMMISSION.LOCAL',
                password_hash,
                true
            FROM core."user"
            WHERE id = '22222222-2222-2222-2222-222222222222';

            INSERT INTO core."user" (id, name, email, normalized_email, password_hash, active)
            SELECT
                '22222222-2222-2222-2222-222222222224',
                'Usuario Exclusivo Tenant B',
                'tenant-b@vetcommission.local',
                'TENANT-B@VETCOMMISSION.LOCAL',
                password_hash,
                true
            FROM core."user"
            WHERE id = '22222222-2222-2222-2222-222222222222';

            INSERT INTO core.access_group (id, tenant_id, code, name, active)
            VALUES
                ('33333333-3333-3333-3333-333333333334', '11111111-1111-1111-1111-111111111112', 'LIMITADO', 'Limitado', true);

            INSERT INTO core.access_group_resource (access_group_id, access_resource_id)
            SELECT '33333333-3333-3333-3333-333333333334', id
            FROM core.access_resource
            WHERE resource_key = 'app.access';

            INSERT INTO core.user_tenant (id, user_id, tenant_id, access_group_id, active, is_default)
            VALUES
                ('55555555-5555-5555-5555-555555555552', '22222222-2222-2222-2222-222222222223', '11111111-1111-1111-1111-111111111111', '33333333-3333-3333-3333-333333333332', true, true),
                ('55555555-5555-5555-5555-555555555553', '22222222-2222-2222-2222-222222222223', '11111111-1111-1111-1111-111111111112', '33333333-3333-3333-3333-333333333334', true, false),
                ('55555555-5555-5555-5555-555555555554', '22222222-2222-2222-2222-222222222224', '11111111-1111-1111-1111-111111111112', '33333333-3333-3333-3333-333333333334', true, true);

            INSERT INTO business.professional (id, tenant_id, name, role, active)
            VALUES
                ('66666666-6666-6666-6666-666666666661', '11111111-1111-1111-1111-111111111111', 'Profissional A', 'Veterinario', true),
                ('66666666-6666-6666-6666-666666666662', '11111111-1111-1111-1111-111111111112', 'Profissional B', 'Veterinario', true);
            """;

        await using var connection = new NpgsqlConnection(_postgres.GetConnectionString());
        await connection.OpenAsync();
        await using var command = new NpgsqlCommand(sql, connection);
        await command.ExecuteNonQueryAsync();
    }
}
