using System;
using System.Collections.Generic;
using System.Linq;
using xVal.ServerSide;

namespace xVal.WebForms.Demo
{
    public static class BookingManager
    {
        public static void PlaceBooking(Booking booking)
        {
            IEnumerable<ErrorInfo> errors = DataAnnotationsValidationRunner.GetErrors(booking);
            if (errors.Any())
            {
                throw new RulesException(errors);
            }

            if (booking.ArrivalDate == DateTime.MinValue)
            {
                throw new RulesException("ArrivalDate", "Arrival date is required.", booking);
            }

            // Business rule: Can't place bookings on Sundays
            if (booking.ArrivalDate.DayOfWeek == DayOfWeek.Sunday)
            {
                throw new RulesException("ArrivalDate", "Bookings are not permitted on Sundays", booking);
            }

            // Todo: save to database or whatever
        }
    }
}