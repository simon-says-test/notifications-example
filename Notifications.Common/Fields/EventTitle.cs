using CSharpFunctionalExtensions;

namespace Notifications.Common.Fields
{
    public sealed class EventTitle : StringField
    {
        private EventTitle(string value) : base(value)
        {
        }

        public static Result<EventTitle> Create(Maybe<string> eventTitleOrNothing)
        {
            // Convert to record and clone
            return Validate(eventTitleOrNothing)
                .Map(name => new EventTitle(name));
        }

        public static Result<string> Validate(Maybe<string> stringValueOrNothing)
        {
            return stringValueOrNothing
                .ToResult("Value should not be empty")
                .Map(name => name.Trim())
                .Ensure(name => name != string.Empty, "Event body should not be empty")
                .Ensure(name => name.Length <= 255, "Event body is too long");
        }

        public static explicit operator EventTitle(string eventTitle)
        {
            return Create(eventTitle).Value;
        }

        public static implicit operator string(EventTitle eventTitle)
        {
            return eventTitle.Value;
        }
    }
}
