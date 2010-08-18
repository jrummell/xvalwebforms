using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace xVal.WebForms
{
    public interface IControlValueResolver
    {
        /// <summary>
        /// Gets value of the control with the given ID.
        /// </summary>
        /// <param name="controlId">The control id.</param>
        /// <returns></returns>
        string GetControlValue(string controlId);
    }

    internal class ControlValueResolver : IControlValueResolver
    {
        private readonly Control _control;

        public ControlValueResolver(Control control)
        {
            if (control == null)
            {
                throw new ArgumentNullException("control");
            }

            _control = control;
        }

        #region IControlValueResolver Members

        /// <summary>
        /// Gets value of the control with the given ID.
        /// </summary>
        /// <param name="controlId">The control id.</param>
        /// <returns></returns>
        public string GetControlValue(string controlId)
        {
            Control control = _control.Parent.FindControl(controlId);

            if (control == null)
            {
                return null;
            }

            ListControl listControl = control as ListControl;
            if (listControl != null)
            {
                return listControl.SelectedValue;
            }

            ITextControl textControl = control as ITextControl;
            if (textControl != null)
            {
                return textControl.Text;
            }

            return null;
        }

        #endregion
    }
}