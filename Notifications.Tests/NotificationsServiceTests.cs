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

namespace Notifications.Tests
{
    public class NotificationsServiceTests
    {
        [Fact]
        public void CreateNotification()
        {
            TemplateModel templateModel = new TemplateModel
            {
                Id = Guid.NewGuid(),
                EventType = "AppointmentCancelled",
                Body = EventBody.Create("Hi {FirstName}, {OrganisationName}, {AppointmentDateTime}, {Reason}.").Value,
                Title = EventTitle.Create("Appointment Cancelled").Value
            };

            EventModel eventModel = new EventModel
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

            Mock<INotificationsAccess> notificationsAccessMock = new Mock<INotificationsAccess>(MockBehavior.Strict);
            notificationsAccessMock
                .Setup(x => x.GetTemplate(It.IsAny<EventModel>()))
                .Returns(Result.Success(templateModel));
            notificationsAccessMock
                .Setup(x => x.CreateNotification(It.IsAny<NotificationModel>()))
                .Returns(Result.Success);

            INotificationsService notificationsService = new NotificationsService(notificationsAccessMock.Object);
            Result<NotificationModel> notificationModel = notificationsService.CreateNotification(eventModel);

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
        public void GetNotificationsForAllUsers()
        {
            NotificationModel notificationModel = new NotificationModel
            {
                Id = Guid.NewGuid(),
                EventType = "AppointmentCancelled",
                Body = EventBody.Create("Hi FirstName, OrganisationName, AppointmentDateTime, Reason.").Value,
                Title = EventTitle.Create("Appointment Cancelled").Value
            };

            Mock<INotificationsAccess> notificationsAccessMock = new Mock<INotificationsAccess>(MockBehavior.Strict);
            notificationsAccessMock.Setup(x => x.GetAllNotifications()).Returns(Result.Success(new List<NotificationModel> { notificationModel }.AsQueryable()));

            INotificationsService notificationsService = new NotificationsService(notificationsAccessMock.Object);
            var notifications = notificationsService.GetNotifications(null);

            Assert.True(notifications.IsSuccess);
            Assert.Equal(1, notifications.Value.Count());
            Assert.Equal(notificationModel, notifications.Value.First());

            notificationsAccessMock.Verify(x => x.GetAllNotifications(), Times.Once);
        }

        [Fact]
        public void GetNotificationsForAUser()
        {
            NotificationModel notificationModel = new NotificationModel
            {
                Id = Guid.NewGuid(),
                EventType = "AppointmentCancelled",
                Body = EventBody.Create("Hi FirstName, OrganisationName, AppointmentDateTime, Reason.").Value,
                Title = EventTitle.Create("Appointment Cancelled").Value
            };

            Mock<INotificationsAccess> notificationsAccessMock = new Mock<INotificationsAccess>(MockBehavior.Strict);
            notificationsAccessMock.Setup(x => x.GetNotificationsForUser(5)).Returns(Result.Success(new List<NotificationModel> { notificationModel }.AsQueryable()));
            notificationsAccessMock.Setup(x => x.GetNotificationsForUser(4)).Returns(Result.Success(new List<NotificationModel>().AsQueryable()));

            INotificationsService notificationsService = new NotificationsService(notificationsAccessMock.Object);

            // Verify notification is found
            var notifications = notificationsService.GetNotifications(5);
            Assert.True(notifications.IsSuccess);
            Assert.Equal(1, notifications.Value.Count());
            Assert.Equal(notificationModel, notifications.Value.First());

            notificationsAccessMock.Verify(x => x.GetNotificationsForUser(5), Times.Once);

            // Verify notification is not found
            var notifications2 = notificationsService.GetNotifications(4);
            Assert.True(notifications2.IsSuccess);
            Assert.Equal(0, notifications2.Value.Count());
            notificationsAccessMock.Verify(x => x.GetNotificationsForUser(4), Times.Once);
        }
    }
}
