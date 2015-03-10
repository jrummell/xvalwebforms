using System;
using System.ComponentModel.DataAnnotations;

namespace xVal.WebForms.Tests
{
    [MetadataType(typeof(IBookingMetadataType))]
    public class BookingWithMetadataType : IBookingMetadataType
    {
        public string ClientName { get; set; }
        public int NumberOfGuests { get; set; }
        public DateTime ArrivalDate { get; set; }
        public DateTime? DepartureDate { get; set; }
    }
}