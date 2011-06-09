using System;
using System.Web.UI;

namespace xVal.WebForms
{
    // implementing IEquatable<T> so that NUnit Mocking will verify correctly
    public class ValidationError : Control, IValidator, IEquatable<ValidationError>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationError"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public ValidationError(string message)
        {
            ErrorMessage = message;
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

        #region IValidator Members

        /// <summary>
        /// When implemented by a class, evaluates the condition it checks and updates the <see cref="P:System.Web.UI.IValidator.IsValid"/> property.
        /// </summary>
        public void Validate()
        {
        }

        /// <summary>
        /// When implemented by a class, gets or sets a value indicating whether the user-entered content in the specified control passes validation.
        /// </summary>
        /// <returns>true if the content is valid; otherwise, false.</returns>
        public bool IsValid { get; set; }

        /// <summary>
        /// When implemented by a class, gets or sets the error message text generated when the condition being validated fails.
        /// </summary>
        /// <returns>The error message to generate.</returns>
        public string ErrorMessage { get; set; }

        #endregion

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
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            unchecked
            {
                return (IsValid.GetHashCode()*397) ^ (ErrorMessage != null ? ErrorMessage.GetHashCode() : 0);
            }
        }
    }
}