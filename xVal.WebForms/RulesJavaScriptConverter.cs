using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web.Script.Serialization;

namespace xVal.WebForms
{
    /// <summary>
    /// A <see cref="JavaScriptConverter"/> for <see cref="Rule"/> collections.
    /// </summary>
    public class RulesJavaScriptConverter : JavaScriptConverter
    {
        private readonly ReadOnlyCollection<Type> _supportedTypes =
            new ReadOnlyCollection<Type>(new List<Type>(new[] {typeof (RuleCollection)}));

        /// <summary>
        /// Gets a collection of the supported types.
        /// </summary>
        /// <returns>An object that implements <see cref="T:System.Collections.Generic.IEnumerable`1"/> that represents the types supported by the converter.</returns>
        public override IEnumerable<Type> SupportedTypes
        {
            get { return _supportedTypes; }
        }

        /// <summary>
        /// Throws <see cref="NotSupportedException"/>.
        /// </summary>
        /// <param name="dictionary"></param>
        /// <param name="type"></param>
        /// <param name="serializer"></param>
        /// <returns></returns>
        public override object Deserialize(IDictionary<string, object> dictionary, Type type,
                                           JavaScriptSerializer serializer)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Builds a dictionary of name/value pairs.
        /// </summary>
        /// <param name="obj">The object to serialize.</param>
        /// <param name="serializer">The object that is responsible for the serialization.</param>
        /// <returns>
        /// An object that contains key/value pairs that represent the object’s data.
        /// </returns>
        public override IDictionary<string, object> Serialize(object obj, JavaScriptSerializer serializer)
        {
            return Serialize(obj as RuleCollection, serializer);
        }

        /// <summary>
        /// Builds a dictionary of name/value pairs.
        /// </summary>
        /// <param name="rules">The rules to serialize.</param>
        /// <param name="serializer">The object that is responsible for the serialization.</param>
        /// <returns>
        /// An object that contains key/value pairs that represent the object’s data.
        /// </returns>
        public IDictionary<string, object> Serialize(RuleCollection rules, JavaScriptSerializer serializer)
        {
            if (rules == null)
            {
                throw new ArgumentNullException("rules");
            }

            Dictionary<string, object> options =
                rules.ToDictionary<Rule, string, object>(rule => rule.Name, rule => rule.Options);
            Dictionary<string, string> messages =
                rules.ToDictionary(rule => rule.Name, rule => rule.Message);

            Dictionary<string, object> result =
                new Dictionary<string, object>(options) {{"messages", messages}};

            return result;
        }
    }
}