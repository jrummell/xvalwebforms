using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace xVal.WebForms
{
    public class ValidationRuleProvider : IValidationRuleProvider
    {
        /// <summary>
        /// Gets the rules.
        /// </summary>
        /// <param name="attributes">The attributes.</param>
        /// <returns></returns>
        public RuleCollection GetRules(IEnumerable<ValidationAttribute> attributes)
        {
            if (attributes == null)
            {
                throw new ArgumentNullException("attributes");
            }

            List<Rule> rules = attributes.Select(attribute => GetRule(attribute)).ToList();
            return new RuleCollection(rules);
        }

        private static Rule GetRule(ValidationAttribute attribute)
        {
            Rule rule = null;

            if (attribute is RequiredAttribute)
            {
                rule = new Rule {Name = "required", Options = true};
            }
            else if (attribute is RangeAttribute)
            {
                RangeAttribute rangeAttribute = (RangeAttribute) attribute;
                rule = new Rule {Name = "range", Options = new[] {rangeAttribute.Minimum, rangeAttribute.Maximum}};
            }
            else if (attribute is StringLengthAttribute)
            {
                StringLengthAttribute stringLengthAttribute = (StringLengthAttribute) attribute;
                rule = new Rule
                           {
                               Name = "rangelength",
                               Options =
                                   new[] { stringLengthAttribute.MinimumLength, stringLengthAttribute.MaximumLength }
                           };
            }
            else if (attribute is DataTypeAttribute)
            {
                DataTypeAttribute dataTypeAttribute = (DataTypeAttribute) attribute;
                switch (dataTypeAttribute.DataType)
                {
                    case DataType.Date:
                    case DataType.DateTime:
                        rule = new Rule {Name = "date", Options = true};
                        break;
                    case DataType.Time:
                        rule = new Rule {Name = "time", Options = true};
                        break;
                    case DataType.PhoneNumber:
                        rule = new Rule {Name = "phoneUS", Options = true};
                        break;
                    case DataType.EmailAddress:
                        rule = new Rule {Name = "email", Options = true};
                        break;
                    case DataType.Url:
                        rule = new Rule {Name = "url", Options = true};
                        break;
                    case DataType.Currency:
                    case DataType.Text:
                    case DataType.Html:
                    case DataType.MultilineText:
                    case DataType.Password:
                    case DataType.Duration:
                    case DataType.Custom:
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            if (rule != null)
            {
                rule.Message = attribute.FormatErrorMessage(String.Empty);
            }

            return rule;
        }
    }
}