using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using Notifications.Common.Enums;
using Notifications.Common.Interfaces;
using Notifications.Common.Models;
using Notifications.DataAccess.Entities;

namespace Notifications.DataAccess.Access
{
    public class NotificationsAccess : INotificationsAccess
    {
        private readonly NotificationsDbContext dbContext;

        public NotificationsAccess(NotificationsDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void CreateNotification(NotificationModel notification)
        {
            EventType eventType = (EventType)Enum.Parse(typeof(EventType), notification.EventType, true);

            dbContext.Notifications.Add(
                new NotificationEntity
                {
                    Id = notification.Id,
                    EventType = eventType,
                    Title = notification.Title,
                    Body = notification.Body,
                    UserId = notification.UserId
                });
            dbContext.SaveChanges();
        }

        public IEnumerable<NotificationModel> GetAllNotifications()
        {
            return dbContext.Notifications
                .OrderBy(x => x.Id)
                .Select(x => new NotificationModel()
                {
                    Id = x.Id,
                    EventType = x.EventType.ToString(),
                    Title = x.Title,
                    Body = x.Body,
                    UserId = x.UserId
                });
        }

        public IEnumerable<NotificationModel> GetNotificationsForUser(int userId)
        {
            return dbContext.Notifications
                .Where(x => x.UserId == userId)
                .OrderBy(x => x.Id)
                .Select(x => new NotificationModel()
                {
                    Id = x.Id,
                    EventType = x.EventType.ToString(),
                    Title = x.Title,
                    Body = x.Body,
                    UserId = x.UserId
                });
        }

        public TemplateModel GetTemplate(EventModel data)
        {
            try
            {
                EventType eventType = (EventType)Enum.Parse(typeof(EventType), data.Type, true);

                TemplateEntity template = dbContext.Templates.Single(x => x.EventType == eventType);

                return new TemplateModel()
                {
                    Id = template.Id,
                    EventType = template.EventType.ToString(),
                    Body = template.Body,
                    Title = template.Title
                };
            }
            catch (Exception ex) when (ex is ArgumentNullException || ex is ArgumentException || ex is OverflowException || ex is InvalidOperationException)
            {
                throw new ArgumentException($"The requested Event Type ({0}) was not found.", data.Type);
            }
        }
    }
}
