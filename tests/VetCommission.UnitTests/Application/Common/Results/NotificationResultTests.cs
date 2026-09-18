using FluentAssertions;
using VetCommission.Application.Common.Errors;
using VetCommission.Application.Common.Results;

namespace VetCommission.UnitTests.Application.Common.Results;

public sealed class NotificationResultTests
{
    [Fact]
    public void Success_ShouldCreateSuccessfulResultWithoutErrors()
    {
        var result = NotificationResult.Success();

        result.IsSuccess.Should().BeTrue();
        result.IsFailure.Should().BeFalse();
        result.Errors.Should().BeEmpty();
    }

    [Fact]
    public void Failure_ShouldPreserveAllErrors()
    {
        var errors = new[]
        {
            new NotificationError(ErrorCodes.Validation, "Nome é obrigatório.", "Nome"),
            new NotificationError(ErrorCodes.Conflict, "Registro duplicado.")
        };

        var result = NotificationResult.Failure(errors);

        result.IsFailure.Should().BeTrue();
        result.Errors.Should().BeEquivalentTo(errors, options => options.WithStrictOrdering());
    }

    [Fact]
    public void GenericSuccess_ShouldPreserveValue()
    {
        var result = NotificationResult<string>.Success("resultado");

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be("resultado");
        result.Errors.Should().BeEmpty();
    }

    [Fact]
    public void GenericFailure_ShouldNotExposeValue()
    {
        var error = new NotificationError(ErrorCodes.NotFound, "Não encontrado.");

        var result = NotificationResult<string>.Failure(error);

        result.IsFailure.Should().BeTrue();
        result.Value.Should().BeNull();
        result.Errors.Should().ContainSingle().Which.Should().Be(error);
    }
}
