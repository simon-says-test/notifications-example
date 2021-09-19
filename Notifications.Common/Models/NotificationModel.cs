using Notifications.Common.Fields;
using System;

namespace Notifications.Common.Models
{
    public class NotificationModel
    {
        public Guid Id { get; set; }

        public string EventType { get; set; }

        public EventBody Body { get; set; }

        public EventTitle Title { get; set; }

        public int UserId { get; set; }
    }
}
