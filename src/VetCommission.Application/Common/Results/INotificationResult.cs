using VetCommission.Application.Common.Errors;

namespace VetCommission.Application.Common.Results;

public interface INotificationResult<TSelf>
    where TSelf : INotificationResult<TSelf>
{
    static abstract TSelf Failure(IEnumerable<NotificationError> errors);
}
