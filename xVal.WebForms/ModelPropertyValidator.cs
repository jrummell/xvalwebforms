using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;
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
            IEnumerable<ValidationAttribute> attributes = GetValidationAttributes();

            IDictionary<string, object> rules = new Dictionary<string, object>();
            foreach (ValidationAttribute attribute in attributes)
            {
                IDictionary<string, object> attributeRules = GetValidationRules(attribute);
                foreach (KeyValuePair<string, object> rule in attributeRules)
                {
                    rules.Add(rule.Key, rule.Value);
                }
            }

            if (rules.Count > 0)
            {
                // register validation scripts
                string validateInitScript = String.Format("$('#{0}').validate();{1}", Page.Form.ClientID,
                                                          Environment.NewLine);
                Page.ClientScript.RegisterStartupScript(GetType(), "Validate", validateInitScript, true);

                StringBuilder validationOptionsScript = new StringBuilder();
                validationOptionsScript.AppendFormat("$('#{0}').rules('add', ", GetControlRenderID(ControlToValidate));
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                serializer.Serialize(rules, validationOptionsScript);
                validationOptionsScript.AppendLine(");");

                Page.ClientScript.RegisterStartupScript(GetType(), ClientID, validationOptionsScript.ToString(), true);
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

        private static IDictionary<string, object> GetValidationRules(ValidationAttribute attribute)
        {
            IDictionary<string, object> rules = new Dictionary<string, object>();

            if (attribute is RequiredAttribute)
            {
                rules.Add("required", true);
            }
            else if (attribute is RangeAttribute)
            {
                RangeAttribute rangeAttribute = (RangeAttribute) attribute;
                rules.Add("range", new[] {rangeAttribute.Minimum, rangeAttribute.Maximum});
            }
            else if (attribute is StringLengthAttribute)
            {
                StringLengthAttribute stringLengthAttribute = (StringLengthAttribute)attribute;
                rules.Add("rangelength", new[] { stringLengthAttribute.MinimumLength, stringLengthAttribute.MaximumLength });
            }
            else if (attribute is DataTypeAttribute)
            {
                DataTypeAttribute dataTypeAttribute = (DataTypeAttribute) attribute;
                switch (dataTypeAttribute.DataType)
                {
                    case DataType.Custom:
                        break;
                    case DataType.DateTime:
                        break;
                    case DataType.Date:
                        rules.Add("date", true);
                        break;
                    case DataType.Time:
                        break;
                    case DataType.Duration:
                        break;
                    case DataType.PhoneNumber:
                        rules.Add("phoneUS", true);
                        break;
                    case DataType.Currency:
                        break;
                    case DataType.Text:
                        break;
                    case DataType.Html:
                        break;
                    case DataType.MultilineText:
                        break;
                    case DataType.EmailAddress:
                        rules.Add("email", true);
                        break;
                    case DataType.Password:
                        break;
                    case DataType.Url:
                        rules.Add("url", true);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            return rules;
        }
    }
}