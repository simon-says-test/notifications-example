using CSharpFunctionalExtensions;
using System.Collections.Generic;

namespace Notifications.Common.Fields
{
    public sealed class EventBody : ValueObject
    {
        public string Value { get; }

        private EventBody(string value)
        {
            Value = value;
        }

        public static Result<EventBody> Create(Maybe<string> eventBodyOrNothing)
        {
            return eventBodyOrNothing
                .ToResult("Event body should not be empty")
                .Map(name => name.Trim())
                .Ensure(name => name != string.Empty, "Event body should not be empty")
                .Ensure(name => name.Length <= 255, "Event body is too long")
                .Map(name => new EventBody(name));
        }

        public static explicit operator EventBody(string eventBody)
        {
            return Create(eventBody).Value;
        }

        public static implicit operator string(EventBody eventBody)
        {
            return eventBody.Value;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}
