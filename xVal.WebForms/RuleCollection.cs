using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace xVal.WebForms
{
    /// <summary>
    /// Strongly typed collection of <see cref="Rule"/>s.
    /// </summary>
    public class RuleCollection : Collection<Rule>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RuleCollection"/> class.
        /// </summary>
        /// <param name="rules">The rules.</param>
        public RuleCollection(IList<Rule> rules)
            : base(rules)
        {
        }
    }
}