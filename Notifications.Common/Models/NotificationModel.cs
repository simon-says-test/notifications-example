using System;

namespace Notifications.Common.Models
{
    public record NotificationModel
    {
        public Guid Id { get; set; }

        public string EventType { get; set; }

        public string Body { get; set; }

        public string Title { get; set; }

        public int UserId { get; set; }
    }
}
