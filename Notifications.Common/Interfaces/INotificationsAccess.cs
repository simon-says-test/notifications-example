using System.Linq;
using CSharpFunctionalExtensions;
using Notifications.Common.Models;

namespace Notifications.Common.Interfaces
{
    public interface INotificationsAccess
    {
        Result<IQueryable<NotificationModel>> GetAllNotifications();
        Result<IQueryable<NotificationModel>> GetNotificationsForUser(int userId);
        Result<TemplateModel> GetTemplate(EventModel data);
        Result CreateNotification(NotificationModel notification);
    }
}
