using CSharpFunctionalExtensions;
using System;

namespace Notifications.Common.Fields
{
    public static class Enum<T> where T : System.Enum
    {
        public static Result<T> Create(string enumOrNothing)
        {
                return Enum.TryParse(typeof(T), enumOrNothing, true, out object enumValue)
                ? Result.Success((T)enumValue)
                : Result.Failure<T>($"The requested {typeof(T).Name}: ({enumOrNothing}) was not found.");
        }
    }
}
