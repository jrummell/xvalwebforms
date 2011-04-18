using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Compilation;
using System.Web.UI;
using System.Web.UI.WebControls;
using xVal.ServerSide;
using xVal.WebForms;

[assembly: WebResource(ModelValidator.ClientScriptResourceName, "text/javascript")]

namespace xVal.WebForms
{
    [ParseChildren(true)]
    [PersistChildren(false)]
    public class ModelValidator : BaseValidator
    {
        internal const string ClientScriptResourceName = "xVal.WebForms.ClientSidePlugins.xVal.jquery.validate.js";
        private readonly IControlValueResolver _controlValueResolver;
        private readonly ModelPropertyCollection _modelProperties = new ModelPropertyCollection();
        private readonly IValidatorCollection _validators;
        private Type _modelType;
        private IValidationScriptManager _validationScriptManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="ModelValidator"/> class.
        /// </summary>
        public ModelValidator()
            : this(null, null, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ModelValidator"/> class.
        /// </summary>
        /// <param name="controlValueResolver">The control value resolver.</param>
        /// <param name="validators">The validators.</param>
        /// <param name="validationScriptManager">The validation script manager.</param>
        /// <remarks>
        /// This constructor is for unit testing.
        /// </remarks>
        public ModelValidator(IControlValueResolver controlValueResolver, IValidatorCollection validators,
                              IValidationScriptManager validationScriptManager)
        {
            _controlValueResolver = controlValueResolver ?? new ControlValueResolver(this);
            _validators = validators ?? new PageValidatorCollection(this);
            _validationScriptManager = validationScriptManager;
        }

        /// <summary>
        /// Gets the model properties.
        /// </summary>
        /// <value>The model properties.</value>
        [PersistenceMode(PersistenceMode.InnerProperty)]
        public ModelPropertyCollection ModelProperties
        {
            get { return _modelProperties; }
        }

        /// <summary>
        /// Gets or sets the type of the model.
        /// </summary>
        /// <value>The type of the model.</value>
        public string ModelType
        {
            get { return (string) ViewState["ModelType"]; }
            set { ViewState["ModelType"] = value; }
        }

        /// <summary>
        /// Gets or sets the validation summary ID.
        /// </summary>
        /// <value>The validation summary ID.</value>
        public string ValidationSummaryID
        {
            get { return (string) ViewState["ValidationSummaryID"]; }
            set { ViewState["ValidationSummaryID"] = value; }
        }

        /// <summary>
        /// Notifies the control that an element was parsed and adds the element to the <see cref="T:System.Web.UI.WebControls.Label"/> control.
        /// </summary>
        /// <param name="obj">An object that represents the parsed element.</param>
        protected override void AddParsedSubObject(object obj)
        {
            ModelProperty property = obj as ModelProperty;
            if (obj != null)
            {
                ModelProperties.Add(property);
            }
        }

        /// <summary>
        /// Returns true if the ModelType is specified.
        /// </summary>
        /// <returns></returns>
        protected override bool ControlPropertiesValid()
        {
            return !String.IsNullOrEmpty(ModelType);
        }

        /// <summary>
        /// When overridden in a derived class, this method contains the code to determine whether the value in the input control is valid.
        /// </summary>
        /// <returns>
        /// true if the value in the input control is valid; otherwise, false.
        /// </returns>
        protected override bool EvaluateIsValid()
        {
            Type type = GetModelType();
            object model = GetModelInstance(type);

            // get any errors
            IEnumerable<ErrorInfo> errors = DataAnnotationsValidationRunner.GetErrors(model);

            // add each error to the page's validator collection - these will display in a ValidationSummary.
            foreach (ErrorInfo error in errors)
            {
                _validators.Add(new ValidationError(error.PropertyName + ": " + error.ErrorMessage, ValidationGroup));
            }

            return errors.Count() == 0;
        }

        /// <summary>
        /// Gets the model instance.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        private object GetModelInstance(Type type)
        {
            ConstructorInfo constructorInfo = type.GetConstructor(BindingFlags.Instance |
                                                                  BindingFlags.Public, null, new Type[0], null);

            if (constructorInfo == null)
            {
                // If no public constructor found try to get a non-public one.
                constructorInfo = type.GetConstructor(BindingFlags.Instance |
                                                      BindingFlags.NonPublic, null, new Type[0], null);

                // No default constructor found. Throw exception.
                if (constructorInfo == null)
                {
                    throw new InvalidOperationException("Could not find parameterless constructor for type: " +
                                                        ModelType);
                }
            }

            object model = constructorInfo.Invoke(null);

            if (model == null)
            {
                throw new InvalidOperationException("Could not create an instance of type: " + ModelType);
            }

            // update the model's properties with the values from the validated controls.
            foreach (ModelProperty modelProperty in ModelProperties)
            {
                PropertyInfo property = type.GetProperty(modelProperty.PropertyName);
                if (property == null)
                {
                    string message = String.Format("Could not find property {0} on type: {1}",
                                                   modelProperty.PropertyName, type);
                    throw new InvalidOperationException(message);
                }

                // get the value of the input control
                string valueString = _controlValueResolver.GetControlValue(modelProperty.ControlToValidate);
                object value;

                // get the underlying type of the nullable type (get int from int?).
                Type nullablePropertyType = Nullable.GetUnderlyingType(property.PropertyType);
                try
                {
                    if (nullablePropertyType != null)
                    {
                        value = Convert.ChangeType(valueString, nullablePropertyType);
                    }
                    else
                    {
                        if (property.PropertyType.IsEnum)
                        {
                            value = Enum.Parse(property.PropertyType, valueString);
                        }
                        else
                        {
                            value = Convert.ChangeType(valueString, property.PropertyType);
                        }
                    }
                }
                catch (FormatException)
                {
                    value = null;
                }

                if (value != null)
                {
                    // set the property value
                    property.SetValue(model, value, null);
                }
            }

            return model;
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Init"/> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.EventArgs"/> that contains the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            // Subscribe to page PreRenderComplete event.
            Page.PreRenderComplete += Page_PreRenderComplete;
        }

        /// <summary>
        /// Gets the control mapping.
        /// </summary>
        /// <returns></returns>
        private IDictionary<string, string> GetControlMapping()
        {
            IDictionary<string, string> controlMapping = new Dictionary<string, string>();
            // Loop through all ModelProperties
            foreach (ModelProperty modelProperty in ModelProperties)
            {
                // Try to find control.
                Control control = Parent.FindControl(modelProperty.ControlToValidate);
                if (control != null)
                {
                    controlMapping.Add(modelProperty.PropertyName, control.ClientID);
                }
                else
                {
                    // No control was found so throw exception.
                    throw new InvalidOperationException(String.Format("Could not find contorol with ID: {0}",
                                                                      modelProperty.ControlToValidate));
                }
            }

            return controlMapping;
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

        /// <summary>
        /// Handles the PreRenderComplete event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void Page_PreRenderComplete(object sender, EventArgs e)
        {
            RegisterValidationScriptIfClientScriptEnabled();
        }

        /// <summary>
        /// Registers the validation script if client script enabled.
        /// </summary>
        public void RegisterValidationScriptIfClientScriptEnabled()
        {
            // don't add client validation script if client script isn't enabled or visible.
            if (!EnableClientScript || !Visible)
            {
                return;
            }

            RegisterValidationScript();
        }

        /// <summary>
        /// Registers the validation script.
        /// </summary>
        private void RegisterValidationScript()
        {
            // if the default constructor was used, initialize the script manager with the default implementation.
            if (_validationScriptManager == null)
            {
                _validationScriptManager = new ValidationScriptManager(Page.ClientScript, GetModelType(), ClientID);
            }

            IEnumerable<IButtonControl> buttons = Page.GetButtons();

            // If there´s any control which needs to bypass validation (Not Causes Validation), register a call to SupressValidation.
            _validationScriptManager.SupressValidation(buttons);

            // Render the scripts at latest stage possible, to ensure controls collection is complete, 
            // and generated scripts contain all possible parameters.

            IDictionary<string, object> options = new Dictionary<string, object>();

            if (!String.IsNullOrEmpty(ValidationSummaryID))
            {
                AddValidationSummaryOptions(options);
            }

            if (buttons.Where(b => !String.IsNullOrEmpty(b.ValidationGroup)).Any())
            {
                AddValidationGroupOptions(options, buttons);
            }

            IDictionary<string, string> controlMapping = GetControlMapping();

            _validationScriptManager.RegisterValidationScript(options, controlMapping);
        }

        /// <summary>
        /// Adds the validation summary options.
        /// </summary>
        /// <param name="options">The options.</param>
        private void AddValidationSummaryOptions(IDictionary<string, object> options)
        {
            Control control = Page.FindControlRecursive(ValidationSummaryID);
            if (control == null)
            {
                throw new InvalidOperationException(
                    String.Format("Could not find a ValidationSummary control with ID: {0}", ValidationSummaryID));
            }

            ValidationSummaryOptions summaryOptions = new ValidationSummaryOptions(control.ClientID, null);

            options.Add("ValidationSummary", summaryOptions);
        }

        /// <summary>
        /// Adds the validation group options.
        /// </summary>
        /// <param name="options">The options.</param>
        /// <param name="buttons">The buttons.</param>
        private void AddValidationGroupOptions(IDictionary<string, object> options, IEnumerable<IButtonControl> buttons)
        {
            IEnumerable<IEditableTextControl> textBoxes = Page.GetTextBoxes();
            ValidationGroups groups = new ValidationGroups(buttons, textBoxes);
            options.Add("valgroups", groups.Groups);
        }
    }
}