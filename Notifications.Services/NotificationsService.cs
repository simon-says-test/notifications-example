using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
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

        public async Task<Result<List<NotificationModel>>> GetNotifications(int? userId)
        {
            return userId == null
                ? await this.notificationsAccess.GetAllNotifications()
                : await this.notificationsAccess.GetNotificationsForUser(userId.Value);
        }

        public async Task<Result<NotificationModel>> CreateNotification(EventModel eventModel)
        {
            Result<TemplateModel> templateResult = await this.notificationsAccess.GetTemplate(eventModel);
            Result<string> bodyResult = templateResult
                .Bind((result) => MapEventDataToBody(result.Body, eventModel.Data));
            if (templateResult.IsFailure || bodyResult.IsFailure)
            {
                return Result.Failure<NotificationModel>(templateResult.IsFailure ? templateResult.Error : bodyResult.Error);
            }

            return await Task.FromResult(templateResult)
                .Map((result) => new NotificationModel
                {
                    Id = Guid.NewGuid(),
                    EventType = result.EventType,
                    Title = result.Title,
                    Body = bodyResult.Value,
                    UserId = eventModel.UserId
                })
                .Check((result) => this.notificationsAccess.CreateNotification(result));
        }

        public static Result<string> MapEventDataToBody(string body, EventDataModel eventData)
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
                return Result.Failure<string>("Bad template or event data");
            }
        }
    }
}
