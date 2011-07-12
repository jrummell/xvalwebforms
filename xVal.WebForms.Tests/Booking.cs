using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace xVal.WebForms.Tests
{
    public class Booking : IValidatableObject
    {
        public const string ClientNameRequiredMessage = "Client name is required";
        public const string NumberOfGuestsRequiredMessage = "Number of guests must be between 1 and 20";
        public const string ArrivalDateRequiredMessage = "Arrival date is required";
        public const string ArrivalDateDataTypeMessage = "The arrival date must be in the form of mm/dd/yyyy";
        public const string DepartureDateDataTypeMessage = "The departure date must be in the form of mm/dd/yyyy";

        [Required(ErrorMessage = ClientNameRequiredMessage)]
        [StringLength(15)]
        public string ClientName { get; set; }

        [Range(1, 20, ErrorMessage = NumberOfGuestsRequiredMessage)]
        public int NumberOfGuests { get; set; }

        [Required(ErrorMessage = ArrivalDateRequiredMessage)]
        [DataType(DataType.Date, ErrorMessage = ArrivalDateDataTypeMessage)]
        public DateTime ArrivalDate { get; set; }

        [DataType(DataType.Date, ErrorMessage = DepartureDateDataTypeMessage)]
        public DateTime? DepartureDate { get; set; }

        #region IValidatableObject Members

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (ArrivalDate == DateTime.MinValue)
            {
                yield return new ValidationResult("Arrival date is required.", new[] {"ArrivalDate"});
            }

            // Business rule: Can't place bookings on Sundays
            if (ArrivalDate.DayOfWeek == DayOfWeek.Sunday)
            {
                yield return new ValidationResult("Bookings are not permitted on Sundays", new[] {"ArrivalDate"});
            }
        }

        #endregion
    }
}