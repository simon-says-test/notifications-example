using System.Collections.Generic;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using Notifications.Common.Models;

namespace Notifications.Common.Interfaces
{
    public interface INotificationsService
    {
        Task<Result<List<NotificationModel>>> GetNotifications(int? userId);

        Task<Result<NotificationModel>> CreateNotification(EventModel eventModel);
    }
}
