using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Compilation;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace xVal.WebForms
{
    public class ModelPropertyValidator : BaseValidator, INamingContainer
    {
        private Type _modelType;

        /// <summary>
        /// Gets or sets the name of the type.
        /// </summary>
        /// <value>
        /// The name of the type.
        /// </value>
        public string TypeName { get; set; }

        /// <summary>
        /// Gets or sets the name of the property.
        /// </summary>
        /// <value>
        /// The name of the property.
        /// </value>
        public string PropertyName { get; set; }

        /// <summary>
        /// When overridden in a derived class, this method contains the code to determine whether the value in the input control is valid.
        /// </summary>
        /// <returns>
        /// true if the value in the input control is valid; otherwise, false.
        /// </returns>
        protected override bool EvaluateIsValid()
        {
            EnsureChildControls();

            foreach (BaseValidator validator in Controls)
            {
                validator.Validate();

                if (!validator.IsValid)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Called by the ASP.NET page framework to notify server controls that use composition-based implementation to create any child controls they contain in preparation for posting back or rendering.
        /// </summary>
        protected override void CreateChildControls()
        {
            _modelType = GetModelType();

            IEnumerable<ValidationAttribute> attributes = DataAnnotationsValidationRunner.GetValidators(_modelType);
            foreach (ValidationAttribute attribute in attributes)
            {
                Type attributeType = attribute.GetType();

                BaseValidator validator = null;
                if (attributeType == typeof (RequiredAttribute))
                {
                    validator = new RequiredFieldValidator();
                }
                else if (attributeType == typeof (StringLengthAttribute))
                {
                    //TODO: set MaxLength on TextBox?
                }
                else if (attributeType == typeof (EnumDataTypeAttribute))
                {
                    //TODO: Enum type?
                    validator = new CompareValidator
                                    {
                                        Operator = ValidationCompareOperator.DataTypeCheck,
                                        Type = ValidationDataType.Integer
                                    };
                }
                else if (attributeType == typeof (DataTypeAttribute))
                {
                    DataTypeAttribute dataTypeAttribute = (DataTypeAttribute) attribute;

                    //TODO: CompareValidator or ReqularExpressionValidator

                    ValidationDataType validationDataType;

                    switch (dataTypeAttribute.DataType)
                    {
                        case DataType.Custom:
                            break;
                        case DataType.DateTime:
                        case DataType.Date:
                            validationDataType = ValidationDataType.Date;
                            break;
                        case DataType.Time:
                            break;
                        case DataType.Duration:
                            break;
                        case DataType.PhoneNumber:
                            break;
                        case DataType.Currency:
                            validationDataType = ValidationDataType.Currency;
                            break;
                        case DataType.Text:
                            validationDataType = ValidationDataType.String;
                            break;
                        case DataType.Html:
                            break;
                        case DataType.MultilineText:
                            validationDataType = ValidationDataType.String;
                            break;
                        case DataType.EmailAddress:
                            break;
                        case DataType.Password:
                            break;
                        case DataType.Url:
                            break;
                        case DataType.ImageUrl:
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
                else if (attributeType == typeof (RangeAttribute))
                {
                    RangeAttribute rangeAttribute = (RangeAttribute) attribute;

                    // String is the default type.
                    ValidationDataType dataType = ValidationDataType.String;
                    if (rangeAttribute.OperandType == typeof (int))
                    {
                        dataType = ValidationDataType.Integer;
                    }
                    else if (rangeAttribute.OperandType == typeof (double))
                    {
                        dataType = ValidationDataType.Double;
                    }
                    else if (rangeAttribute.OperandType == typeof (DateTime))
                    {
                        dataType = ValidationDataType.Date;
                    }

                    validator = new RangeValidator
                                    {
                                        Type = dataType,
                                        MinimumValue = rangeAttribute.Minimum.ToString(),
                                        MaximumValue = rangeAttribute.Maximum.ToString()
                                    };
                }
                else if (attributeType == typeof (RegularExpressionAttribute))
                {
                    RegularExpressionAttribute regexAttribute = (RegularExpressionAttribute) attribute;

                    validator = new RegularExpressionValidator
                                    {
                                        ValidationExpression = regexAttribute.Pattern
                                    };
                }

                if (validator != null)
                {
                    validator.ID = ID + "_val" + (Controls.Count + 1);
                    validator.ControlToValidate = ControlToValidate;
                    validator.Display = Display;
                    validator.ValidationGroup = ValidationGroup;
                    validator.ErrorMessage = attribute.ErrorMessage;

                    Controls.Add(validator);
                }
            }
        }

        /// <summary>
        /// Gets the type of the model.
        /// </summary>
        /// <returns></returns>
        private Type GetModelType()
        {
            if (_modelType == null)
            {
                if (String.IsNullOrEmpty(TypeName))
                {
                    throw new InvalidOperationException("TypeName must be set.");
                }

                _modelType = Type.GetType(TypeName);

                if (_modelType == null)
                {
                    // App_Code Types are created with System.Web.Compilation.BuildManager
                    _modelType = BuildManager.GetType(TypeName, false);
                }

                if (_modelType == null)
                {
                    throw new InvalidOperationException("Could not get a Type from " + TypeName);
                }
            }

            return _modelType;
        }
    }
}