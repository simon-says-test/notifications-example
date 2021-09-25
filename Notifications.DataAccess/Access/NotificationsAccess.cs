using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
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

        public async Task<Result> CreateNotification(NotificationModel notification)
        {
            Result<EventType> eventTypeResult = Enum<EventType>.Create(notification.EventType);
            Result<EventBody> eventBodyResult = EventBody.Create(notification.Body);
            Result<EventTitle> eventTitleResult = EventTitle.Create(notification.Title);

            return await Task.FromResult(Result.Combine(eventTypeResult, eventBodyResult, eventTitleResult))
                .Tap(() => dbContext.Notifications.Add(
                    new NotificationEntity(notification.Id, eventTypeResult.Value, eventBodyResult.Value, eventTitleResult.Value, notification.UserId)))
                .Tap(() => dbContext.SaveChangesAsync());
        }

        public async Task<Result<List<NotificationModel>>> GetAllNotifications()
        {
            var notifications = await dbContext.Notifications
                .OrderBy(x => x.Id)
                .Select(x => new NotificationModel()
                {
                    Id = x.Id,
                    EventType = x.EventType.ToString(),
                    Title = x.Title,
                    Body = x.Body,
                    UserId = x.UserId
                })
                .ToListAsync();
            return Result.Success(notifications);
        }

        public async Task<Result<List<NotificationModel>>> GetNotificationsForUser(int userId)
        {
            var notifications = await dbContext.Notifications
                .Where(x => x.UserId == userId)
                .OrderBy(x => x.Id)
                .Select(x => new NotificationModel()
                {
                    Id = x.Id,
                    EventType = x.EventType.ToString(),
                    Title = x.Title,
                    Body = x.Body,
                    UserId = x.UserId
                })
                .ToListAsync();
            return Result.Success(notifications);
        }

        public Task<Result<TemplateModel>> GetTemplate(EventModel eventModel)
        {
            return Enum<EventType>.Create(eventModel.Type)
                .Map(result => dbContext.Templates.SingleAsync(x => x.EventType == result))
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
