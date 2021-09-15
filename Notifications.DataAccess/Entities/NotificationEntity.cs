using System;
using Notifications.Common.Enums;

namespace Notifications.DataAccess.Entities
{
    public class NotificationEntity
    {
        public Guid Id { get; set; }

        public EventType EventType { get; set; }

        public string Body { get; set; }

        public string Title { get; set; }

        public int UserId { get; set; }
    }
}
