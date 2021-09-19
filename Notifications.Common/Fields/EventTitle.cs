using CSharpFunctionalExtensions;
using System.Collections.Generic;

namespace Notifications.Common.Fields
{
    public sealed class EventTitle : ValueObject
    {
        public string Value { get; }

        private EventTitle(string value)
        {
            Value = value;
        }

        public static Result<EventTitle> Create(Maybe<string> eventTitleOrNothing)
        {
            return eventTitleOrNothing
                .ToResult("Event body should not be empty")
                .Map(name => name.Trim())
                .Ensure(name => name != string.Empty, "Event body should not be empty")
                .Ensure(name => name.Length <= 255, "Event body is too long")
                .Map(name => new EventTitle(name));
        }

        public static explicit operator EventTitle(string eventTitle)
        {
            return Create(eventTitle).Value;
        }

        public static implicit operator string(EventTitle eventTitle)
        {
            return eventTitle.Value;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}
