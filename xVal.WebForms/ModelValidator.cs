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
        public ModelValidator()
            : this(null)
        {
        }

        public ModelValidator(IValidationRunner validationRunner)
            : base(validationRunner)
        {
        }

        protected override bool ControlPropertiesValid()
        {
            if (String.IsNullOrEmpty(ModelType))
            {
                return false;
            }

            Type modelType = GetModelType();
            if (modelType == null)
            {
                return false;
            }

            Type validatableObjectInterface = modelType.GetInterface(typeof(IValidatableObject).Name);
            return validatableObjectInterface != null;
        }

        protected override bool EvaluateIsValid()
        {
            List<ModelPropertyValidator> propertyValidators = new List<ModelPropertyValidator>();
            BuildPropertyValidatorList(Page, propertyValidators);

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
                        Page.Validators.Add(new ValidationError(result.ErrorMessage, ValidationGroup));
                    }

                    return !results.Any();
                }
            }

            return true;
        }

        private static void BuildPropertyValidatorList(Control control, ICollection<ModelPropertyValidator> propertyValidators)
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