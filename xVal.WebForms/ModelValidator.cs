using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Web.UI;

namespace xVal.WebForms
{
    public class ModelValidator : ModelValidatorBase
    {
        private IValidatorCollection _validatorCollection;

        /// <summary>
        /// Initializes a new instance of the <see cref="ModelValidator"/> class.
        /// </summary>
        public ModelValidator()
            : this(null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ModelValidator"/> class.
        /// </summary>
        /// <param name="validatorCollection">The validator collection.</param>
        public ModelValidator(IValidatorCollection validatorCollection)
        {
            _validatorCollection = validatorCollection;
        }

        /// <summary>
        /// Returns true if the properties are valid.
        /// </summary>
        /// <returns></returns>
        public override bool ControlPropertiesValid()
        {
            base.ControlPropertiesValid();

            string validatableObjectTypeName = typeof (IValidatableObject).Name;
            Type validatableObjectInterface = GetModelType().GetInterface(validatableObjectTypeName);

            if (validatableObjectInterface == null)
            {
                throw new InvalidOperationException("ModelType does not implement " + validatableObjectTypeName);
            }

            return true;
        }

        /// <summary>
        /// Evaluates the condition.
        /// </summary>
        /// <returns></returns>
        protected override bool EvaluateIsValid()
        {
            if (_validatorCollection == null)
            {
                _validatorCollection = new PageValidatorCollection(Page);
            }

            List<ModelPropertyValidator> propertyValidators = new List<ModelPropertyValidator>();
            BuildPropertyValidatorList(NamingContainer, propertyValidators);

            if (propertyValidators.Count > 0)
            {
                Type modelType = GetModelType();
                IValidatableObject model = Activator.CreateInstance(modelType) as IValidatableObject;

                if (model != null)
                {
                    // set the model with the form values
                    foreach (ModelPropertyValidator propertyValidator in propertyValidators)
                    {
                        PropertyInfo property = modelType.GetProperty(propertyValidator.PropertyName);
                        object propertyValue = GetModelPropertyValue(property.Name, propertyValidator.ControlToValidate);
                        property.SetValue(model, propertyValue, null);
                    }

                    IEnumerable<ValidationResult> results = model.Validate(new ValidationContext(model, null, null));
                    foreach (ValidationResult result in results)
                    {
                        _validatorCollection.Add(new ValidationError(result.ErrorMessage));
                    }

                    return !results.Any();
                }
            }

            return true;
        }

        /// <summary>
        /// Builds the property validator list.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <param name="propertyValidators"></param>
        private static void BuildPropertyValidatorList(Control control,
                                                       ICollection<ModelPropertyValidator> propertyValidators)
        {
            ModelPropertyValidator propertyValidator = control as ModelPropertyValidator;
            if (propertyValidator != null)
            {
                propertyValidators.Add(propertyValidator);
            }
            else if (control.HasControls())
            {
                foreach (Control c in control.Controls)
                {
                    BuildPropertyValidatorList(c, propertyValidators);
                }
            }
        }
    }
}