using System;
using System.Text;
using System.Web.Script.Serialization;
using System.Web.UI;

namespace xVal.WebForms
{
    public class ValidationScriptManager : IValidationScriptManager
    {
        private readonly string _controlToValidateId;
        private readonly Type _registerType = typeof (ModelPropertyValidator);
        private readonly Page _page;

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationScriptManager"/> class.
        /// </summary>
        /// <param name="page">The page.</param>
        /// <param name="controlToValidateId">The control to validate id.</param>
        public ValidationScriptManager(Page page, string controlToValidateId)
        {
            _page = page;
            _controlToValidateId = controlToValidateId;
        }

        #region IValidationScriptManager Members

        /// <summary>
        /// Registers the scripts.
        /// </summary>
        /// <param name="rules">The rules.</param>
        public void RegisterScripts(RuleCollection rules)
        {
            ClientScriptManager scriptManager = _page.ClientScript;

            string initScriptResourceUrl = scriptManager.GetWebResourceUrl(_registerType,ModelPropertyValidator.WebformValidateResourceName);
            scriptManager.RegisterClientScriptInclude(_registerType, "WebformValidate", initScriptResourceUrl);
            
            string validateInitScript = String.Format("$(document).ready(function(){{ $webformValidate.init('#{0}'); }});{1}", _page.Form.ClientID, Environment.NewLine);
            scriptManager.RegisterStartupScript(_registerType, "WebformValidateInit", validateInitScript, true);
            
            StringBuilder validationOptionsScript = new StringBuilder();
            validationOptionsScript.AppendFormat("$(document).ready(function(){{ $('#{0}').rules('add', ", _controlToValidateId);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            serializer.RegisterConverters(new[] {new RulesJavaScriptConverter()});
            serializer.Serialize(rules, validationOptionsScript);

            validationOptionsScript.AppendLine("); });");

            scriptManager.RegisterStartupScript(
                _registerType, _controlToValidateId, validationOptionsScript.ToString(), true);
        }

        #endregion
    }
}