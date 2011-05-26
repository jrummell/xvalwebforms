using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Script.Serialization;
using System.Web.UI;

namespace xVal.WebForms
{
    public class ValidationScriptManager
    {
        private readonly string _controlToValidateId;
        private readonly Type _registerType = typeof (ModelPropertyValidator);
        private readonly ClientScriptManager _scriptManager;
        private readonly string _validationControlId;

        public ValidationScriptManager(ClientScriptManager scriptManager, string validationControlId,
                                       string controlToValidateId)
        {
            _scriptManager = scriptManager;
            _validationControlId = validationControlId;
            _controlToValidateId = controlToValidateId;
        }

        public void RegisterValidationScripts(IDictionary<string, object> rules)
        {
            const string validateInitScript = "$('form').validate();";
            _scriptManager.RegisterStartupScript(_registerType, "Validate", validateInitScript, true);

            StringBuilder validationOptionsScript = new StringBuilder();
            validationOptionsScript.AppendFormat("$('#{0}').rules('add', ", _controlToValidateId);
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            serializer.Serialize(rules, validationOptionsScript);
            validationOptionsScript.AppendLine(");");

            _scriptManager.RegisterStartupScript(_registerType, _validationControlId, validationOptionsScript.ToString(),
                                                 true);
        }
    }
}