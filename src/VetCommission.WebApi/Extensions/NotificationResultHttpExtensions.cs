using Microsoft.AspNetCore.Mvc;
using VetCommission.Application.Common.Errors;
using VetCommission.Application.Common.Results;

namespace VetCommission.WebApi.Extensions;

public static class NotificationResultHttpExtensions
{
    public static IActionResult ToActionResult<T>(this NotificationResult<T> result, ControllerBase controller)
    {
        if (result.IsSuccess)
        {
            return controller.Ok(result.Value);
        }

        var statusCode = ResolveStatusCode(result.Errors);
        return controller.StatusCode(statusCode, new { errors = result.Errors });
    }

    private static int ResolveStatusCode(IReadOnlyCollection<NotificationError> errors)
    {
        if (errors.Any(error => error.Code == ErrorCodes.Unauthorized))
        {
            return StatusCodes.Status401Unauthorized;
        }

        if (errors.Any(error => error.Code == ErrorCodes.Forbidden))
        {
            return StatusCodes.Status403Forbidden;
        }

        if (errors.Any(error => error.Code == ErrorCodes.NotFound))
        {
            return StatusCodes.Status404NotFound;
        }

        if (errors.Any(error => error.Code == ErrorCodes.Conflict))
        {
            return StatusCodes.Status409Conflict;
        }

        return StatusCodes.Status400BadRequest;
    }
}
