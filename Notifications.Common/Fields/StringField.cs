using CSharpFunctionalExtensions;
using System.Collections.Generic;

namespace Notifications.Common.Fields
{
    public abstract class StringField : ValueObject
    {
        public string Value { get; }

        protected StringField(string value)
        {
            this.Value = value;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}
