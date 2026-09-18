using FluentValidation;
using MediatR;
using VetCommission.Application.Common.Errors;
using VetCommission.Application.Common.Results;

namespace VetCommission.Application.Common.Behaviors;

public sealed class ValidationBehavior<TRequest, TResponse>(IEnumerable<IValidator<TRequest>> validators)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
    where TResponse : INotificationResult<TResponse>
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (!validators.Any())
        {
            return await next(cancellationToken);
        }

        var context = new ValidationContext<TRequest>(request);
        var validationResults = await Task.WhenAll(
            validators.Select(validator => validator.ValidateAsync(context, cancellationToken)));

        var errors = validationResults
            .SelectMany(result => result.Errors)
            .Where(failure => failure is not null)
            .Select(failure => new NotificationError(
                ErrorCodes.Validation,
                failure.ErrorMessage,
                failure.PropertyName))
            .ToArray();

        return errors.Length > 0
            ? TResponse.Failure(errors)
            : await next(cancellationToken);
    }
}
