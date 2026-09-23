using FluentAssertions;
using NSubstitute;
using VetCommission.Application.Common.Errors;
using VetCommission.Application.Features.Auth.Tenant;
using VetCommission.Application.Features.CommissionConfiguration;

namespace VetCommission.UnitTests.Application.CommissionConfiguration;

public sealed class CommissionHandlersTests
{
    private static readonly Guid TenantId = Guid.NewGuid();
    private static readonly Guid ClinicId = Guid.NewGuid();
    private static readonly Guid CompetencyId = Guid.NewGuid();
    private static readonly Guid ProcedureId = Guid.NewGuid();

    [Fact]
    public async Task SaveRule_ReturnsConflictWhenCompetencyIsClosed()
    {
        var repo = Substitute.For<ICommissionConfigurationRepository>();
        repo.GetCompetencyAsync(Arg.Any<Guid>(), Arg.Any<Guid>(), Arg.Any<Guid>(), Arg.Any<CancellationToken>())
            .Returns(Competency("closed"));

        var result = await Handler(repo).Handle(
            new SaveCommissionRuleCommand(null, CompetencyId, ProcedureId, "percentage", 10, null), CancellationToken.None);

        result.Errors.Should().ContainSingle(error => error.Code == ErrorCodes.Conflict);
        await repo.DidNotReceive().SaveRuleAsync(Arg.Any<Guid>(), Arg.Any<Guid>(), Arg.Any<SaveCommissionRuleCommand>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task SaveRule_RejectsPercentageWithFixedValueBeforeRepository()
    {
        var repo = Substitute.For<ICommissionConfigurationRepository>();

        var result = await Handler(repo).Handle(
            new SaveCommissionRuleCommand(null, CompetencyId, ProcedureId, "percentage", 10, 20), CancellationToken.None);

        result.Errors.Should().ContainSingle(error => error.Code == ErrorCodes.Validation);
        await repo.DidNotReceive().GetCompetencyAsync(Arg.Any<Guid>(), Arg.Any<Guid>(), Arg.Any<Guid>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task SaveRule_ReturnsNotFoundWhenProcedureDoesNotBelongToContext()
    {
        var repo = Substitute.For<ICommissionConfigurationRepository>();
        repo.GetCompetencyAsync(Arg.Any<Guid>(), Arg.Any<Guid>(), Arg.Any<Guid>(), Arg.Any<CancellationToken>())
            .Returns(Competency("open"));
        repo.ProcedureExistsAsync(Arg.Any<Guid>(), Arg.Any<Guid>(), Arg.Any<Guid>(), Arg.Any<CancellationToken>()).Returns(false);

        var result = await Handler(repo).Handle(
            new SaveCommissionRuleCommand(null, CompetencyId, ProcedureId, "fixed", null, 20), CancellationToken.None);

        result.Errors.Should().ContainSingle(error => error.Code == ErrorCodes.NotFound);
        await repo.DidNotReceive().SaveRuleAsync(Arg.Any<Guid>(), Arg.Any<Guid>(), Arg.Any<SaveCommissionRuleCommand>(), Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task SaveCompetency_ReturnsConflictWhenCompetencyIsClosed()
    {
        var repo = Substitute.For<ICommissionConfigurationRepository>();
        repo.GetCompetencyAsync(Arg.Any<Guid>(), Arg.Any<Guid>(), Arg.Any<Guid>(), Arg.Any<CancellationToken>())
            .Returns(Competency("closed"));

        var result = await Handler(repo).Handle(new SaveCompetencyCommand(CompetencyId, 2026, 9), CancellationToken.None);

        result.Errors.Should().ContainSingle(error => error.Code == ErrorCodes.Conflict);
    }

    private static CommissionHandlers Handler(ICommissionConfigurationRepository repo) =>
        new(repo, new TenantContext(TenantId), new ClinicContext(ClinicId));

    private static CompetencyDto Competency(string status) =>
        new(CompetencyId, TenantId, ClinicId, 2026, 9, status, status == "closed" ? DateTime.UtcNow : null);

    private sealed class TenantContext(Guid id) : ITenantContext
    {
        public Guid? TenantId => id;
    }

    private sealed class ClinicContext(Guid id) : IClinicContext
    {
        public Guid? ClinicId => id;
    }
}
