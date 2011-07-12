using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace xVal.WebForms
{
    public interface IValidationRuleProvider
    {
        /// <summary>
        /// Gets the rules.
        /// </summary>
        /// <param name="attributes">The attributes.</param>
        /// <returns></returns>
        RuleCollection GetRules(IEnumerable<ValidationAttribute> attributes);
    }
}