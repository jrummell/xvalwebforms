using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace xVal.WebForms
{
    public class ModelPropertyCollection : Collection<ModelProperty>
    {
        public IDictionary<string, string> ToDictionary()
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>(Count);

            foreach (ModelProperty property in this)
            {
                dictionary.Add(property.PropertyName, property.ControlToValidate);
            }

            return dictionary;
        }
    }
}