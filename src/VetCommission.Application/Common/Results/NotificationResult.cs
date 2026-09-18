using VetCommission.Application.Common.Errors;

namespace VetCommission.Application.Common.Results;

public class NotificationResult : INotificationResult<NotificationResult>
{
    protected NotificationResult(bool isSuccess, IReadOnlyCollection<NotificationError> errors)
    {
        IsSuccess = isSuccess;
        Errors = errors;
    }

    public bool IsSuccess { get; }

    public bool IsFailure => !IsSuccess;

    public IReadOnlyCollection<NotificationError> Errors { get; }

    public static NotificationResult Success()
    {
        return new NotificationResult(true, Array.Empty<NotificationError>());
    }

    public static NotificationResult Failure(params NotificationError[] errors)
    {
        return Failure(errors.AsEnumerable());
    }

    public static NotificationResult Failure(IEnumerable<NotificationError> errors)
    {
        return new NotificationResult(false, errors.ToArray());
    }
}

public sealed class NotificationResult<T> : NotificationResult, INotificationResult<NotificationResult<T>>
{
    private NotificationResult(bool isSuccess, T? value, IReadOnlyCollection<NotificationError> errors)
        : base(isSuccess, errors)
    {
        Value = value;
    }

    public T? Value { get; }

    public static NotificationResult<T> Success(T value)
    {
        return new NotificationResult<T>(true, value, Array.Empty<NotificationError>());
    }

    public new static NotificationResult<T> Failure(params NotificationError[] errors)
    {
        return Failure(errors.AsEnumerable());
    }

    public new static NotificationResult<T> Failure(IEnumerable<NotificationError> errors)
    {
        return new NotificationResult<T>(false, default, errors.ToArray());
    }
}
