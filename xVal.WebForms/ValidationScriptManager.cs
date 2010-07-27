using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Script.Serialization;
using System.Web.UI;
using System.Web.UI.WebControls;
using xVal.RuleProviders;
using xVal.WebForms.Html;

namespace xVal.WebForms
{
    public interface IValidationScriptManager
    {
        /// <summary>
        /// Supresses validation for the given collection of <see cref="IButtonControl"/>s.
        /// </summary>
        /// <param name="buttons">The buttons.</param>
        void SupressValidation(IEnumerable<IButtonControl> buttons);

        /// <summary>
        /// Registers the validation script.
        /// </summary>
        /// <param name="options">The options.</param>
        /// <param name="controlMapping">The control mapping.</param>
        void RegisterValidationScript(IDictionary<string, object> options, IDictionary<string, string> controlMapping);
    }

    public class ValidationScriptManager : IValidationScriptManager
    {
        private readonly string _clientId;
        private readonly Type _modelType;
        private readonly ClientScriptManager _scriptManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationScriptManager"/> class.
        /// </summary>
        /// <param name="scriptManager">The script manager for the page containing the <see cref="ModelValidator"/>.</param>
        /// <param name="modelType">Type of the model.</param>
        /// <param name="clientId">The client ID of the <see cref="ModelValidator"/>. This is used to register the scripts.</param>
        public ValidationScriptManager(ClientScriptManager scriptManager, Type modelType, string clientId)
        {
            if (scriptManager == null)
            {
                throw new ArgumentNullException("scriptManager");
            }

            if (modelType == null)
            {
                throw new ArgumentNullException("modelType");
            }

            if (clientId == null)
            {
                throw new ArgumentNullException("clientId");
            }

            _scriptManager = scriptManager;
            _modelType = modelType;
            _clientId = clientId;
        }

        #region IValidationScriptManager Members

        /// <summary>
        /// Supresses validation for the given collection of <see cref="IButtonControl"/>s.
        /// </summary>
        /// <param name="buttons">The buttons.</param>
        public void SupressValidation(IEnumerable<IButtonControl> buttons)
        {
            // If there´s any control which needs to bypass validation, register a call to SupressValidation.
            if (!_scriptManager.IsClientScriptBlockRegistered("supress_validation") &&
                buttons.Where(b => b.CausesValidation == false).Any())
            {
                IEnumerable<string> buttonIds = from button in buttons
                                                where !button.CausesValidation
                                                select String.Format("'{0}'", ((Control) button).ClientID);

                string buttonIdList = String.Join(",", buttonIds.ToArray());
                string controlsNotCausingValidation = String.Format("[{0}]", buttonIdList);

                string script = String.Format("SupressValidation({0});", controlsNotCausingValidation);
                _scriptManager.RegisterStartupScript(typeof (ModelValidator), "supress_validation", script, true);
            }
        }

        /// <summary>
        /// Registers the validation script.
        /// </summary>
        /// <param name="options">The options.</param>
        /// <param name="controlMapping">The control mapping.</param>
        public void RegisterValidationScript(IDictionary<string, object> options,
                                             IDictionary<string, string> controlMapping)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            string serializedOptions = serializer.Serialize(options);
            ControlJsonValidationConfigFormatter formatter = new ControlJsonValidationConfigFormatter(controlMapping);
            RuleSet rules = ActiveRuleProviders.GetRulesForType(_modelType);
            string formattedRules = formatter.FormatRules(rules);

            string script = String.Format("xVal.AttachValidator(null, {0}, {1});",
                                          formattedRules, serializedOptions);
            _scriptManager.RegisterStartupScript(typeof(ModelValidator), _clientId, script, true);
        }

        #endregion
    }
}