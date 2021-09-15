using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Notifications.Common.Interfaces;
using Notifications.Common.Models;

namespace Notifications.Services
{
    public class NotificationsService : INotificationsService
    {
        private readonly INotificationsAccess notificationsAccess;

        public NotificationsService(INotificationsAccess notificationsAccess)
        {
            this.notificationsAccess = notificationsAccess;
        }

        public IReadOnlyCollection<NotificationModel> GetNotifications(int? userId)
        {
            return userId == null 
                ? this.notificationsAccess.GetAllNotifications().ToList()
                : this.notificationsAccess.GetNotificationsForUser(userId.Value).ToList();
        }

        public NotificationModel CreateNotification(EventModel eventModel)
        {
            TemplateModel template = this.notificationsAccess.GetTemplate(eventModel);
            NotificationModel notification = new NotificationModel
            {
                Id = Guid.NewGuid(),
                EventType = template.EventType,
                Title = template.Title,
                Body = template.Body
                    .Replace("{FirstName}", eventModel.Data.FirstName)
                    .Replace("{AppointmentDateTime}", eventModel.Data.AppointmentDateTime.ToShortDateString() + " " + eventModel.Data.AppointmentDateTime.ToShortTimeString())
                    .Replace("{OrganisationName}", eventModel.Data.OrganisationName)
                    .Replace("{Reason}", eventModel.Data.Reason),
                UserId = eventModel.UserId
            };

            this.notificationsAccess.CreateNotification(notification);

            return notification;
        }
    }
}
