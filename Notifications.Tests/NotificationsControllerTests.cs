using Xunit;
using Notifications.Common.Models;
using System;
using Microsoft.AspNetCore.TestHost;
using Microsoft.AspNetCore.Hosting;
using System.Net.Http;
using System.Text.Json;
using Notifications.DataAccess;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Net;
using System.Text;
using Notifications.DataAccess.Entities;
using Microsoft.Extensions.Configuration;
using Notifications.Tests.Helpers;
using Notifications.Common.Enums;
using Notifications.Common.Fields;

namespace Notifications.Tests
{
    public class NotificationsControllerTests
    {
        private readonly TestServer server;
        private readonly HttpClient client;
        private readonly DbContextOptionsBuilder<NotificationsDbContext> builder;
        private readonly NotificationsDbContext context;

        public NotificationsControllerTests()
        {
            server = new TestServer(new WebHostBuilder().UseStartup<Startup>());
            client = server.CreateClient();
            builder = new DbContextOptionsBuilder<NotificationsDbContext>().UseSqlServer(Startup.Configuration().GetConnectionString("NotificationsContext"));
            context = new NotificationsDbContext(builder.Options);
        }

        [Fact]
        public async void When_Get_Notifications_Where_NoneExist_Expect_NoNotificationsReturned()
        {
            // Arrange
            context.Notifications.RemoveRange(context.Notifications);
            context.SaveChanges();

            // Act
            var response = await client.GetAsync("/api/Notifications").ConfigureAwait(false);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var result = await response.GetObjectFromContentString<NotificationModel[]>();
            Assert.Empty(result);
        }

        [Fact]
        public async void When_Get_NotificationsForUser_Where_NoneExistForUser_Expect_NoNotificationsReturned()
        {
            // Arrange
            NotificationEntity notificationEntity1 = new(Guid.NewGuid(),
                                                         EventType.AppointmentCancelled,
                                                         EventBody.Create("body1").Value,
                                                         EventTitle.Create("Title1").Value,
                                                         1);

            NotificationEntity notificationEntity2 = new(Guid.NewGuid(),
                                                         EventType.AppointmentCancelled,
                                                         EventBody.Create("body2").Value,
                                                         EventTitle.Create("Title2").Value,
                                                         1);

            NotificationEntity notificationEntity3 = new(Guid.NewGuid(),
                                                         EventType.AppointmentCancelled,
                                                         EventBody.Create("body3").Value,
                                                         EventTitle.Create("Title3").Value,
                                                         2);
            context.Notifications.RemoveRange(context.Notifications);

            context.Notifications.Add(notificationEntity1);
            context.Notifications.Add(notificationEntity2);
            context.Notifications.Add(notificationEntity3);
            context.SaveChanges();

            // Act
            var response = await client.GetAsync("/api/Notifications/?userId=5").ConfigureAwait(false);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var result = await response.GetObjectFromContentString<NotificationModel[]>();
            Assert.Empty(result);
        }

