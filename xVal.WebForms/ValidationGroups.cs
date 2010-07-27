using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace xVal.WebForms
{
    public class ValidationGroups
    {
        private readonly IDictionary<string, IDictionary<string, IList<string>>> _valgroups;

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationGroups"/> class.
        /// </summary>
        /// <param name="buttons">The buttons.</param>
        /// <param name="textBoxes">The text boxes.</param>
        public ValidationGroups(IEnumerable<IButtonControl> buttons, IEnumerable<IEditableTextControl> textBoxes)
        {
            _valgroups = new Dictionary<string, IDictionary<string, IList<string>>>();

            foreach (IButtonControl control in buttons.Where(b => b.CausesValidation))
            {
                if (String.IsNullOrEmpty(control.ValidationGroup))
                {
                    continue;
                }

                if (!_valgroups.Keys.Contains(control.ValidationGroup))
                {
                    _valgroups.Add(control.ValidationGroup, new Dictionary<string, IList<string>>());
                    _valgroups[control.ValidationGroup].Add("buttons", new List<string>());
                    _valgroups[control.ValidationGroup].Add("fields", new List<string>());
                }

                _valgroups[control.ValidationGroup]["buttons"].Add(((Control) control).GetJQueryName());

                IButtonControl buttonControl = control;
                _valgroups[control.ValidationGroup]["fields"] =
                    _valgroups[control.ValidationGroup]["fields"].Union(
                        textBoxes.Where(t => CheckValidationGroup(t, buttonControl.ValidationGroup))
                            .Select(t => ((Control) t).GetJQueryName()).ToList()).ToList();
            }
        }

        /// <summary>
        /// Gets the groups.
        /// </summary>
        /// <value>The groups.</value>
        public IDictionary<string, IDictionary<string, IList<string>>> Groups
        {
            get { return _valgroups; }
        }

        private static bool CheckValidationGroup(IEditableTextControl textControl, string validationGroup)
        {
            TextBox textBox = textControl as TextBox;
            if (textBox != null)
            {
                return textBox.ValidationGroup == validationGroup;
            }

            ListControl listControl = textControl as ListControl;
            if (listControl != null)
            {
                return listControl.ValidationGroup == validationGroup;
            }

            return false;
        }
    }
}