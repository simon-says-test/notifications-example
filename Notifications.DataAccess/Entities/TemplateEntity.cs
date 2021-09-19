using Notifications.Common.Enums;
using Notifications.Common.Fields;
using System;

namespace Notifications.DataAccess.Entities
{
    public class TemplateEntity
    {
        private Guid _id;
        private EventType _eventType;
        private string _body;
        private string _title;

        protected TemplateEntity()
        {
        }

        public TemplateEntity(Guid id, EventType eventType, EventBody eventBody, EventTitle eventTitle)
        {
            _id = id;
            Update(eventType, eventBody, eventTitle);
        }
        public void Update(EventType eventType, EventBody eventBody, EventTitle eventTitle)
        {
            _eventType = eventType;
            _body = eventBody;
            _title = eventTitle;
        }

        public Guid Id => _id;

        public EventType EventType => _eventType;

        public EventBody Body => (EventBody)_body;

        public EventTitle Title => (EventTitle)_title;
    }
}