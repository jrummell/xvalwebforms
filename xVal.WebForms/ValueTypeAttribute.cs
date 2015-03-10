using System;
using System.ComponentModel.DataAnnotations;

namespace xVal.WebForms
{
    internal class ValueTypeAttribute : ValidationAttribute
    {
        private readonly Type _valueType;

        /// <summary>
        /// Initializes a new instance of the <see cref="ValueTypeAttribute"/> class.
        /// </summary>
        /// <param name="valueType">Type of the value.</param>
        public ValueTypeAttribute(Type valueType)
        {
            if (valueType == null)
            {
                throw new ArgumentNullException("valueType");
            }

            _valueType = valueType;
        }

        /// <summary>
        /// Validates the specified value with respect to the current validation attribute.
        /// </summary>
        /// <param name="value">The value to validate.</param>
        /// <param name="validationContext">The context information about the validation operation.</param>
        /// <returns>
        /// An instance of the <see cref="T:System.ComponentModel.DataAnnotations.ValidationResult"/> class.
        /// </returns>
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value.GetType() == _valueType)
            {
                return ValidationResult.Success;
            }

            try
            {
                Convert.ChangeType(value, _valueType);
            }
            catch (InvalidCastException)
            {
                return new ValidationResult(FormatErrorMessage(validationContext.DisplayName),
                                            new[] {validationContext.MemberName});
            }

            return ValidationResult.Success;
        }

        /// <summary>
        /// Applies formatting to an error message, based on the data field where the error occurred.
        /// </summary>
        /// <param name="name">The name to include in the formatted message.</param>
        /// <returns>
        /// An instance of the formatted error message.
        /// </returns>
        public override string FormatErrorMessage(string name)
        {
            if (String.IsNullOrEmpty(ErrorMessage) && String.IsNullOrEmpty(ErrorMessageResourceName))
            {
                return String.Format("{0} must be an {1}.", name, _valueType.Name);
            }

            return base.FormatErrorMessage(name);
        }
    }
}