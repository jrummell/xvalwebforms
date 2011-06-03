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
        private readonly ClientScriptManager _scriptManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationScriptManager"/> class.
        /// </summary>
        /// <param name="scriptManager">The script manager.</param>
        /// <param name="controlToValidateId">The control to validate id.</param>
        public ValidationScriptManager(ClientScriptManager scriptManager, string controlToValidateId)
        {
            _scriptManager = scriptManager;
            _controlToValidateId = controlToValidateId;
        }

        #region IValidationScriptManager Members

        /// <summary>
        /// Registers the scripts.
        /// </summary>
        /// <param name="rules">The rules.</param>
        public void RegisterScripts(RuleCollection rules)
        {
            const string validateInitScript = "$('form').validate();";
            _scriptManager.RegisterStartupScript(_registerType, "Validate", validateInitScript, true);

            StringBuilder validationOptionsScript = new StringBuilder();
            validationOptionsScript.AppendFormat("$('#{0}').rules('add', ", _controlToValidateId);

            JavaScriptSerializer serializer = new JavaScriptSerializer();
            serializer.RegisterConverters(new[] {new RulesJavaScriptConverter()});
            serializer.Serialize(rules, validationOptionsScript);

            validationOptionsScript.AppendLine(");");

            _scriptManager.RegisterStartupScript(
                _registerType, _controlToValidateId, validationOptionsScript.ToString(), true);
        }

        #endregion
    }
}