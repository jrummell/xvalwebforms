using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace xVal.WebForms
{
    public interface IValidationRunner
    {
        /// <summary>
        /// Validates the specified instance.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <returns></returns>
        IEnumerable<ValidationResult> Validate(object instance);

        /// <summary>
        /// Validates the specified model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="validateObject">if set to <c>true</c> and <paramref name="model"/> implements <see cref="IValidatableObject"/>, 
        /// calls <see cref="IValidatableObject.Validate"/> in addition to running all property validation attributes.</param>
        /// <returns></returns>
        IEnumerable<ValidationResult> Validate(object model, bool validateObject);

        /// <summary>
        /// Validates the specified property.
        /// </summary>
        /// <param name="propertyType">Type of the property.</param>
        /// <param name="propertyValue">The property value.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns></returns>
        IEnumerable<ValidationResult> Validate(Type propertyType, object propertyValue, string propertyName);

        /// <summary>
        /// Gets the validators.
        /// </summary>
        /// <param name="modelType">Type of the model.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <returns></returns>
        IEnumerable<ValidationAttribute> GetValidators(Type modelType, string propertyName);
    }
}