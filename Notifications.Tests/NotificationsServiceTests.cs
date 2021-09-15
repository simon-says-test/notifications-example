using Xunit;
using Moq;
using Notifications.Common.Interfaces;
using Notifications.Common.Models;
using Notifications.Services;
using System;
using Notifications.Common.Enums;

namespace Notifications.Tests
{
    public class NotificationsServiceTests
    {
        public NotificationsServiceTests()
        {
        }

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

            Assert.True(true);
        }
    }
}
