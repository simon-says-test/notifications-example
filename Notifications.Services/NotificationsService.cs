using System;
using System.Collections.Generic;
using System.Linq;
using CSharpFunctionalExtensions;
using Notifications.Common.Fields;
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

        public Result<IQueryable<NotificationModel>> GetNotifications(int? userId)
        {
            return userId == null
                ? this.notificationsAccess.GetAllNotifications()
                : this.notificationsAccess.GetNotificationsForUser(userId.Value);
        }

        public Result<NotificationModel> CreateNotification(EventModel eventModel)
        {
            Result<TemplateModel> templateResult = this.notificationsAccess.GetTemplate(eventModel);
            Result<string> bodyResult = templateResult.Bind((result) => MapEventDataToBody(result.Body, eventModel.Data));
            if (templateResult.IsFailure || bodyResult.IsFailure)
            {
                return Result.Failure<NotificationModel>(string.IsNullOrEmpty(templateResult.Error) ? bodyResult.Error : templateResult.Error);
            }

            return templateResult
                .Map((result) => new NotificationModel
                {
                    Id = Guid.NewGuid(),
                    EventType = result.EventType,
                    Title = result.Title,
                    Body = EventBody.Create(bodyResult.Value).Value,
                    UserId = eventModel.UserId
                })
                .Check((result) => this.notificationsAccess.CreateNotification(result));
        }

        public Result<string> MapEventDataToBody(string body, EventDataModel eventData)
        {
            try
            {
                string result = body
                    .Replace("{FirstName}", eventData.FirstName)
                    .Replace("{AppointmentDateTime}", eventData.AppointmentDateTime.ToShortDateString() + " " + eventData.AppointmentDateTime.ToShortTimeString())
                    .Replace("{OrganisationName}", eventData.OrganisationName)
                    .Replace("{Reason}", eventData.Reason);
                return Result.Success(result);
            }
            catch
            {
                return Result.Failure<string>("Bad event data");
            }
        }
    }
}
