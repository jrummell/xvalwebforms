using System;
using System.Web.UI;

namespace xVal.WebForms
{
    public interface IValidatorCollection
    {
        /// <summary>
        /// Adds the specified validator.
        /// </summary>
        /// <param name="validator">The validator.</param>
        void Add(IValidator validator);
    }

    internal class PageValidatorCollection : IValidatorCollection
    {
        private readonly Control _control;

        /// <summary>
        /// Initializes a new instance of the <see cref="PageValidatorCollection"/> class.
        /// </summary>
        /// <param name="control">The control.</param>
        public PageValidatorCollection(Control control)
        {
            if (control == null)
            {
                throw new ArgumentNullException("control");
            }

            _control = control;
        }

        #region IValidatorCollection Members

        /// <summary>
        /// Adds the specified validator.
        /// </summary>
        /// <param name="validator">The validator.</param>
        public void Add(IValidator validator)
        {
            _control.Page.Validators.Add(validator);
        }

        #endregion
    }
}