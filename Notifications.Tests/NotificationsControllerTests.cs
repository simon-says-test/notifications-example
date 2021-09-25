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
            context.Notifications.RemoveRange(context.Notifications);
            context.SaveChanges();
        }

        [Fact]
        public async void HappyPath()
        {
            // Verify that there are zero notifications 
            var getResponse = await client.GetAsync("/api/Notifications").ConfigureAwait(false);
            var getContent = await getResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
            var getResult = JsonSerializer.Deserialize<NotificationModel[]>(getContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);
            Assert.Empty(getResult);

            // Verify that there are zero filtered notifications
            var getResponse2 = await client.GetAsync("/api/Notifications/?userId=5").ConfigureAwait(false);
            var getContent2 = await getResponse2.Content.ReadAsStringAsync().ConfigureAwait(false);
            var getResult2 = JsonSerializer.Deserialize<NotificationModel[]>(getContent2, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            Assert.Equal(HttpStatusCode.OK, getResponse2.StatusCode);
            Assert.Empty(getResult2);

            // Post an event and verify that the resulting notification is returned and saved
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
            var postResponse = await client
                .PostAsync("/api/Notifications", new StringContent(JsonSerializer.Serialize(eventModel), Encoding.UTF8, "application/json"))
                .ConfigureAwait(false);
            var postContent = await postResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
            var postResult = JsonSerializer.Deserialize<NotificationModel>(postContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            Assert.Equal(HttpStatusCode.OK, postResponse.StatusCode);
            Assert.Equal(eventModel.Type, postResult.EventType);
            Assert.Equal("Appointment Cancelled", postResult.Title);
            Assert.Equal(eventModel.UserId, postResult.UserId);
            Assert.Equal("Hi Bob, your appointment with Bob's builders at 10/02/2011 00:00 has been - cancelled for the following reason: Can't make it.", postResult.Body);

            NotificationEntity notification = context.Notifications.Find(postResult.Id);
            Assert.Equal(eventModel.Type, notification.EventType.ToString());
            Assert.Equal("Appointment Cancelled", notification.Title);
            Assert.Equal(eventModel.UserId, notification.UserId);
            Assert.Equal("Hi Bob, your appointment with Bob's builders at 10/02/2011 00:00 has been - cancelled for the following reason: Can't make it.", notification.Body);

            // Post a second event and verify that the resulting notification is returned and saved
            EventModel eventModel2 = new()
            {
                UserId = 2,
                Type = "AppointmentCancelled",
                Data = new EventDataModel
                {
                    AppointmentDateTime = new DateTime(2012, 2, 10),
                    FirstName = "Jim",
                    Reason = "Can't do it",
                    OrganisationName = "Jim's builders"
                }
            };
            var postResponse2 = await client
                .PostAsync("/api/Notifications", new StringContent(JsonSerializer.Serialize(eventModel2), Encoding.UTF8, "application/json"))
                .ConfigureAwait(false);
            var postContent2 = await postResponse2.Content.ReadAsStringAsync().ConfigureAwait(false);
            var postResult2 = JsonSerializer.Deserialize<NotificationModel>(postContent2, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            Assert.Equal(HttpStatusCode.OK, postResponse2.StatusCode);
            Assert.Equal(eventModel2.Type, postResult2.EventType);
            Assert.Equal("Appointment Cancelled", postResult2.Title);
            Assert.Equal(eventModel2.UserId, postResult2.UserId);
            Assert.Equal("Hi Jim, your appointment with Jim's builders at 10/02/2012 00:00 has been - cancelled for the following reason: Can't do it.", postResult2.Body);

            NotificationEntity notification2 = context.Notifications.Find(postResult2.Id);
            Assert.Equal(eventModel2.Type, notification2.EventType.ToString());
            Assert.Equal("Appointment Cancelled", notification2.Title);
            Assert.Equal(eventModel2.UserId, notification2.UserId);
            Assert.Equal("Hi Jim, your appointment with Jim's builders at 10/02/2012 00:00 has been - cancelled for the following reason: Can't do it.", notification2.Body);

            // Post a third event and verify that the resulting notification is returned and saved
            EventModel eventModel3 = new()
            {
                UserId = 2,
                Type = "AppointmentCancelled",
                Data = new EventDataModel
                {
                    AppointmentDateTime = new DateTime(2012, 3, 12, 5, 59, 58),
                    FirstName = "Doris",
                    Reason = "Not coming",
                    OrganisationName = "Doris's builders"
                }
            };
            var postResponse3 = await client
                .PostAsync("/api/Notifications", new StringContent(JsonSerializer.Serialize(eventModel3), Encoding.UTF8, "application/json"))
                .ConfigureAwait(false);
            var postContent3 = await postResponse3.Content.ReadAsStringAsync().ConfigureAwait(false);
            var postResult3 = JsonSerializer.Deserialize<NotificationModel>(postContent3, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            Assert.Equal(HttpStatusCode.OK, postResponse3.StatusCode);
            Assert.Equal(eventModel3.Type, postResult3.EventType);
            Assert.Equal("Appointment Cancelled", postResult3.Title);
            Assert.Equal(eventModel3.UserId, postResult3.UserId);
            Assert.Equal("Hi Doris, your appointment with Doris's builders at 12/03/2012 05:59 has been - cancelled for the following reason: Not coming.", postResult3.Body);

            NotificationEntity notification3 = context.Notifications.Find(postResult3.Id);
            Assert.Equal(eventModel3.Type, notification3.EventType.ToString());
            Assert.Equal("Appointment Cancelled", notification3.Title);
            Assert.Equal(eventModel3.UserId, notification3.UserId);
            Assert.Equal("Hi Doris, your appointment with Doris's builders at 12/03/2012 05:59 has been - cancelled for the following reason: Not coming.", notification3.Body);

            // Verify that there are three notifications returned from the API 
            var getResponse3 = await client.GetAsync("/api/Notifications").ConfigureAwait(false);
            var getContent3 = await getResponse3.Content.ReadAsStringAsync().ConfigureAwait(false);
            var getResult3 = JsonSerializer.Deserialize<NotificationModel[]>(getContent3, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            Assert.Equal(HttpStatusCode.OK, getResponse3.StatusCode);
            Assert.Equal(3, getResult3.Length);

            // Verify that there are two filtered notifications for userID = 2
            var getResponse4 = await client.GetAsync("/api/Notifications/?userId=2").ConfigureAwait(false);
            var getContent4 = await getResponse4.Content.ReadAsStringAsync().ConfigureAwait(false);
            var getResult4 = JsonSerializer.Deserialize<NotificationModel[]>(getContent4, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            Assert.Equal(HttpStatusCode.OK, getResponse4.StatusCode);
            Assert.Equal(2, getResult4.Length);

            var jimResult = getResult4.First(x => x.Body.Contains("Jim"));
            Assert.Equal("Appointment Cancelled", jimResult.Title);
            Assert.Equal(2, jimResult.UserId);
            Assert.Equal("Hi Jim, your appointment with Jim's builders at 10/02/2012 00:00 has been - cancelled for the following reason: Can't do it.", jimResult.Body);

            var dorisResult = getResult4.First(x => x.Body.Contains("Doris"));
            Assert.Equal("Appointment Cancelled", dorisResult.Title);
            Assert.Equal(2, dorisResult.UserId);
            Assert.Equal("Hi Doris, your appointment with Doris's builders at 12/03/2012 05:59 has been - cancelled for the following reason: Not coming.", dorisResult.Body);

            // Verify that there are zero filtered notifications for userId = 5
            var getResponse5 = await client.GetAsync("/api/Notifications/?userId=5").ConfigureAwait(false);
            var getContent5 = await getResponse5.Content.ReadAsStringAsync().ConfigureAwait(false);
            var getResult5 = JsonSerializer.Deserialize<NotificationModel[]>(getContent5, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            Assert.Equal(HttpStatusCode.OK, getResponse5.StatusCode);
            Assert.Empty(getResult5);
        }

        [Fact]
        public async void SadPath()
        {
            // Verify that incorrect URL is reported
            var getResponse = await client.GetAsync("/api/Notification").ConfigureAwait(false);
            Assert.Equal(HttpStatusCode.NotFound, getResponse.StatusCode);

            // Verify that incorrect query arameter name is reported
            var getResponse2 = await client.GetAsync("/api/Notifications/?usId=5").ConfigureAwait(false);
            Assert.Equal(HttpStatusCode.OK, getResponse2.StatusCode);

            // Post an event with incorrect Type and verify that it is reported correctly
            EventModel eventModel = new()
            {
                UserId = 1,
                Type = "AppointmentFinished",  // Incorrect
                Data = new EventDataModel
                {
                    AppointmentDateTime = new DateTime(2011, 2, 10),
                    FirstName = "Bob",
                    Reason = "Can't make it",
                    OrganisationName = "Bob's builders"
                }
            };
            var postResponse = await client
                .PostAsync("/api/Notifications", new StringContent(JsonSerializer.Serialize(eventModel), Encoding.UTF8, "application/json"))
                .ConfigureAwait(false);
            var postContent = await postResponse.Content.ReadAsStringAsync().ConfigureAwait(false);
            Assert.Equal(HttpStatusCode.BadRequest, postResponse.StatusCode);
            Assert.Equal("The requested EventType: (AppointmentFinished) was not found.", postContent);

            // Post a second event with type for which no template exists and verify that it is reported correctly
            EventModel eventModel2 = new()
            {
                UserId = 2,
                Type = "AppointmentPostponed",  // Valid but no template
                Data = new EventDataModel
                {
                    AppointmentDateTime = new DateTime(2012, 2, 10),
                    FirstName = "Jim",
                    Reason = "Can't do it",
                    OrganisationName = "Jim's builders"
                }
            };
            var postResponse2 = await client
                .PostAsync("/api/Notifications", new StringContent(JsonSerializer.Serialize(eventModel2), Encoding.UTF8, "application/json"))
                .ConfigureAwait(false);
            var postContent2 = await postResponse2.Content.ReadAsStringAsync().ConfigureAwait(false);
            Assert.Equal(HttpStatusCode.BadRequest, postResponse2.StatusCode);
            Assert.Equal("The requested EventType: (AppointmentPostponed) was not found.", postContent2);
        }
    }
}
