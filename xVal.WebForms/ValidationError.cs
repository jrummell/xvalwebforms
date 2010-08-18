using System;
using System.Web.UI.WebControls;

namespace xVal.WebForms
{
    // implementing IEquatable<T> so that NUnit Mocking will verify correctly
    public class ValidationError : BaseValidator, IEquatable<ValidationError>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationError"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public ValidationError(string message)
            : this(message, String.Empty)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationError"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public ValidationError(string message, string validationGroup)
        {
            ErrorMessage = message;
            ValidationGroup = validationGroup;
            IsValid = false;
        }

        #region IEquatable<ValidationError> Members

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
        /// </returns>
        /// <param name="other">An object to compare with this object.
        ///                 </param>
        public bool Equals(ValidationError other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }
            return other.IsValid.Equals(IsValid) && Equals(other.ErrorMessage, ErrorMessage);
        }

        #endregion

        /// <summary>
        /// When overridden in a derived class, this method contains the code to determine whether the value in the input control is valid.
        /// </summary>
        /// <returns>
        /// true if the value in the input control is valid; otherwise, false.
        /// </returns>
        protected override bool EvaluateIsValid()
        {
            return false;
        }

        /// <summary>
        /// Determines whether the control specified by the <see cref="P:System.Web.UI.WebControls.BaseValidator.ControlToValidate"/> property is a valid control.
        /// </summary>
        /// <returns>
        /// true if the control specified by <see cref="P:System.Web.UI.WebControls.BaseValidator.ControlToValidate"/> is a valid control; otherwise, false.
        /// </returns>
        /// <exception cref="T:System.Web.HttpException">
        /// No value is specified for the <see cref="P:System.Web.UI.WebControls.BaseValidator.ControlToValidate"/> property.
        /// - or -
        /// The input control specified by the <see cref="P:System.Web.UI.WebControls.BaseValidator.ControlToValidate"/> property is not found on the page.
        /// - or -
        /// The input control specified by the <see cref="P:System.Web.UI.WebControls.BaseValidator.ControlToValidate"/> property does not have a <see cref="T:System.Web.UI.ValidationPropertyAttribute"/> attribute associated with it; therefore, it cannot be validated with a validation control.
        /// </exception>
        protected override bool ControlPropertiesValid()
        {
            return true;
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object"/> is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object"/> to compare with this instance.</param>
        /// <returns>
        /// 	<c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="T:System.NullReferenceException">
        /// The <paramref name="obj"/> parameter is null.
        /// </exception>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }
            if (ReferenceEquals(this, obj))
            {
                return true;
            }
            if (obj.GetType() != typeof (ValidationError))
            {
                return false;
            }
            return Equals((ValidationError) obj);
        }

        /// <summary>
        /// Serves as a hash function for a particular type. 
        /// </summary>
        /// <returns>
        /// A hash code for the current <see cref="T:System.Object"/>.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        public override int GetHashCode()
        {
            unchecked
            {
                return (IsValid.GetHashCode()*397) ^ (ErrorMessage != null ? ErrorMessage.GetHashCode() : 0);
            }
        }
    }
}