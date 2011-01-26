using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using xVal.ServerSide;

namespace xVal.WebForms
{
    public static class DataAnnotationsValidationRunner
    {
        /// <summary>
        /// Gets the errors.
        /// </summary>
        /// <remarks>
        /// Warning: For some reason, DataTypeAttribute.IsValid() always returns "true", regardless of whether
        /// it is actually valid. Need to improve this test runner to fix that.
        /// </remarks>
        /// <param name="instance">The instance.</param>
        /// <returns></returns>
        public static IEnumerable<ErrorInfo> GetErrors(object instance)
        {
            return from prop in TypeDescriptor.GetProperties(instance).Cast<PropertyDescriptor>()
                   from attribute in prop.Attributes.OfType<ValidationAttribute>()
                   where !attribute.IsValid(prop.GetValue(instance))
                   select new ErrorInfo(prop.Name, attribute.FormatErrorMessage(string.Empty), instance);
        }

        /// <summary>
        /// Gets the validators.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="propertyName">The property name.</param>
        /// <returns></returns>
        public static IEnumerable<ValidationAttribute> GetValidators(Type type, string propertyName)
        {
            return from prop in TypeDescriptor.GetProperties(type).Cast<PropertyDescriptor>()
                   where prop.Name == propertyName
                   from attribute in prop.Attributes.OfType<ValidationAttribute>()
                   select attribute;
        }
    }
}