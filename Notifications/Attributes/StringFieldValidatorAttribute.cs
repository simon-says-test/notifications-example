using CSharpFunctionalExtensions;
using System;
using System.ComponentModel.DataAnnotations;

namespace Notifications.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public abstract class StringFieldValidatorAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            if (value?.GetType().IsAssignableTo(typeof(string)) == false)
            {
                throw new ArgumentException(
                    $"{GetType()} cannot be used to validate property {context.MemberName} " +
                    $"on {context.ObjectType} as the property type ({value.GetType()}) is not assignable to a string");
            }

            Result result = Validate((string)value);
            return result.IsSuccess ? ValidationResult.Success : new ValidationResult(result.Error);
        }

        protected abstract Result Validate(string value);
    }
}
