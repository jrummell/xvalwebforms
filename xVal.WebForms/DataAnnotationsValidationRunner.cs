using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace xVal.WebForms
{
    public class DataAnnotationsValidationRunner : IValidationRunner
    {
        public IEnumerable<ValidationResult> Validate(object instance)
        {
            IEnumerable<ValidationResult> results =
                from prop in TypeDescriptor.GetProperties(instance).Cast<PropertyDescriptor>()
                from attribute in prop.Attributes.OfType<ValidationAttribute>()
                where !attribute.IsValid(prop.GetValue(instance))
                select new ValidationResult(attribute.FormatErrorMessage(String.Empty), new[] { prop.Name });

            List<ValidationResult> resultList = new List<ValidationResult>(results);

            IValidatableObject validatableObject = instance as IValidatableObject;
            if (validatableObject != null)
            {
                IEnumerable<ValidationResult> objectResults =
                    validatableObject.Validate(new ValidationContext(instance, null, null));

                if (objectResults.Any())
                {
                    resultList.AddRange(objectResults);
                }
            }

            return resultList;
        }

        public IEnumerable<ValidationResult> Validate(Type modelType, object propertyValue, string propertyName)
        {
            return from prop in TypeDescriptor.GetProperties(modelType).Cast<PropertyDescriptor>()
                   where prop.Name == propertyName
                   from attribute in prop.Attributes.OfType<ValidationAttribute>()
                   where !attribute.IsValid(propertyValue)
                   select new ValidationResult(attribute.FormatErrorMessage(String.Empty), new[] { prop.Name });
        }

        public IEnumerable<ValidationAttribute> GetValidators(Type type, string propertyName)
        {
            return from prop in TypeDescriptor.GetProperties(type).Cast<PropertyDescriptor>()
                   where prop.Name == propertyName
                   from attribute in prop.Attributes.OfType<ValidationAttribute>()
                   select attribute;
        }
    }
}