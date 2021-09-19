using System.Collections.Generic;
using Notifications.Common.Models;

namespace Notifications.Common.Interfaces
{
    public interface INotificationsService
    {
        IReadOnlyCollection<NotificationModel> GetNotifications(int? userId);

        NotificationModel CreateNotification(EventModel eventModel);
    }
}
