namespace Notifications.Models
{
    public class Notification
    {
        public string EventType { get; set; }

        public string Body { get; set; }

        public string Title { get; set; }

        public int UserId { get; set; }
    }
}
