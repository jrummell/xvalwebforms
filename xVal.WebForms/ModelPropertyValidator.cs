using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.UI;

namespace xVal.WebForms
{
    public class ModelPropertyValidator : ModelValidatorBase
    {
        private IEnumerable<ValidationAttribute> _validationAttributes;

        public ModelPropertyValidator()
        {
        }

        public ModelPropertyValidator(IValidationRunner validationRunner)
            : base(validationRunner)
        {
        }

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
            IEnumerable<ValidationAttribute> attributes = GetValidationAttributes();
            if (!attributes.Any())
            {
                return true;
            }

            Type modelType = GetModelType();
            object value = GetModelPropertyValue(PropertyName, ControlToValidate);

            IEnumerable<ValidationResult> errors =
                ValidationRunner.Validate(modelType, value, PropertyName);

            if (errors.Any())
            {
                ErrorMessage = errors.GetErrorMessage();
                return false;
            }

            return true;
        }

        protected override void Render(HtmlTextWriter writer)
        {
            // only render the child validator controls
            RenderChildren(writer);
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            if (String.IsNullOrEmpty(PropertyName))
            {
                throw new InvalidOperationException("PropertyName must be set.");
            }

            // build rules dictionary
            ValidationRuleProvider ruleProvider = new ValidationRuleProvider();
            IDictionary<string, object> rules = ruleProvider.GetValidationRules(GetValidationAttributes());

            if (rules.Count > 0)
            {
                // register validation scripts
                ValidationScriptManager scriptManager =
                    new ValidationScriptManager(Page.ClientScript, ClientID, GetControlRenderID(ControlToValidate));
                scriptManager.RegisterValidationScripts(rules);
            }
        }

        private IEnumerable<ValidationAttribute> GetValidationAttributes()
        {
            if (_validationAttributes == null)
            {
                _validationAttributes =
                    ValidationRunner.GetValidators(GetModelType(), PropertyName);
            }

            return _validationAttributes;
        }

        
    }
}