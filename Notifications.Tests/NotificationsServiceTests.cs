using Xunit;
using Moq;
using Notifications.Common.Interfaces;
using Notifications.Common.Models;
using Notifications.Services;
using System;
using System.Collections.Generic;
using System.Linq;

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
                Body = "Hi {FirstName}, {OrganisationName}, {AppointmentDateTime}, {Reason}.",
                Title = "Appointment Cancelled"
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
                .Returns(templateModel);
            notificationsAccessMock.Setup(x => x.CreateNotification(It.IsAny<NotificationModel>()));

            INotificationsService notificationsService = new NotificationsService(notificationsAccessMock.Object);
            NotificationModel notificationModel = notificationsService.CreateNotification(eventModel);

            Assert.Equal(templateModel.EventType, notificationModel.EventType);
            Assert.Equal(templateModel.Title, notificationModel.Title);
            Assert.Equal(eventModel.UserId, notificationModel.UserId);
            Assert.Equal("Hi Bob, Bob's builders, 10/02/2011 00:00, Can't make it.", notificationModel.Body);

            notificationsAccessMock.Verify(x => x.GetTemplate(It.IsAny<EventModel>()), Times.Once);
            notificationsAccessMock.Verify(x => x.GetTemplate(eventModel), Times.Once);
            notificationsAccessMock.Verify(x => x.CreateNotification(It.IsAny<NotificationModel>()), Times.Once);
            notificationsAccessMock.Verify(x => x.CreateNotification(notificationModel), Times.Once);
        }

        [Fact]
        public void GetNotificationsForAllUsers()
        {
            NotificationModel notificationModel = new NotificationModel
            {
                Id = Guid.NewGuid(),
                EventType = "AppointmentCancelled",
                Body = "Hi FirstName, OrganisationName, AppointmentDateTime, Reason.",
                Title = "Appointment Cancelled"
            };

            Mock<INotificationsAccess> notificationsAccessMock = new Mock<INotificationsAccess>(MockBehavior.Strict);
            notificationsAccessMock.Setup(x => x.GetAllNotifications()).Returns(new List<NotificationModel> { notificationModel });

            INotificationsService notificationsService = new NotificationsService(notificationsAccessMock.Object);
            var notifications = notificationsService.GetNotifications(null);

            Assert.Equal(1, notifications.Count);
            Assert.Equal(notificationModel, notifications.First());

            notificationsAccessMock.Verify(x => x.GetAllNotifications(), Times.Once);
        }

        [Fact]
        public void GetNotificationsForAUser()
        {
            NotificationModel notificationModel = new NotificationModel
            {
                Id = Guid.NewGuid(),
                EventType = "AppointmentCancelled",
                Body = "Hi FirstName, OrganisationName, AppointmentDateTime, Reason.",
                Title = "Appointment Cancelled"
            };

            Mock<INotificationsAccess> notificationsAccessMock = new Mock<INotificationsAccess>(MockBehavior.Strict);
            notificationsAccessMock.Setup(x => x.GetNotificationsForUser(5)).Returns(new List<NotificationModel> { notificationModel });
            notificationsAccessMock.Setup(x => x.GetNotificationsForUser(4)).Returns(new List<NotificationModel>());

            INotificationsService notificationsService = new NotificationsService(notificationsAccessMock.Object);

            // Verify notification is found
            var notifications = notificationsService.GetNotifications(5);
            Assert.Equal(1, notifications.Count);
            Assert.Equal(notificationModel, notifications.First());

            notificationsAccessMock.Verify(x => x.GetNotificationsForUser(5), Times.Once);

            // Verify notification is not found
            var notifications2 = notificationsService.GetNotifications(4);
            Assert.Equal(0, notifications2.Count);
            notificationsAccessMock.Verify(x => x.GetNotificationsForUser(4), Times.Once);
        }
    }
}
