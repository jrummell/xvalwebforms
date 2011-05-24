using System;
using System.ComponentModel.DataAnnotations;

namespace xVal.WebForms.Demo
{
    public enum SmokingType
    {
        Smoking,
        NonSmoking
    }

    public class Booking
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
    }
}
