using FluentAssertions;
using FluentValidation;
using MediatR;
using VetCommission.Application.Common.Behaviors;
using VetCommission.Application.Common.Errors;
using VetCommission.Application.Common.Results;

namespace VetCommission.UnitTests.Application.Common.Behaviors;

public sealed class ValidationBehaviorTests
{
    [Fact]
    public async Task Handle_WhenRequestIsInvalid_ShouldReturnErrorsWithoutCallingHandler()
    {
        var behavior = new ValidationBehavior<TestRequest, NotificationResult<string>>(
            [new TestRequestValidator()]);
        var handlerWasCalled = false;

        Task<NotificationResult<string>> Next(CancellationToken _)
        {
            handlerWasCalled = true;
            return Task.FromResult(NotificationResult<string>.Success("executado"));
        }

        var result = await behavior.Handle(new TestRequest(string.Empty), Next, CancellationToken.None);

        result.IsFailure.Should().BeTrue();
        result.Errors.Should().ContainSingle(error =>
            error.Code == ErrorCodes.Validation &&
            error.Field == nameof(TestRequest.Name));
        handlerWasCalled.Should().BeFalse();
    }

    [Fact]
    public async Task Handle_WhenRequestIsValid_ShouldCallHandler()
    {
        var behavior = new ValidationBehavior<TestRequest, NotificationResult<string>>(
            [new TestRequestValidator()]);
        var handlerWasCalled = false;

        Task<NotificationResult<string>> Next(CancellationToken _)
        {
            handlerWasCalled = true;
            return Task.FromResult(NotificationResult<string>.Success("executado"));
        }

        var result = await behavior.Handle(new TestRequest("Clínica"), Next, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be("executado");
        handlerWasCalled.Should().BeTrue();
    }

    private sealed record TestRequest(string Name) : IRequest<NotificationResult<string>>;

    private sealed class TestRequestValidator : AbstractValidator<TestRequest>
    {
        public TestRequestValidator()
        {
            RuleFor(request => request.Name).NotEmpty();
        }
    }
}
