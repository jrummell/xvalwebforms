using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.Reflection;
using System.Web.Compilation;
using System.ComponentModel.DataAnnotations;

namespace xVal.WebForms
{
    [ParseChildren(true)]
    [PersistChildren(false)]
    public class ModelObjectValidator : BaseValidator
    {
        private readonly IControlValueResolver _controlValueResolver;
        private readonly ModelPropertyCollection _modelProperties = new ModelPropertyCollection();
        private Type _modelType;

        /// <summary>
        /// Initializes a new instance of the <see cref="ModelObjectValidator"/> class.
        /// </summary>
        public ModelObjectValidator()
            : this(null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ModelObjectValidator"/> class.
        /// </summary>
        /// <param name="controlValueResolver">The control value resolver.</param>
        /// <param name="validators">The validators.</param>
        /// <remarks>
        /// This constructor is for unit testing.
        /// </remarks>
        public ModelObjectValidator(IControlValueResolver controlValueResolver)
        {
            _controlValueResolver = controlValueResolver ?? new ControlValueResolver(this);
        }

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
            get { return (string)ViewState["ModelType"]; }
            set { ViewState["ModelType"] = value; }
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
            IValidatableObject model = GetModelInstance(type) as IValidatableObject;

            // get any errors
            IEnumerable<ValidationResult> results = model.Validate(null);

            if (results.Any())
            {
                ErrorMessage = String.Join(" ", results.Select(result => result.ErrorMessage));
                return false;
            }

            return true;
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

                string validatableObjectTypeName = typeof(IValidatableObject).ToString();
                if (_modelType.GetInterface(validatableObjectTypeName) == null)
                {
                    throw new InvalidOperationException("ModelType must implement " + validatableObjectTypeName);
                }
            }

            return _modelType;
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
                        value = Convert.ChangeType(valueString, property.PropertyType);
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
    }
}
