using System;
using System.Web.Compilation;
using System.Web.UI.WebControls;

namespace xVal.WebForms
{
    public abstract class ModelValidatorBase : BaseValidator
    {
        private Type _modelType;

        /// <summary>
        /// Initializes a new instance of the <see cref="ModelValidatorBase"/> class.
        /// </summary>
        protected ModelValidatorBase()
            : this(null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ModelValidatorBase"/> class.
        /// </summary>
        /// <param name="validationRunner">The validation runner.</param>
        protected ModelValidatorBase(IValidationRunner validationRunner)
        {
            ValidationRunner = validationRunner ?? new DataAnnotationsValidationRunner();
        }

        /// <summary>
        /// Gets the validation runner.
        /// </summary>
        protected IValidationRunner ValidationRunner { get; private set; }

        /// <summary>
        /// Gets or sets the type of the model.
        /// </summary>
        /// <value>
        /// The type of the model.
        /// </value>
        public string ModelType { get; set; }

        /// <summary>
        /// Determines whether the control specified by the <see cref="P:System.Web.UI.WebControls.BaseValidator.ControlToValidate"/> property is a valid control.
        /// </summary>
        /// <returns>
        /// true if the control specified by <see cref="P:System.Web.UI.WebControls.BaseValidator.ControlToValidate"/> is a valid control; otherwise, false.
        /// </returns>
        /// <exception cref="T:System.Web.HttpException">No value is specified for the <see cref="P:System.Web.UI.WebControls.BaseValidator.ControlToValidate"/> property.- or -The input control specified by the <see cref="P:System.Web.UI.WebControls.BaseValidator.ControlToValidate"/> property is not found on the page.- or -The input control specified by the <see cref="P:System.Web.UI.WebControls.BaseValidator.ControlToValidate"/> property does not have a <see cref="T:System.Web.UI.ValidationPropertyAttribute"/> attribute associated with it; therefore, it cannot be validated with a validation control.</exception>
        protected override bool ControlPropertiesValid()
        {
            return base.ControlPropertiesValid() && !String.IsNullOrEmpty(ModelType);
        }

        /// <summary>
        /// Gets the type of the model.
        /// </summary>
        /// <returns></returns>
        protected Type GetModelType()
        {
            if (_modelType == null)
            {
                _modelType = Type.GetType(ModelType);

                if (_modelType == null)
                {
                    // App_Code Types are created with System.Web.Compilation.BuildManager
                    _modelType = BuildManager.GetType(ModelType, false);
                }

                if (_modelType == null)
                {
                    throw new InvalidOperationException("Could not get a Type from " + ModelType);
                }
            }

            return _modelType;
        }

        /// <summary>
        /// Gets the model property value.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="valueControlId">The value control id.</param>
        /// <returns></returns>
        protected object GetModelPropertyValue(string propertyName, string valueControlId)
        {
            string stringValue = GetControlValidationValue(valueControlId);
            Type propertyType = GetModelType().GetProperty(propertyName).PropertyType;
            try
            {
                return Convert.ChangeType(stringValue, propertyType);
            }
            catch (FormatException)
            {
                return null;
            }
        }
    }
}