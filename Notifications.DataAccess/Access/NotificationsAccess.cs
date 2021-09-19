using System;
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
            Result<EventType> eventTypeResult = Enum<EventType>.Create(notification.EventType);
            Result<EventBody> eventBodyResult = EventBody.Create(notification.Body);
            Result<EventTitle> eventTitleResult = EventTitle.Create(notification.Title);

            return Result.Combine(eventTypeResult, eventBodyResult, eventTitleResult)
                .Tap(() => dbContext.Notifications.Add(
                    new NotificationEntity(notification.Id, eventTypeResult.Value, eventBodyResult.Value, eventTitleResult.Value, notification.UserId)))
                .Tap(() => dbContext.SaveChanges());
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

        public Result<TemplateModel> GetTemplate(EventModel eventModel)
        {
            return Enum<EventType>.Create(eventModel.Type)
                .Map(result => dbContext.Templates.Single(x => x.EventType == result))
                .Map(result =>
                    new TemplateModel()
                    {
                        Id = result.Id,
                        EventType = result.EventType.ToString(),
                        Body = result.Body,
                        Title = result.Title
                    });
        }
    }
}
