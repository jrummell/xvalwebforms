using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace xVal.WebForms.Demo
{
    public class Booking : IValidatableObject
    {
        [Required(ErrorMessage = "Client Name is required.")]
        [StringLength(15, ErrorMessage = "Client Name must be 15 characters or less.")]
        public string ClientName { get; set; }

        [Range(1, 20, ErrorMessage = "Number of Guests must be between 1 and 20.")]
        public int NumberOfGuests { get; set; }

        [Required(ErrorMessage = "Arrival Date is required.")]
        [DataType(DataType.Date, ErrorMessage = "Arrival Date must be a valid date.")]
        public DateTime ArrivalDate { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> results = new List<ValidationResult>();

            if (ArrivalDate == DateTime.MinValue)
            {
                results.Add(new ValidationResult("Arrival Date is required.", new[] { "ArrivalDate" }));
            }

            // Business rule: Can't place bookings on Sundays
            if (ArrivalDate.DayOfWeek == DayOfWeek.Sunday)
            {
                results.Add(new ValidationResult("Bookings are not permitted on Sundays.", new[] { "ArrivalDate" }));
            }

            return results;
        }
    }
}
