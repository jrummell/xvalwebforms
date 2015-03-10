using System;
using System.ComponentModel.DataAnnotations;

namespace xVal.WebForms.Tests
{
    public interface IBookingMetadataType
    {
        [Required(ErrorMessage = Booking.ClientNameRequiredMessage)]
        [StringLength(15)]
        string ClientName { get; set; }

        [Range(1, 20, ErrorMessage = Booking.NumberOfGuestsRequiredMessage)]
        int NumberOfGuests { get; set; }

        [Required(ErrorMessage = Booking.ArrivalDateRequiredMessage)]
        [DataType(DataType.Date, ErrorMessage = Booking.ArrivalDateDataTypeMessage)]
        DateTime ArrivalDate { get; set; }

        [DataType(DataType.Date, ErrorMessage = Booking.DepartureDateDataTypeMessage)]
        DateTime? DepartureDate { get; set; }
    }
}