using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Web.Compilation;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace xVal.WebForms
{
    public class ModelPropertyValidator : BaseValidator
    {
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

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            _modelType = GetModelType();

            if (String.IsNullOrEmpty(PropertyName))
            {
                throw new InvalidOperationException("PropertyName must be set.");
            }

            IEnumerable<ValidationAttribute> attributes = DataAnnotationsValidationRunner.GetValidators(_modelType,
                                                                                                        PropertyName);
            IDictionary<string, object> rules = new Dictionary<string, object>();
            foreach (ValidationAttribute attribute in attributes)
            {
                WebControl controlToValidate = Parent.FindControl(ControlToValidate) as WebControl;
                if (controlToValidate != null)
                {
                    IDictionary<string, object> attributeRules = GetValidationRules(attribute);
                    foreach (KeyValuePair<string, object> rule in attributeRules)
                    {
                        rules.Add(rule.Key, rule.Value);
                    }
                }
            }

            if (rules.Count > 0)
            {
                string validateInitScript = String.Format("$('#{0}').validate();{1}", Page.Form.ClientID, Environment.NewLine);
                Page.ClientScript.RegisterStartupScript(GetType(), "Validate", validateInitScript, true);

                StringBuilder validationOptionsScript = new StringBuilder();
                validationOptionsScript.AppendFormat("$('#{0}').rules('add', ", GetControlRenderID(ControlToValidate));
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                serializer.Serialize(rules, validationOptionsScript);
                validationOptionsScript.AppendLine(");");

                Page.ClientScript.RegisterStartupScript(GetType(), ClientID, validationOptionsScript.ToString(), true);
            }
        }

        private IDictionary<string, object> GetValidationRules(ValidationAttribute attribute)
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