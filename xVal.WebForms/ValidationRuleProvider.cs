using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace xVal.WebForms
{
    public class ValidationRuleProvider
    {
        public IDictionary<string, object> GetValidationRules(IEnumerable<ValidationAttribute> attributes)
        {
            IDictionary<string, object> rules = new Dictionary<string, object>();
            foreach (ValidationAttribute attribute in attributes)
            {
                IDictionary<string, object> attributeRules = GetValidationRules(attribute);
                foreach (KeyValuePair<string, object> rule in attributeRules)
                {
                    rules.Add(rule.Key, rule.Value);
                }
            }
            return rules;
        }

        private static IDictionary<string, object> GetValidationRules(ValidationAttribute attribute)
        {
            IDictionary<string, object> rules = new Dictionary<string, object>();

            if (attribute is RequiredAttribute)
            {
                rules.Add("required", true);
            }
            else if (attribute is RangeAttribute)
            {
                RangeAttribute rangeAttribute = (RangeAttribute) attribute;
                rules.Add("range", new[] {rangeAttribute.Minimum, rangeAttribute.Maximum});
            }
            else if (attribute is StringLengthAttribute)
            {
                StringLengthAttribute stringLengthAttribute = (StringLengthAttribute) attribute;
                rules.Add("rangelength",
                          new[] {stringLengthAttribute.MinimumLength, stringLengthAttribute.MaximumLength});
            }
            else if (attribute is DataTypeAttribute)
            {
                DataTypeAttribute dataTypeAttribute = (DataTypeAttribute) attribute;
                switch (dataTypeAttribute.DataType)
                {
                    case DataType.Custom:
                        break;
                    case DataType.DateTime:
                        break;
                    case DataType.Date:
                        rules.Add("date", true);
                        break;
                    case DataType.Time:
                        break;
                    case DataType.Duration:
                        break;
                    case DataType.PhoneNumber:
                        rules.Add("phoneUS", true);
                        break;
                    case DataType.Currency:
                        break;
                    case DataType.Text:
                        break;
                    case DataType.Html:
                        break;
                    case DataType.MultilineText:
                        break;
                    case DataType.EmailAddress:
                        rules.Add("email", true);
                        break;
                    case DataType.Password:
                        break;
                    case DataType.Url:
                        rules.Add("url", true);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            return rules;
        }
    }
}