using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace xVal.WebForms.Demo
{
    public static class BookingManager
    {
        public static void PlaceBooking(Booking booking)
        {
            DataAnnotationsValidationRunner validationRunner = new DataAnnotationsValidationRunner();
            IEnumerable<ValidationResult> errors = validationRunner.Validate(booking);
            if (errors.Any())
            {
                throw new ValidationException(errors.GetErrorMessage());
            }

            // Todo: save to database or whatever
        }
    }
}