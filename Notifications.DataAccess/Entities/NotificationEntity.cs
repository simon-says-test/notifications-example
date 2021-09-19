using System;
using Notifications.Common.Enums;
using Notifications.Common.Fields;

namespace Notifications.DataAccess.Entities
{
    public class NotificationEntity
    {
        private Guid _id;
        private EventType _eventType;
        private string _body;
        private string _title;
        private int _userId;

        protected NotificationEntity()
        {
        }

        public NotificationEntity(Guid id, EventType eventType, EventBody eventBody, EventTitle eventTitle, int userId)
        {
            _id = id;
            Update(eventType, eventBody, eventTitle, userId);
        }
        public void Update(EventType eventType, EventBody eventBody, EventTitle eventTitle, int userId)
        {
            _eventType = eventType;
            _body = eventBody;
            _title = eventTitle;
            _userId = userId;
        }

        public Guid Id => _id;

        public EventType EventType => _eventType;

        public EventBody Body => (EventBody)_body;

        public EventTitle Title => (EventTitle)_title;

        public int UserId => _userId;
    }
}
