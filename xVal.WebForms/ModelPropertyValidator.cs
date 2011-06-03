using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.UI;

namespace xVal.WebForms
{
    public class ModelPropertyValidator : ModelValidatorBase
    {
        private IValidationRuleProvider _ruleProvider;
        private IValidationScriptManager _scriptManager;
        private IEnumerable<ValidationAttribute> _validationAttributes;

        /// <summary>
        /// Initializes a new instance of the <see cref="ModelPropertyValidator"/> class.
        /// </summary>
        public ModelPropertyValidator()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ModelPropertyValidator"/> class.
        /// </summary>
        /// <param name="validationRunner">The validation runner.</param>
        /// <param name="ruleProvider">The rule provider.</param>
        /// <param name="scriptManager">The script manager.</param>
        public ModelPropertyValidator(IValidationRunner validationRunner, IValidationRuleProvider ruleProvider,
                                      IValidationScriptManager scriptManager)
            : base(validationRunner)
        {
            _ruleProvider = ruleProvider;
            _scriptManager = scriptManager;
        }

        /// <summary>
        /// Gets or sets the name of the property.
        /// </summary>
        /// <value>
        /// The name of the property.
        /// </value>
        public string PropertyName { get; set; }

        /// <summary>
        /// Determines whether the control specified by the <see cref="P:System.Web.UI.WebControls.BaseValidator.ControlToValidate"/> property is a valid control.
        /// </summary>
        /// <returns>
        /// true if the control specified by <see cref="P:System.Web.UI.WebControls.BaseValidator.ControlToValidate"/> is a valid control; otherwise, false.
        /// </returns>
        /// <exception cref="T:System.Web.HttpException">No value is specified for the <see cref="P:System.Web.UI.WebControls.BaseValidator.ControlToValidate"/> property.- or -The input control specified by the <see cref="P:System.Web.UI.WebControls.BaseValidator.ControlToValidate"/> property is not found on the page.- or -The input control specified by the <see cref="P:System.Web.UI.WebControls.BaseValidator.ControlToValidate"/> property does not have a <see cref="T:System.Web.UI.ValidationPropertyAttribute"/> attribute associated with it; therefore, it cannot be validated with a validation control.</exception>
        protected override bool ControlPropertiesValid()
        {
            return base.ControlPropertiesValid() && !String.IsNullOrEmpty(PropertyName);
        }

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

            RegisterScripts();
        }

        public void RegisterScripts()
        {
            // get the attriubtes
            IEnumerable<ValidationAttribute> attributes = GetValidationAttributes();
            if (!attributes.Any())
            {
                return;
            }

            // get the rules
            if (_ruleProvider == null)
            {
                _ruleProvider = new ValidationRuleProvider();
            }

            RuleCollection rules = _ruleProvider.GetRules(attributes);
            if (!rules.Any())
            {
                return;
            }

            // register validation scripts
            if (_scriptManager == null)
            {
                _scriptManager = new ValidationScriptManager(Page.ClientScript, GetControlRenderID(ControlToValidate));
            }

            _scriptManager.RegisterScripts(rules);
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