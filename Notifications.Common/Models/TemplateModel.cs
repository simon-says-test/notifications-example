using System;

namespace Notifications.Common.Models
{
    public class TemplateModel
    {
        public Guid Id { get; set; }

        public string EventType { get; set; }

        public string Body { get; set; }

        public string Title { get; set; }
    }
}
