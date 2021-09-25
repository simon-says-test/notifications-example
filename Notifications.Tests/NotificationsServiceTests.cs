using Xunit;
using Moq;
using Notifications.Common.Interfaces;
using Notifications.Common.Models;
using Notifications.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using Notifications.Common.Fields;
using CSharpFunctionalExtensions;
using System.Threading.Tasks;

namespace Notifications.Tests
{
    public class NotificationsServiceTests
    {
        [Fact]
        public async void CreateNotification()
        {
            TemplateModel templateModel = new()
            {
                Id = Guid.NewGuid(),
                EventType = "AppointmentCancelled",
                Body = "Hi {FirstName}, {OrganisationName}, {AppointmentDateTime}, {Reason}.",
                Title = "Appointment Cancelled"
            };

            EventModel eventModel = new()
            {
                UserId = 1,
                Type = "AppointmentCancelled",
                Data = new EventDataModel
                {
                    AppointmentDateTime = new DateTime(2011, 2, 10),
                    FirstName = "Bob",
                    Reason = "Can't make it",
                    OrganisationName = "Bob's builders"
                }
            };

            Mock<INotificationsAccess> notificationsAccessMock = new(MockBehavior.Strict);
            notificationsAccessMock
                .Setup(x => x.GetTemplate(It.IsAny<EventModel>()))
                .Returns(Task.FromResult(Result.Success(templateModel)));
            notificationsAccessMock
                .Setup(x => x.CreateNotification(It.IsAny<NotificationModel>()))
                .Returns(Task.FromResult(Result.Success()));

            INotificationsService notificationsService = new NotificationsService(notificationsAccessMock.Object);
            Result<NotificationModel> notificationModel = await notificationsService.CreateNotification(eventModel);

            Assert.True(notificationModel.IsSuccess);
            Assert.Equal(templateModel.EventType, notificationModel.Value.EventType);
            Assert.Equal(templateModel.Title, notificationModel.Value.Title);
            Assert.Equal(eventModel.UserId, notificationModel.Value.UserId);
            Assert.Equal("Hi Bob, Bob's builders, 10/02/2011 00:00, Can't make it.", notificationModel.Value.Body);

            notificationsAccessMock.Verify(x => x.GetTemplate(It.IsAny<EventModel>()), Times.Once);
            notificationsAccessMock.Verify(x => x.GetTemplate(eventModel), Times.Once);
            notificationsAccessMock.Verify(x => x.CreateNotification(It.IsAny<NotificationModel>()), Times.Once);
            notificationsAccessMock.Verify(x => x.CreateNotification(notificationModel.Value), Times.Once);
        }

        [Fact]
        public async void GetNotificationsForAllUsers()
        {
            NotificationModel notificationModel = new()
            {
                Id = Guid.NewGuid(),
                EventType = "AppointmentCancelled",
                Body = "Hi FirstName, OrganisationName, AppointmentDateTime, Reason.",
                Title = (EventTitle)"Appointment Cancelled"
            };

            Mock<INotificationsAccess> notificationsAccessMock = new(MockBehavior.Strict);
            notificationsAccessMock.Setup(x => x.GetAllNotifications())
                .Returns(Task.FromResult(Result.Success(new List<NotificationModel> { notificationModel })));

            INotificationsService notificationsService = new NotificationsService(notificationsAccessMock.Object);
            var notifications = await notificationsService.GetNotifications(null);

            Assert.True(notifications.IsSuccess);
            Assert.Single(notifications.Value);
            Assert.Equal(notificationModel, notifications.Value[0]);

            notificationsAccessMock.Verify(x => x.GetAllNotifications(), Times.Once);
        }

        [Fact]
        public async void GetNotificationsForAUser()
        {
            NotificationModel notificationModel = new()
            {
                Id = Guid.NewGuid(),
                EventType = "AppointmentCancelled",
                Body = "Hi FirstName, OrganisationName, AppointmentDateTime, Reason.",
                Title = (EventTitle)"Appointment Cancelled"
            };

            Mock<INotificationsAccess> notificationsAccessMock = new(MockBehavior.Strict);
            notificationsAccessMock.Setup(x => x.GetNotificationsForUser(5))
                .Returns(Task.FromResult(Result.Success(new List<NotificationModel> { notificationModel })));
            notificationsAccessMock.Setup(x => x.GetNotificationsForUser(4))
                .Returns(Task.FromResult(Result.Success(new List<NotificationModel>())));

            INotificationsService notificationsService = new NotificationsService(notificationsAccessMock.Object);

            // Verify notification is found
            var notifications = await notificationsService.GetNotifications(5);
            Assert.True(notifications.IsSuccess);
            Assert.Single(notifications.Value);
            Assert.Equal(notificationModel, notifications.Value[0]);

            notificationsAccessMock.Verify(x => x.GetNotificationsForUser(5), Times.Once);

            // Verify notification is not found
            var notifications2 = await notificationsService.GetNotifications(4);
            Assert.True(notifications2.IsSuccess);
            Assert.Empty(notifications2.Value);
            notificationsAccessMock.Verify(x => x.GetNotificationsForUser(4), Times.Once);
        }
    }
}
