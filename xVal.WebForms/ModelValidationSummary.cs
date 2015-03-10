using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace xVal.WebForms
{
    public class ModelValidationSummary : WebControl, IValidationGroup
    {
        //TODO: ModelValidationSummary client script
        private HtmlGenericControl _errorList;

        /// <summary>
        /// Gets the <see cref="T:System.Web.UI.HtmlTextWriterTag"/> value that corresponds to this Web server control. This property is used primarily by control developers.
        /// </summary>
        /// <returns>One of the <see cref="T:System.Web.UI.HtmlTextWriterTag"/> enumeration values.</returns>
        protected override HtmlTextWriterTag TagKey
        {
            get { return HtmlTextWriterTag.Div; }
        }

        #region IValidationGroup Members

        /// <summary>
        /// Gets or sets the validation group.
        /// </summary>
        /// <value>
        /// The validation group.
        /// </value>
        public string ValidationGroup
        {
            get { return (string) ViewState["ValidationGroup"] ?? String.Empty; }
            set { ViewState["ValidationGroup"] = value; }
        }

        #endregion

        /// <summary>
        /// Called by the ASP.NET page framework to notify server controls that use composition-based implementation to create any child controls they contain in preparation for posting back or rendering.
        /// </summary>
        protected override void CreateChildControls()
        {
            base.CreateChildControls();

            _errorList = new HtmlGenericControl("ul");
            Controls.Add(_errorList);
        }

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.PreRender"/> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> object that contains the event data.</param>
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            EnsureChildControls();

            IEnumerable<IModelValidator> validators = GetErrors();
            foreach (IModelValidator validator in validators)
            {
                HtmlGenericControl errorListItem = new HtmlGenericControl("li") {InnerText = validator.ErrorMessage};
                _errorList.Controls.Add(errorListItem);
            }

            _errorList.Visible = _errorList.HasControls();

            if (_errorList.Visible)
            {
                Style[HtmlTextWriterStyle.Display] = "inline-block";
            }
            else
            {
                Style[HtmlTextWriterStyle.Display] = "none";
            }
        }

        private IEnumerable<IModelValidator> GetErrors()
        {
            return Page.Validators.OfType<IModelValidator>().Where(
                val => String.Compare(ValidationGroup, val.ValidationGroup) == 0
                       && !val.IsValid && !String.IsNullOrEmpty(val.ErrorMessage));
        }
    }
}