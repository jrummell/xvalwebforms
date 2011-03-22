using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Compilation;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace xVal.WebForms
{
    public class ModelPropertyValidator : BaseValidator
    {
        private readonly ValidatorControlFactory _validatorFactory = new ValidatorControlFactory();
        private Type _modelType;

        /// <summary>
        /// Gets or sets the type of the model.
        /// </summary>
        /// <value>
        /// The type of the model.
        /// </value>
        public string ModelType { get; set; }

        /// <summary>
        /// Gets or sets the name of the property.
        /// </summary>
        /// <value>
        /// The name of the property.
        /// </value>
        public string PropertyName { get; set; }

        /// <summary>
        /// When overridden in a derived class, this method contains the code to determine whether the value in the input control is valid.
        /// </summary>
        /// <returns>
        /// true if the value in the input control is valid; otherwise, false.
        /// </returns>
        protected override bool EvaluateIsValid()
        {
            EnsureChildControls();

            foreach (BaseValidator validator in Controls)
            {
                validator.Validate();

                if (!validator.IsValid)
                {
                    return false;
                }
            }

            return true;
        }

        protected override void Render(HtmlTextWriter writer)
        {
            // only render the child validator controls
            RenderChildren(writer);
        }

        /// <summary>
        /// Called by the ASP.NET page framework to notify server controls that use composition-based implementation to create any child controls they contain in preparation for posting back or rendering.
        /// </summary>
        protected override void CreateChildControls()
        {
            _modelType = GetModelType();

            if (String.IsNullOrEmpty(PropertyName))
            {
                throw new InvalidOperationException("PropertyName must be set.");
            }

            IEnumerable<ValidationAttribute> attributes = DataAnnotationsValidationRunner.GetValidators(_modelType, PropertyName);
            foreach (ValidationAttribute attribute in attributes)
            {
                BaseValidator validator = _validatorFactory.CreateValidator(attribute);

                if (validator != null)
                {
                    PrepareValidator(validator, attribute);

                    Controls.Add(validator);
                }
            }
        }

        private void PrepareValidator(BaseValidator validator, ValidationAttribute attribute)
        {
            validator.ID = ID + "_modelValidator" + (Controls.Count + 1);
            validator.ControlToValidate = ControlToValidate;
            validator.Display = Display;
            validator.ValidationGroup = ValidationGroup;
            validator.CssClass = CssClass;

            // try the ErrorMessage property first, then attribute error message
            string errorMessage = null;
            if (!String.IsNullOrEmpty(ErrorMessage))
            {
                errorMessage = ErrorMessage;
            }
            else if (attribute != null)
            {
                errorMessage = attribute.ErrorMessage;
            }

            validator.ErrorMessage = errorMessage;
            validator.Text = !String.IsNullOrEmpty(Text) ? Text : errorMessage;
        }

        /// <summary>
        /// Gets the type of the model.
        /// </summary>
        /// <returns></returns>
        private Type GetModelType()
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
    }
}