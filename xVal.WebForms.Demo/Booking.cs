using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace xVal.WebForms.Demo
{
    public enum SmokingType
    {
        Smoking,
        NonSmoking
    }

    public class Booking : IValidatableObject
    {
        [Required(ErrorMessage = "Client name is required.")]
        [StringLength(15)]
        public string ClientName { get; set; }

        [Range(1, 20)]
        public int NumberOfGuests { get; set; }

        [Required(ErrorMessage = "Arrival date is required.")]
        [DataType(DataType.Date)]
        public DateTime ArrivalDate { get; set; }

        [Required(ErrorMessage = "Smoking type is requried.")]
        public SmokingType SmokingType { get; set; }

        [Required(ErrorMessage = "Email address is required.")]
        [DataType(DataType.EmailAddress)]
        public string EmailAddress { get; set; }

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