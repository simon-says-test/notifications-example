using System.Collections.Generic;
using Notifications.Common.Models;

namespace Notifications.Common.Interfaces
{
    public interface INotificationsAccess
    {
        IEnumerable<NotificationModel> GetAllNotifications();
        IEnumerable<NotificationModel> GetNotificationsForUser(int userId);
        TemplateModel GetTemplate(EventModel data);
        void CreateNotification(NotificationModel notification);
    }
}
