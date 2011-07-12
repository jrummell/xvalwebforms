using System;
using System.Reflection;
using System.Web.UI;

namespace xVal.WebForms
{
    public class ModelPage : Page
    {
        private readonly IValidatorCollection _validatorCollection;

        /// <summary>
        /// Initializes a new instance of the <see cref="ModelPage"/> class.
        /// </summary>
        public ModelPage()
            : this(null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ModelPage"/> class.
        /// </summary>
        /// <param name="validatorCollection">The validator collection.</param>
        public ModelPage(IValidatorCollection validatorCollection)
        {
            _validatorCollection = validatorCollection ?? new PageValidatorCollection(this);
        }

        /// <summary>
        /// Instructs the validation controls in the specified validation group to validate their assigned information.
        /// </summary>
        /// <param name="validationGroup">The validation group name of the controls to validate.</param>
        public override void Validate(string validationGroup)
        {
            SetValidated();

            ValidatorCollection validators = GetModelValidators(validationGroup);
            if (String.IsNullOrEmpty(validationGroup) && (Validators.Count == validators.Count))
            {
                Validate();
            }
            else
            {
                foreach (IValidator validator in validators)
                {
                    validator.Validate();
                }
            }
        }

        private void SetValidated()
        {
            // this is an ugly hack, but I haven't found another way to tell Page that we've already validated and prevent the following exception when IsValid is accessed:
            //  HttpException : Page.IsValid cannot be called before validation has taken place. It should be queried in the event handler for a control that has CausesValidation=True and initiated the postback, or after a call to Page.Validate.
            Type pageType = typeof (Page);
            FieldInfo validatedField =
                pageType.GetField("_validated", BindingFlags.Instance | BindingFlags.NonPublic);
            validatedField.SetValue(this, true);
        }

        /// <summary>
        /// Gets the model validators.
        /// </summary>
        /// <param name="validationGroup">The validation group.</param>
        /// <returns></returns>
        public ValidatorCollection GetModelValidators(string validationGroup)
        {
            if (validationGroup == null)
            {
                validationGroup = String.Empty;
            }

            ValidatorCollection validators = new ValidatorCollection();
            foreach (IValidator validator in _validatorCollection)
            {
                IModelValidator modelValidator = validator as IModelValidator;
                if (modelValidator != null)
                {
                    if (String.Compare(modelValidator.ValidationGroup, validationGroup, StringComparison.Ordinal) == 0)
                    {
                        validators.Add(modelValidator);
                    }
                }
                else if (validationGroup.Length == 0)
                {
                    validators.Add(validator);
                }
            }

            return validators;
        }
    }
}