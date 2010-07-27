using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Script.Serialization;
using xVal.Html;
using xVal.RuleProviders;
using xVal.Rules;
using xVal.WebForms.RuleProviders;

namespace xVal.WebForms.Html
{
    public class ControlJsonValidationConfigFormatter : IValidationConfigFormatter
    {
        private static readonly JavaScriptSerializer Serializer = new JavaScriptSerializer();
        private readonly IDictionary<string, string> _controlMapping;

        /// <summary>
        /// Initializes the <see cref="ControlJsonValidationConfigFormatter"/> class.
        /// </summary>
        static ControlJsonValidationConfigFormatter()
        {
            Serializer.RegisterConverters(new JavaScriptConverter[] {new ControlRuleSetConverter(), new RuleConverter()});
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ControlJsonValidationConfigFormatter"/> class.
        /// </summary>
        /// <param name="controlMapping">The control mapping.</param>
        public ControlJsonValidationConfigFormatter(IDictionary<string, string> controlMapping)
        {
            if (controlMapping == null)
            {
                throw new ArgumentNullException("controlMapping");
            }
            _controlMapping = controlMapping;
        }

        #region IValidationConfigFormatter Members

        /// <summary>
        /// Formats the rules.
        /// </summary>
        /// <param name="rules">The rules.</param>
        /// <returns></returns>
        public string FormatRules(RuleSet rules)
        {
            var allRules =
                (from key in rules.Keys
                 from rule in rules[key]
                 join property in _controlMapping on key equals property.Key
                 select new
                            {
                                Key = new ControlRuleSetKey {ControlID = property.Value, FieldName = property.Key},
                                Rule = rule
                            }
                );

            ControlRuleSet controlRuleSet = new ControlRuleSet(allRules.ToLookup(x => x.Key, x => x.Rule));

            return FormatRules(controlRuleSet);
        }

        #endregion

        /// <summary>
        /// Formats the rules.
        /// </summary>
        /// <param name="rules">The rules.</param>
        /// <returns></returns>
        public string FormatRules(ControlRuleSet rules)
        {
            return Serializer.Serialize(rules);
        }

        #region Nested type: ControlRuleSetConverter

        private class ControlRuleSetConverter : JavaScriptConverter
        {
            public override IEnumerable<Type> SupportedTypes
            {
                get { return new[] {typeof (ControlRuleSet)}; }
            }

            public override object Deserialize(IDictionary<string, object> dictionary, Type type,
                                               JavaScriptSerializer serializer)
            {
                throw new NotImplementedException();
            }

            public override IDictionary<string, object> Serialize(object obj, JavaScriptSerializer serializer)
            {
                ControlRuleSet ruleSet = obj as ControlRuleSet;
                if (ruleSet == null)
                {
                    throw new ArgumentException("obj must be of type ControlRuleSet");
                }

                return new Dictionary<string, object>
                           {
                               {
                                   "Fields",
                                   ruleSet.Keys.Select(x => new
                                                                {
                                                                    x.FieldName,
                                                                    x.ControlID,
                                                                    FieldRules = ruleSet[x].ToArray()
                                                                }).ToArray()
                                   }
                           };
            }
        }

        #endregion

        #region Nested type: RuleConverter

        private class RuleConverter : JavaScriptConverter
        {
            public override IEnumerable<Type> SupportedTypes
            {
                get { return new[] {typeof (Rule)}; }
            }

            public override object Deserialize(IDictionary<string, object> dictionary, Type type,
                                               JavaScriptSerializer serializer)
            {
                throw new NotImplementedException();
            }

            public override IDictionary<string, object> Serialize(object obj, JavaScriptSerializer serializer)
            {
                Rule rule = obj as Rule;
                if (rule == null)
                {
                    throw new ArgumentException("obj must be of type RouteBase");
                }

                Dictionary<string, object> result = new Dictionary<string, object>
                                                        {
                                                            {"RuleName", rule.RuleName},
                                                            {"RuleParameters", rule.ListParameters()}
                                                        };
                string errorMessage = rule.ErrorMessageOrResourceString;
                if (errorMessage != null)
                {
                    result.Add("Message", errorMessage);
                }
                return result;
            }
        }

        #endregion
    }
}