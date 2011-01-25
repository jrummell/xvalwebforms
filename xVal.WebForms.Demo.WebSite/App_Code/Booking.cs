using System;
using System.ComponentModel.DataAnnotations;

namespace xVal.WebForms.Demo
{
    public class Booking
    {
        [Required]
        [StringLength(15)]
        public string ClientName { get; set; }

        [Range(1, 20)]
        public int NumberOfGuests { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime ArrivalDate { get; set; }
    }
}
