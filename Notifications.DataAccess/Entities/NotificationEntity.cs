using System;
using Notifications.Common.Enums;
using Notifications.Common.Fields;

namespace Notifications.DataAccess.Entities
{
    public class NotificationEntity
    {
        protected NotificationEntity()
        {
        }

        public NotificationEntity(Guid id, EventType eventType, EventBody eventBody, EventTitle eventTitle, int userId)
        {
            Id = id;
            Update(eventType, eventBody, eventTitle, userId);
        }
        public void Update(EventType eventType, EventBody eventBody, EventTitle eventTitle, int userId)
        {
            EventType = eventType;
            Body = eventBody;
            Title = eventTitle;
            UserId = userId;
        }

        public Guid Id { get; private set; }

        public EventType EventType { get; private set; }

        public EventBody Body { get; private set; }

        public EventTitle Title { get; private set; }

        public int UserId { get; private set; }
    }
}
