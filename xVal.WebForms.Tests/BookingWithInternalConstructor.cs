using System;
using System.ComponentModel.DataAnnotations;

namespace xVal.WebForms.Tests
{
    public class BookingWithInternalConstructor
    {
        internal BookingWithInternalConstructor()
        {
            ClientName = String.Empty;
            NumberOfGuests = 0;
            ArrivalDate = DateTime.MinValue;
        }

        [Required(ErrorMessage = "Client name is required")]
        [StringLength(15)]
        public string ClientName { get; set; }

        [Range(1, 20, ErrorMessage = "Number of guests must be between 1 and 20")]
        public int NumberOfGuests { get; set; }

        [Required(ErrorMessage = "Arrival date is required")]
        [DataType(DataType.Date, ErrorMessage = "The arrival date must be in the form of mm/dd/yyyy")]
        public DateTime ArrivalDate { get; set; }
    }
}