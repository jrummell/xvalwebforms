using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.UI.WebControls;

namespace xVal.WebForms
{
    public class ValidatorControlFactory
    {
        private readonly List<DataType> _regexDataTypes =
            new List<DataType> { DataType.EmailAddress, DataType.Url, DataType.ImageUrl, DataType.PhoneNumber };
        public static readonly string EmailRegex = @"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*";
        public static readonly string UrlRegex = @"http(s)?://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?";
        public static readonly string PhoneNumberRegex = @"1?\W*([2-9][0-8][0-9])\W*([2-9][0-9]{2})\W*([0-9]{4})(\sx?(\d*))?";

        public BaseValidator CreateValidator(ValidationAttribute attribute)
        {
            Type attributeType = attribute.GetType();

            BaseValidator validator = null;
            if (attributeType == typeof(RequiredAttribute))
            {
                validator = new RequiredFieldValidator();
            }
            else if (attributeType == typeof(StringLengthAttribute))
            {
                //TODO: set MaxLength on TextBox?
            }
            else if (attributeType == typeof(EnumDataTypeAttribute))
            {
                //TODO: Enum type?
                validator = new CompareValidator
                {
                    Operator = ValidationCompareOperator.DataTypeCheck,
                    Type = ValidationDataType.Integer
                };
            }
            else if (attributeType == typeof(DataTypeAttribute))
            {
                DataTypeAttribute dataTypeAttribute = (DataTypeAttribute)attribute;

                ValidationDataType? validationDataType = ConvertDataType(dataTypeAttribute);

                if (validationDataType != null)
                {
                    validator = new CompareValidator
                    {
                        Operator = ValidationCompareOperator.DataTypeCheck,
                        Type = validationDataType.Value
                    };
                }
                else
                {
                    // certain DataTypes can be validated with regex
                    if (_regexDataTypes.Contains(dataTypeAttribute.DataType))
                    {
                        validator = CreateRegexValidator(dataTypeAttribute);
                    }
                }
            }
            else if (attributeType == typeof(RangeAttribute))
            {
                validator = CreateRangeValidator((RangeAttribute)attribute);
            }
            else if (attributeType == typeof(RegularExpressionAttribute))
            {
                RegularExpressionAttribute regexAttribute = (RegularExpressionAttribute)attribute;

                validator = new RegularExpressionValidator
                {
                    ValidationExpression = regexAttribute.Pattern
                };
            }

            return validator;
        }

        private static ValidationDataType? ConvertDataType(DataTypeAttribute dataTypeAttribute)
        {
            ValidationDataType? validationDataType = null;

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

            return validationDataType;
        }

        private RegularExpressionValidator CreateRegexValidator(DataTypeAttribute dataTypeAttribute)
        {
            string expression = GetRegex(dataTypeAttribute);

            if (!String.IsNullOrEmpty(expression))
            {
                return new RegularExpressionValidator
                {
                    ValidationExpression = expression
                };
            }

            return null;
        }

        private static string GetRegex(DataTypeAttribute dataTypeAttribute)
        {
            string expression = null;
            switch (dataTypeAttribute.DataType)
            {
                case DataType.Currency:
                    break;
                case DataType.Custom:
                    break;
                case DataType.Date:
                    break;
                case DataType.DateTime:
                    break;
                case DataType.Duration:
                    break;
                case DataType.EmailAddress:
                    expression = EmailRegex;
                    break;
                case DataType.Html:
                    break;
                case DataType.MultilineText:
                    break;
                case DataType.Password:
                    break;
                case DataType.PhoneNumber:
                    expression = PhoneNumberRegex;
                    break;
                case DataType.Text:
                    break;
                case DataType.Time:
                    break;
                case DataType.Url:
                case DataType.ImageUrl:
                    expression = UrlRegex;
                    break;
                default:
                    break;
            }

            return expression;
        }

        private RangeValidator CreateRangeValidator(RangeAttribute attribute)
        {
            // String is the default type.
            ValidationDataType dataType = ValidationDataType.String;
            if (attribute.OperandType == typeof(int))
            {
                dataType = ValidationDataType.Integer;
            }
            else if (attribute.OperandType == typeof(double))
            {
                dataType = ValidationDataType.Double;
            }
            else if (attribute.OperandType == typeof(DateTime))
            {
                dataType = ValidationDataType.Date;
            }

            return new RangeValidator
            {
                Type = dataType,
                MinimumValue = attribute.Minimum.ToString(),
                MaximumValue = attribute.Maximum.ToString()
            };
        }
    }
}
