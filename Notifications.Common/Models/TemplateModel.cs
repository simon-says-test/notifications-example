using System;
using Notifications.Common.Fields;

namespace Notifications.Common.Models
{
    public class TemplateModel
    {
        public Guid Id { get; set; }

        public string EventType { get; set; }

        public EventBody Body { get; set; }

        public EventTitle Title { get; set; }
    }
}
