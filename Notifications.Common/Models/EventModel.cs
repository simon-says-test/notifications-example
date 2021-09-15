using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Text;

namespace Notifications.Common.Models
{
    public class EventModel
    {
        public string Type { get; set; }

        public EventDataModel Data { get; set; }

        public int UserId { get; set; }
    }
}