        [Fact]
        public async void When_Get_AllNotifications_Where_Exist_Expect_AllNotificationsReturned()
        {
            // Arrange
            NotificationEntity notificationEntity1 = new(Guid.NewGuid(),
                                                         EventType.AppointmentCancelled,
                                                         EventBody.Create("body1").Value,
                                                         EventTitle.Create("Title1").Value,
                                                         1);

            NotificationEntity notificationEntity2 = new(Guid.NewGuid(),
                                                         EventType.AppointmentCancelled,
                                                         EventBody.Create("body2").Value,
                                                         EventTitle.Create("Title2").Value,
                                                         1);

            NotificationEntity notificationEntity3 = new(Guid.NewGuid(),
                                                         EventType.AppointmentCancelled,
                                                         EventBody.Create("body3").Value,
                                                         EventTitle.Create("Title3").Value,
                                                         2);
            context.Notifications.RemoveRange(context.Notifications);

            context.Notifications.Add(notificationEntity1);
            context.Notifications.Add(notificationEntity2);
            context.Notifications.Add(notificationEntity3);
            context.SaveChanges();

            // Act
            var response = await client.GetAsync("/api/Notifications").ConfigureAwait(false);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var result = await response.GetObjectFromContentString<NotificationModel[]>();
            Assert.Equal(3, result.Length);
            NotificationModel notificationModel1 = result.Single(x => x.Id == notificationEntity1.Id);
            NotificationModel notificationModel2 = result.Single(x => x.Id == notificationEntity2.Id);
            NotificationModel notificationModel3 = result.Single(x => x.Id == notificationEntity3.Id);

            Assert.Equal(notificationEntity1.EventType.ToString(), notificationModel1.EventType);
            Assert.Equal(notificationEntity1.Body.Value, notificationModel1.Body);
            Assert.Equal(notificationEntity1.Title.Value, notificationModel1.Title);
            Assert.Equal(notificationEntity1.UserId, notificationModel1.UserId);

            Assert.Equal(notificationEntity2.EventType.ToString(), notificationModel2.EventType);
            Assert.Equal(notificationEntity2.Body.Value, notificationModel2.Body);
            Assert.Equal(notificationEntity2.Title.Value, notificationModel2.Title);
            Assert.Equal(notificationEntity2.UserId, notificationModel2.UserId);

            Assert.Equal(notificationEntity3.EventType.ToString(), notificationModel3.EventType);
            Assert.Equal(notificationEntity3.Body.Value, notificationModel3.Body);
            Assert.Equal(notificationEntity3.Title.Value, notificationModel3.Title);
            Assert.Equal(notificationEntity3.UserId, notificationModel3.UserId);
        }

        [Fact]
        public async void When_Get_NotificationsForUser_Where_Exist_Expect_NotificationsForUserReturned()
        {
            // Arrange
            NotificationEntity notificationEntity1 = new(Guid.NewGuid(),
                                                         EventType.AppointmentCancelled,
                                                         EventBody.Create("body1").Value,
                                                         EventTitle.Create("Title1").Value,
                                                         1);

            NotificationEntity notificationEntity2 = new(Guid.NewGuid(),
                                                         EventType.AppointmentCancelled,
                                                         EventBody.Create("body2").Value,
                                                         EventTitle.Create("Title2").Value,
                                                         1);

            NotificationEntity notificationEntity3 = new(Guid.NewGuid(),
                                                         EventType.AppointmentCancelled,
                                                         EventBody.Create("body3").Value,
                                                         EventTitle.Create("Title3").Value,
                                                         2);
            context.Notifications.RemoveRange(context.Notifications);

            context.Notifications.Add(notificationEntity1);
            context.Notifications.Add(notificationEntity2);
            context.Notifications.Add(notificationEntity3);
            context.SaveChanges();

            // Act
            var response = await client.GetAsync("/api/Notifications/?userId=1").ConfigureAwait(false);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var result = await response.GetObjectFromContentString<NotificationModel[]>();
            Assert.Equal(2, result.Length);
            NotificationModel notificationModel1 = result.Single(x => x.Id == notificationEntity1.Id);
            NotificationModel notificationModel2 = result.Single(x => x.Id == notificationEntity2.Id);

            Assert.Equal(notificationEntity1.EventType.ToString(), notificationModel1.EventType);
            Assert.Equal(notificationEntity1.Body.Value, notificationModel1.Body);
            Assert.Equal(notificationEntity1.Title.Value, notificationModel1.Title);
            Assert.Equal(notificationEntity1.UserId, notificationModel1.UserId);

            Assert.Equal(notificationEntity2.EventType.ToString(), notificationModel2.EventType);
            Assert.Equal(notificationEntity2.Body.Value, notificationModel2.Body);
            Assert.Equal(notificationEntity2.Title.Value, notificationModel2.Title);
            Assert.Equal(notificationEntity2.UserId, notificationModel2.UserId);
        }

