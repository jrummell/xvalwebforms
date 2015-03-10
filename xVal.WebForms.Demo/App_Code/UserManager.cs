using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace xVal.WebForms.Demo
{
    public static class UserManager
    {
        public static void LogOn(User user)
        {
            DataAnnotationsValidationRunner validationRunner = new DataAnnotationsValidationRunner();
            IEnumerable<ValidationResult> errors = validationRunner.Validate(user);
            if (errors.Any())
            {
                throw new ValidationException(errors.GetErrorMessage());
            }

            // perform logon
        }
    }
}