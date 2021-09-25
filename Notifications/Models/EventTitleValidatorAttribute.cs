﻿using CSharpFunctionalExtensions;
using Notifications.Common.Fields;
using System;

namespace Notifications.Models
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class EventTitleValidatorAttribute : StringFieldValidatorAttribute
    {
        protected override Result Validate(string value)
        {
            return EventTitle.Validate(value);
        }
    }
}
