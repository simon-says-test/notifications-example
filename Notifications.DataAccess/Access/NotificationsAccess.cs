using System;
using System.Collections.Generic;
using System.Linq;
using CSharpFunctionalExtensions;
using Notifications.Common.Enums;
using Notifications.Common.Fields;
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

        public Result CreateNotification(NotificationModel notification)
        {
            EventType eventType;

            try
            {
                eventType = (EventType)Enum.Parse(typeof(EventType), notification.EventType, true);
            }
            catch (Exception ex) when (ex is ArgumentNullException || ex is ArgumentException || ex is OverflowException)
            {
                return Result.Failure($"The requested Event Type ({notification.EventType}) was not found.");
            }

            dbContext.Notifications.Add(
                new NotificationEntity(notification.Id, eventType, notification.Body, notification.Title, notification.UserId));
            dbContext.SaveChanges();
            return Result.Success();
        }

        public Result<IQueryable<NotificationModel>> GetAllNotifications()
        {
            var notifications = dbContext.Notifications
                .OrderBy(x => x.Id)
                .Select(x => new NotificationModel()
                {
                    Id = x.Id,
                    EventType = x.EventType.ToString(),
                    Title = x.Title,
                    Body = x.Body,
                    UserId = x.UserId
                });
            return Result.Success(notifications);
        }

        public Result<IQueryable<NotificationModel>> GetNotificationsForUser(int userId)
        {
            var notifications = dbContext.Notifications
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
            return Result.Success(notifications);
        }

        public Result<TemplateModel> GetTemplate(EventModel data)
        {
            try
            {
                EventType eventType = (EventType)Enum.Parse(typeof(EventType), data.Type, true);

                TemplateEntity template = dbContext.Templates.Single(x => x.EventType == eventType);

                return Result.Success(
                    new TemplateModel()
                    {
                        Id = template.Id,
                        EventType = template.EventType.ToString(),
                        Body = template.Body,
                        Title = template.Title
                    });
            }
            catch (Exception ex) when (ex is ArgumentNullException || ex is ArgumentException || ex is OverflowException || ex is InvalidOperationException)
            {
                return Result.Failure<TemplateModel>($"The requested Event Type ({data.Type}) was not found.");
            }
        }
    }
}
