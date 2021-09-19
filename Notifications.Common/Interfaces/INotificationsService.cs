using System.Collections.Generic;
using System.Linq;
using CSharpFunctionalExtensions;
using Notifications.Common.Models;

namespace Notifications.Common.Interfaces
{
    public interface INotificationsService
    {
        Result<IQueryable<NotificationModel>> GetNotifications(int? userId);

        Result<NotificationModel> CreateNotification(EventModel eventModel);
    }
}
