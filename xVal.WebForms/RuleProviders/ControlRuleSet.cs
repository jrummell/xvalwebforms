using System;
using System.Collections.Generic;
using System.Linq;
using xVal.RuleProviders;
using xVal.Rules;

namespace xVal.WebForms.RuleProviders
{
    public class ControlRuleSet : RuleSet
    {
        private readonly ILookup<ControlRuleSetKey, Rule> rules;

        public ControlRuleSet(ILookup<ControlRuleSetKey, Rule> rules)
            : base(rules.ToLookup(x => x.Key.FieldName, x => rules[x.Key].First()))
        {
            if (rules == null) throw new ArgumentNullException("rules");
            this.rules = rules;
        }

        public IEnumerable<Rule> this[ControlRuleSetKey key]
        {
            get { return rules[key]; }
        }

        public new IEnumerable<ControlRuleSetKey> Keys
        {
            get { return rules.Select(x => x.Key); }
        }

        public bool Contains(ControlRuleSetKey key)
        {
            return rules.Contains(key);
        }
    }
}