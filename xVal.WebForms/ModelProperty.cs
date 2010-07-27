namespace xVal.WebForms
{
    public class ModelProperty
    {
        /// <summary>
        /// Gets or sets the name of the property.
        /// </summary>
        /// <value>The name of the property.</value>
        public string PropertyName { get; set; }

        /// <summary>
        /// Gets or sets the control to validate.
        /// </summary>
        /// <value>The control to validate.</value>
        public string ControlToValidate { get; set; }
    }
}