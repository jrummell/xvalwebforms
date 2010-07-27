using System;

namespace xVal.WebForms
{
    /// <summary>
    /// This is the same class the internal  xVal.Html.Options.ValidationSummaryOptions in xVal 1.0
    /// </summary>
    internal class ValidationSummaryOptions
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationSummaryOptions"/> class.
        /// </summary>
        /// <param name="elementId">The element id.</param>
        /// <param name="headerMessage">The header message.</param>
        public ValidationSummaryOptions(string elementId, string headerMessage)
        {
            if (string.IsNullOrEmpty(elementId))
                throw new ArgumentException("Cannot be null or empty", "elementId");

            if (headerMessage == string.Empty)
                throw new ArgumentException(
                    "headerMessage cannot be empty (pass null if you don't want to display a header message)");

            ElementID = elementId;
            HeaderMessage = headerMessage;
        }

        /// <summary>
        /// Gets or sets the element ID.
        /// </summary>
        /// <value>The element ID.</value>
        public string ElementID { get; private set; }

        /// <summary>
        /// Gets or sets the header message.
        /// </summary>
        /// <value>The header message.</value>
        public string HeaderMessage { get; private set; }
    }
}