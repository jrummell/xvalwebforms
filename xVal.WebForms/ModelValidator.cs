using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

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

            ICollection<ModelPropertyValidator> validators = GetPropertyValidators();

            if (validators.Count > 0)
            {
                Type modelType = GetModelType();
                IValidatableObject model = Activator.CreateInstance(modelType) as IValidatableObject;

                if (model != null)
                {
                    // set the model with the form values
                    List<ValidationResult> results = new List<ValidationResult>();
                    foreach (ModelPropertyValidator propertyValidator in validators)
                    {
                        PropertyInfo property = modelType.GetProperty(propertyValidator.PropertyName);
                        object propertyValue;
                        try
                        {
                            propertyValue = GetModelPropertyValue(property.Name, propertyValidator.ControlToValidate);
                        }
                        catch(ValidationException ex)
                        {
                            results.Add(ex.ValidationResult);
                            continue;
                        }
                        property.SetValue(model, propertyValue, null);
                    }

                    results.AddRange(model.Validate(new ValidationContext(model, null, null)));
                    foreach (ValidationResult result in results)
                    {
                        _validatorCollection.Add(new ValidationError(result.ErrorMessage, ValidationGroup));
                    }

                    return !results.Any();
                }
            }

            return true;
        }

        private ICollection<ModelPropertyValidator> GetPropertyValidators()
        {
            IEnumerable<ModelPropertyValidator> validators;
            ModelPage modelPage = Page as ModelPage;
            if (modelPage != null)
            {
                validators =
                    modelPage.GetModelValidators(ValidationGroup).OfType<ModelPropertyValidator>();
            }
            else
            {
                validators =
                    _validatorCollection.OfType<ModelPropertyValidator>().Where(
                        val => String.Compare(ValidationGroup, val.ValidationGroup) == 0);
            }

            return validators.ToList();
        }
    }
}