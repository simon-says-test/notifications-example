using Notifications.Models;
using Xunit;
using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;

namespace Notifications.Tests
{
    public class EventTitleValidatorAttributeTests
    {
        [Fact]
        public void ValidationSuccess()
        {
            const string input = "Fine value";
            var attribute = new EventTitleValidatorAttribute();
            var result = attribute.GetValidationResult(input, new ValidationContext(input));

            Assert.True(result == ValidationResult.Success);
        }

        [Fact]
        public void NullProperty_ValidationFailure()
        {
            ObjectToValidate input = new() { EventTitle1 = null, EventTitle2 = "Great event title" };
            var validationResults = new List<ValidationResult>();
            var ctx = new ValidationContext(input, null, null);

            var result = Validator.TryValidateObject(input, ctx, validationResults, true);

            Assert.False(result);
            Assert.Equal("Value should not be empty", validationResults[0].ErrorMessage);
        }

        [Fact]
        public void ValidProperties_ValidationSuccess()
        {
            ObjectToValidate input = new() { EventTitle1 = "Fine event title", EventTitle2 = "Great event title" };
            var validationResults = new List<ValidationResult>();
            var ctx = new ValidationContext(input, null, null);

            var result = Validator.TryValidateObject(input, ctx, validationResults, true);

            Assert.True(result);
        }

        [Fact]
        public void EmptyProperty_ValidationFailure()
        {
            ObjectToValidate input = new() { EventTitle1 = string.Empty, EventTitle2 = "Great event title" };
            var validationResults = new List<ValidationResult>();
            var ctx = new ValidationContext(input, null, null);

            var result = Validator.TryValidateObject(input, ctx, validationResults, true);

            Assert.False(result);
            Assert.Equal("Event body should not be empty", validationResults[0].ErrorMessage);
        }

        [Fact]
        public void MultipleInvalidProperties_ValidationFailure()
        {
            ObjectToValidate input = new() { EventTitle1 = string.Empty, EventTitle2 = null };
            var validationResults = new List<ValidationResult>();
            var ctx = new ValidationContext(input, null, null);

            var result = Validator.TryValidateObject(input, ctx, validationResults, true);

            Assert.False(result);
            Assert.Equal(2, validationResults.Count);
            Assert.Equal("Event body should not be empty", validationResults[0].ErrorMessage);
            Assert.Equal("Value should not be empty", validationResults[1].ErrorMessage);
        }

        [Fact]
        public void AttributeAppliedToWrongType_ThrowsException()
        {
            InvalidObjectToValidate input = new() { Integer = 5, EventTitle1 = "Great" };
            var validationResults = new List<ValidationResult>();
            var ctx = new ValidationContext(input, null, null);

            void action() => Validator.TryValidateObject(input, ctx, validationResults, true);

            ArgumentException exception = Assert.Throws<ArgumentException>(action);
            Assert.Equal("Notifications.Models.EventTitleValidatorAttribute cannot be used to validate property " +
                "Integer on Notifications.Tests.InvalidObjectToValidate as the property type (System.Int32) is not assignable to a string"
                , exception.Message);
        }
    }

    public class ObjectToValidate
    {
        [EventTitleValidator]
        public string EventTitle1 { get; set; }

        [EventTitleValidator]
        public string EventTitle2 { get; set; }
    }

    public class InvalidObjectToValidate
    {
        [EventTitleValidator]
        public int Integer { get; set; }

        [EventTitleValidator]
        public string EventTitle1 { get; set; }

        [EventTitleValidator]
        public string EventTitle2 { get; set; }
    }
}
