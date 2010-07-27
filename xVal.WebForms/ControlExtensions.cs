using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace xVal.WebForms
{
    internal static class ControlExtensions
    {
        /// <summary>
        /// Finds the control.
        /// </summary>
        /// <param name="parent">The parent.</param>
        /// <param name="controlID">The control ID.</param>
        /// <returns></returns>
        public static Control FindControlRecursive(this Control parent, string controlID)
        {
            Control current = parent;
            LinkedList<Control> controlList = new LinkedList<Control>();

            while (current != null)
            {
                if (current.ID == controlID)
                {
                    return current;
                }

                foreach (Control child in current.Controls)
                {
                    if (child.ID == controlID)
                    {
                        return child;
                    }
                    if (child.HasControls())
                    {
                        controlList.AddLast(child);
                    }
                }

                if (controlList.Count == 0)
                {
                    return null;
                }

                current = controlList.First.Value;
                controlList.Remove(current);
            }

            return null;
        }

        /// <summary>
        /// Gets the JQuery name of the given control.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <returns></returns>
        public static string GetJQueryName(this Control control)
        {
            return control.ClientID.Replace("_", "$");
        }

        /// <summary>
        /// Gets the controls.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="control">The control.</param>
        /// <param name="controls">The controls.</param>
        /// <returns></returns>
        public static IEnumerable<T> GetControls<T>(this Control control, IList<T> controls)
        {
            foreach (object child in control.Controls)
            {
                if (child is T)
                {
                    controls.Add((T) child);
                }

                Control childControl = child as Control;

                if (childControl != null)
                {
                    if (childControl.HasControls())
                    {
                        controls.Union(GetControls(childControl, controls));
                    }
                }
            }

            return controls;
        }

        /// <summary>
        /// Gets the buttons inside the given control.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <returns></returns>
        public static IEnumerable<IButtonControl> GetButtons(this Control control)
        {
            return GetControls(control, new List<IButtonControl>());
        }

        /// <summary>
        /// Gets the text boxes inside the given control.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <returns></returns>
        public static IEnumerable<IEditableTextControl> GetTextBoxes(this Control control)
        {
            return GetControls(control, new List<IEditableTextControl>());
        }
    }
}