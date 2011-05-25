using System;
using System.Web.Compilation;
using System.Web.UI.WebControls;

namespace xVal.WebForms
{
    public abstract class ModelValidatorBase : BaseValidator
    {
        private Type _modelType;

        protected ModelValidatorBase()
            : this(null)
        {
            
        }

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
        /// Gets the type of the model.
        /// </summary>
        /// <returns></returns>
        protected Type GetModelType()
        {
            if (_modelType == null)
            {
                if (String.IsNullOrEmpty(ModelType))
                {
                    throw new InvalidOperationException("ModelType must be set.");
                }

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
            return Convert.ChangeType(stringValue, propertyType);
        }
    }
}