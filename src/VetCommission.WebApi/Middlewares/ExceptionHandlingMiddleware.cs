using System.Text.Json;
using VetCommission.Application.Common.Errors;

namespace VetCommission.WebApi.Middlewares;

public sealed class ExceptionHandlingMiddleware(
    RequestDelegate next,
    ILogger<ExceptionHandlingMiddleware> logger)
{
    private static readonly JsonSerializerOptions SerializerOptions = new(JsonSerializerDefaults.Web);

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception exception)
        {
            logger.LogError(
                exception,
                "Erro inesperado. CorrelationId: {CorrelationId}",
                context.TraceIdentifier);

            if (context.Response.HasStarted)
            {
                throw;
            }

            context.Response.Clear();
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Response.ContentType = "application/json";

            var response = new ErrorEnvelope(
                [new NotificationError(ErrorCodes.Unexpected, "Ocorreu um erro inesperado.")]);

            await JsonSerializer.SerializeAsync(
                context.Response.Body,
                response,
                SerializerOptions,
                cancellationToken: context.RequestAborted);
        }
    }

    private sealed record ErrorEnvelope(IReadOnlyCollection<NotificationError> Errors);
}
