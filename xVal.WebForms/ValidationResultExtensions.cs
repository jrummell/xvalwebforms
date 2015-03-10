using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace xVal.WebForms
{
    public static class ValidationResultExtensions
    {
        /// <summary>
        /// Gets the combined error messages for all <see cref="ValidationResult"/>s.
        /// </summary>
        /// <param name="collection">The collection.</param>
        /// <returns></returns>
        public static string GetErrorMessage(this IEnumerable<ValidationResult> collection)
        {
            StringBuilder message = new StringBuilder();
            foreach (ValidationResult result in collection)
            {
                message.Append(result.ErrorMessage).Append(" ");
            }

            return message.ToString();
        }
    }
}