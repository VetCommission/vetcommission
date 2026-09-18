namespace VetCommission.Application.Common.Errors;

public sealed record NotificationError(string Code, string Message, string? Field = null);
