using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Web.UI;
using xVal.WebForms;

[assembly: WebResource(ModelPropertyValidator.WebformValidateResourceName, "text/javascript")]

namespace xVal.WebForms
{
    public class ModelPropertyValidator : ModelValidatorBase
    {
        internal const string WebformValidateResourceName = "xVal.WebForms.webformValidate.js";
        private readonly IValidationRunner _validationRunner;

        private IValidationRuleProvider _ruleProvider;
        private IValidationScriptManager _scriptManager;
        private IEnumerable<ValidationAttribute> _validationAttributes;

        /// <summary>
        /// Initializes a new instance of the <see cref="ModelPropertyValidator"/> class.
        /// </summary>
        public ModelPropertyValidator()
            : this(null, null, null)
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
        {
            _validationRunner = validationRunner ?? new DataAnnotationsValidationRunner();
            _ruleProvider = ruleProvider;
            _scriptManager = scriptManager;
        }

        /// <summary>
        /// Gets or sets the name of the property.
        /// </summary>
        /// <value>
        /// The name of the property.
        /// </value>
        public string PropertyName
        {
            get { return (string) ViewState["PropertyName"] ?? String.Empty; }
            set { ViewState["PropertyName"] = value; }
        }

        /// <summary>
        /// Determines whether the control specified by the <see cref="P:System.Web.UI.WebControls.BaseValidator.ControlToValidate"/> property is a valid control.
        /// </summary>
        /// <returns>
        /// true if the control specified by <see cref="P:System.Web.UI.WebControls.BaseValidator.ControlToValidate"/> is a valid control; otherwise, false.
        /// </returns>
        /// <exception cref="T:System.Web.HttpException">No value is specified for the <see cref="P:System.Web.UI.WebControls.BaseValidator.ControlToValidate"/> property.- or -The input control specified by the <see cref="P:System.Web.UI.WebControls.BaseValidator.ControlToValidate"/> property is not found on the page.- or -The input control specified by the <see cref="P:System.Web.UI.WebControls.BaseValidator.ControlToValidate"/> property does not have a <see cref="T:System.Web.UI.ValidationPropertyAttribute"/> attribute associated with it; therefore, it cannot be validated with a validation control.</exception>
        public override bool ControlPropertiesValid()
        {
            base.ControlPropertiesValid();

            if (String.IsNullOrEmpty(ControlToValidate))
            {
                throw new InvalidOperationException("ControlToValidate is required.");
            }

            if (String.IsNullOrEmpty(PropertyName))
            {
                throw new InvalidOperationException("PropertyName is required.");
            }

            Type modelType = GetModelType();
            PropertyInfo property = modelType.GetProperty(PropertyName);

            if (property == null)
            {
                string message =
                    String.Format("Could not find PropertyName:{0} on ModelType: {1}", PropertyName, ModelType);
                throw new InvalidOperationException(message);
            }

            return true;
        }

        /// <summary>
        /// When overridden in a derived class, this method contains the code to determine whether the value in the input control is valid.
        /// </summary>
        /// <returns>
        /// true if the value in the input control is valid; otherwise, false.
        /// </returns>
        protected override bool EvaluateIsValid()
        {
            Type modelType = GetModelType();
            object value;
            try
            {
                value = GetModelPropertyValue(PropertyName, ControlToValidate);
            }
            catch(ValidationException ex)
            {
                ErrorMessage = ex.ValidationResult.ErrorMessage;
                return false;
            }

            IEnumerable<ValidationResult> errors =
                _validationRunner.Validate(modelType, value, PropertyName);

            if (errors.Any())
            {
                ErrorMessage = errors.GetErrorMessage();
                return false;
            }

            return true;
        }

        /// <summary>
        /// Renders the control to the specified HTML writer.
        /// </summary>
        /// <param name="writer">The <see cref="T:System.Web.UI.HtmlTextWriter"/> object that receives the control content.</param>
        protected override void Render(HtmlTextWriter writer)
        {
            // only render the child validator controls
            RenderChildren(writer);
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.PreRender"/> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> object that contains the event data.</param>
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            if (EnableClientScript)
            {
                RegisterScripts();
            }
        }

        /// <summary>
        /// Registers the scripts.
        /// </summary>
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
                _scriptManager = new ValidationScriptManager(Page, GetControlRenderId(ControlToValidate));
            }

            _scriptManager.RegisterScripts(rules);
        }

        /// <summary>
        /// Gets the validation attributes.
        /// </summary>
        /// <returns></returns>
        private IEnumerable<ValidationAttribute> GetValidationAttributes()
        {
            if (_validationAttributes == null)
            {
                _validationAttributes =
                    _validationRunner.GetValidators(GetModelType(), PropertyName);
            }

            return _validationAttributes;
        }
    }
}