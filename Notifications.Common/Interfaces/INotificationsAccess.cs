using System.Collections.Generic;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using Notifications.Common.Models;

namespace Notifications.Common.Interfaces
{
    public interface INotificationsAccess
    {
        Task<Result<List<NotificationModel>>> GetAllNotifications();
        Task<Result<List<NotificationModel>>> GetNotificationsForUser(int userId);
        Task<Result<TemplateModel>> GetTemplate(EventModel data);
        Task<Result> CreateNotification(NotificationModel notification);
    }
}