        [Fact]
        public async void When_Post_Event_Expect_NotificationReturned()
        {
            // Arrange
            context.Notifications.RemoveRange(context.Notifications);
            context.SaveChanges();

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

            NotificationModel expectedResult = new()
            {
                UserId = 1,
                EventType = "AppointmentCancelled",
                Title = "Appointment Cancelled",
                Body = "Hi Bob, your appointment with Bob's builders at 10/02/2011 00:00 has been - cancelled for the following reason: Can't make it."
            };

            // Act
            var response = await client
                .PostAsync("/api/Notifications", new StringContent(JsonSerializer.Serialize(eventModel), Encoding.UTF8, "application/json"))
                .ConfigureAwait(false);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var result = await response.GetObjectFromContentString<NotificationModel>();
            Assert.NotEqual(Guid.Empty, result.Id);
            Assert.Equal(expectedResult, result with { Id = new Guid()});
        }

        [Fact]
        public async void When_Post_Event_Expect_NotificationSaved()
        {
            // Arrange
            context.Notifications.RemoveRange(context.Notifications);
            context.SaveChanges();

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

            // Act
            var response = await client
                .PostAsync("/api/Notifications", new StringContent(JsonSerializer.Serialize(eventModel), Encoding.UTF8, "application/json"))
                .ConfigureAwait(false);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            NotificationEntity result = context.Notifications.First();
            Assert.Equal(1, context.Notifications.Count());
            Assert.NotEqual(Guid.Empty, result.Id);
            Assert.Equal(EventType.AppointmentCancelled, result.EventType);
            Assert.Equal("Appointment Cancelled", result.Title);
            Assert.Equal(eventModel.UserId, result.UserId);
            Assert.Equal("Hi Bob, your appointment with Bob's builders at 10/02/2011 00:00 has been - cancelled for the following reason: Can't make it.", result.Body);
        }

        [Fact]
        public async void When_Get_IncorrectUrl_Expect_NotFoundReturned()
        {
            // Act
            var response = await client.GetAsync("/api/Notification").ConfigureAwait(false);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async void When_Get_IncorrectQueryParameter_Expect_BadRequestReturned()
        {
            // Act
            var response = await client.GetAsync("/api/Notifications/?usId=5").ConfigureAwait(false);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            var result = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            Assert.Equal("Bad request - check query parameters", result);
        }

        [Fact]
        public async void When_Post_InvalidEventType_Expect_BadRequestReturned()
        {
            // Arrange
            EventModel eventModel = new()
            {
                UserId = 1,
                Type = "AppointmentFinished",  // Invalid
                Data = new EventDataModel
                {
                    AppointmentDateTime = new DateTime(2011, 2, 10),
                    FirstName = "Bob",
                    Reason = "Can't make it",
                    OrganisationName = "Bob's builders"
                }
            };

            // Act
            var response = await client
                .PostAsync("/api/Notifications", new StringContent(JsonSerializer.Serialize(eventModel), Encoding.UTF8, "application/json"))
                .ConfigureAwait(false);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            var result = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            Assert.Equal("The requested EventType: (AppointmentFinished) was not found.", result);
        }

        [Fact]
        public async void When_Post_EventTypeMissingTemplate_Expect_BadRequestReturned()
        {
            // Arrange
            EventModel eventModel = new()
            {
                UserId = 1,
                Type = "AppointmentPostponed",  // Invalid
                Data = new EventDataModel
                {
                    AppointmentDateTime = new DateTime(2011, 2, 10),
                    FirstName = "Bob",
                    Reason = "Can't make it",
                    OrganisationName = "Bob's builders"
                }
            };

            // Act
            var response = await client
                .PostAsync("/api/Notifications", new StringContent(JsonSerializer.Serialize(eventModel), Encoding.UTF8, "application/json"))
                .ConfigureAwait(false);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);

            var result = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            Assert.Equal("The requested EventType: (AppointmentPostponed) was not found.", result);
        }
    }
}
