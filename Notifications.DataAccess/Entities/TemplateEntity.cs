using Notifications.Common.Enums;
using Notifications.Common.Fields;
using System;

namespace Notifications.DataAccess.Entities
{
    public class TemplateEntity
    {
        protected TemplateEntity()
        {
        }

        public TemplateEntity(Guid id, EventType eventType, EventBody eventBody, EventTitle eventTitle)
        {
            Id = id;
            Update(eventType, eventBody, eventTitle);
        }
        public void Update(EventType eventType, EventBody eventBody, EventTitle eventTitle)
        {
            EventType = eventType;
            Body = eventBody;
            Title = eventTitle;
        }

        public Guid Id { get; private set; }

        public EventType EventType { get; private set; }

        public EventBody Body { get; private set; }

        public EventTitle Title { get; private set; }
    }
}