using System;
using System.ComponentModel;
using System.Web.Compilation;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace xVal.WebForms
{
    /// <summary>
    /// Base model validator.
    /// </summary>
    /// <remarks>
    /// We're implementing <see cref="IValidator"/> instead of <see cref="BaseValidator"/> since 
    /// we don't need (or want) standard ASP.NET web form validation scripts injected into the page.
    /// </remarks>
    public abstract class ModelValidatorBase : WebControl, IValidator
    {
        //TODO: persist properties with ViewState
    
        private Type _modelType;

        /// <summary>
        /// Initializes a new instance of the <see cref="ModelValidatorBase"/> class.
        /// </summary>
        protected ModelValidatorBase()
            : this(null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ModelValidatorBase"/> class.
        /// </summary>
        /// <param name="validationRunner">The validation runner.</param>
        protected ModelValidatorBase(IValidationRunner validationRunner)
        {
            ValidationRunner = validationRunner ?? new DataAnnotationsValidationRunner();
            
            EnableClientScript = true;
        }

        /// <summary>
        /// Gets the validation runner.
        /// </summary>
        protected IValidationRunner ValidationRunner { get; private set; }

        /// <summary>
        /// Gets or sets the type of the model.
        /// </summary>
        /// <value>
        /// The type of the model.
        /// </value>
        public string ModelType { get; set; }

        /// <summary>
        /// Gets or sets the control to validate.
        /// </summary>
        /// <value>
        /// The control to validate.
        /// </value>
        public string ControlToValidate { get; set; }

        /// <summary>
        /// Gets or sets the validation group.
        /// </summary>
        /// <value>
        /// The validation group.
        /// </value>
        public string ValidationGroup { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to client script is enabled.
        /// </summary>
        /// <value>
        ///   <c>true</c> if client script is enabled; otherwise, <c>false</c>.
        /// </value>
        public bool EnableClientScript { get; set; }

        #region IValidator Members

        /// <summary>
        /// Evaluates the condition it checks and updates the <see cref="P:System.Web.UI.IValidator.IsValid"/> property.
        /// </summary>
        public void Validate()
        {
            IsValid = EvaluateIsValid();
        }

        /// <summary>
        /// Gets or sets a value indicating whether the user-entered content in the specified control passes validation.
        /// </summary>
        /// <returns>true if the content is valid; otherwise, false.</returns>
        public bool IsValid { get; set; }

        /// <summary>
        /// Gets or sets the error message text generated when the condition being validated fails.
        /// </summary>
        /// <returns>The error message to generate.</returns>
        public string ErrorMessage { get; set; }

        #endregion

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Init"/> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> object that contains the event data.</param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            if (ErrorMessage == null)
            {
                ErrorMessage = String.Empty;
            }

            Page.Validators.Add(this);
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.Unload"/> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> object that contains event data.</param>
        protected override void OnUnload(EventArgs e)
        {
            if (Page != null)
            {
                Page.Validators.Remove(this);
            }
            base.OnUnload(e);
        }


        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.PreRender"/> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> object that contains the event data.</param>
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            ControlPropertiesValid();
        }

        /// <summary>
        /// Gets the type of the model.
        /// </summary>
        /// <returns></returns>
        protected Type GetModelType()
        {
            if (_modelType == null)
            {
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
        /// Gets the model property value.
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        /// <param name="valueControlId">The value control id.</param>
        /// <returns></returns>
        protected object GetModelPropertyValue(string propertyName, string valueControlId)
        {
            string stringValue = GetControlValidationValue(valueControlId);
            Type propertyType = GetModelType().GetProperty(propertyName).PropertyType;
            try
            {
                return Convert.ChangeType(stringValue, propertyType);
            }
            catch (FormatException)
            {
                return null;
            }
        }

        /// <summary>
        /// Gets the control validation value.
        /// </summary>
        /// <remarks>
        /// Based on the reflected source of <see cref="BaseValidator.GetControlValidationValue"/>.
        /// </remarks>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        private string GetControlValidationValue(string name)
        {
            Control valueControl = NamingContainer.FindControl(name);
            if (valueControl == null)
            {
                return null;
            }
            PropertyDescriptor validationProperty = BaseValidator.GetValidationProperty(valueControl);
            if (validationProperty == null)
            {
                return null;
            }

            object value = validationProperty.GetValue(valueControl);
            if (value is ListItem)
            {
                return ((ListItem) value).Value;
            }

            if (value != null)
            {
                return value.ToString();
            }

            return String.Empty;
        }

        protected abstract bool EvaluateIsValid();

        protected virtual bool ControlPropertiesValid()
        {
            if (String.IsNullOrEmpty(ControlToValidate))
            {
                throw new InvalidOperationException("ControlToValidate is required.");
            }

            if (String.IsNullOrEmpty(ModelType))
            {
                throw new InvalidOperationException("ModelType is required.");
            }

            return true;
        }

        protected string GetControlRenderID(string name)
        {
            Control control = FindControl(name);
            if (control == null)
            {
                return string.Empty;
            }
            return control.ClientID;
        }
    }
}