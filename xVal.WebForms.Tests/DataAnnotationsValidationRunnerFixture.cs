using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using xVal.ServerSide;

namespace xVal.WebForms.Tests
{
    [TestFixture]
    public class DataAnnotationsValidationRunnerFixture
    {
        [Test]
        public void Booking_GetErrors_ClientName_Null()
        {
            Booking instance = new Booking
                                   {
                                       ArrivalDate = new DateTime(2009, 8, 13),
                                       NumberOfGuests = 2
                                   };

            IEnumerable<ErrorInfo> errors =
                DataAnnotationsValidationRunner.GetErrors(instance);

            Assert.That(errors.Count(), Is.EqualTo(1));

            ErrorInfo errorInfo = errors.First();
            Assert.That(errorInfo.PropertyName, Is.EqualTo("ClientName"));
            Assert.That(errorInfo.ErrorMessage, Is.EqualTo(Booking.ClientNameRequiredMessage));
        }

        [Test]
        public void BookingWithInternalConstructor_GetErrors_ClientName_Null()
        {
            BookingWithInternalConstructor instance = new BookingWithInternalConstructor
            {
                ArrivalDate = new DateTime(2009, 8, 13),
                NumberOfGuests = 2
            };

            IEnumerable<ErrorInfo> errors =
                DataAnnotationsValidationRunner.GetErrors(instance);

            Assert.That(errors.Count(), Is.EqualTo(1));

            ErrorInfo errorInfo = errors.First();
            Assert.That(errorInfo.PropertyName, Is.EqualTo("ClientName"));
            Assert.That(errorInfo.ErrorMessage, Is.EqualTo(Booking.ClientNameRequiredMessage));
        }

        [Test]
        public void Booking_GetErrors_NumberOfGuests_0()
        {
            Booking instance = new Booking
                                   {
                                       ClientName = "John Doe",
                                       ArrivalDate = new DateTime(2009, 8, 13),
                                       NumberOfGuests = 0
                                   };

            IEnumerable<ErrorInfo> errors =
                DataAnnotationsValidationRunner.GetErrors(instance);

            Assert.That(errors.Count(), Is.EqualTo(1));

            ErrorInfo errorInfo = errors.First();
            Assert.That(errorInfo.PropertyName, Is.EqualTo("NumberOfGuests"));
            Assert.That(errorInfo.ErrorMessage, Is.EqualTo(Booking.NumberOfGuestsRequiredMessage));
        }

        [Test]
        public void Booking_GetErrors_Valid()
        {
            Booking instance = new Booking
                                   {
                                       ClientName = "John Doe",
                                       ArrivalDate = new DateTime(2009, 8, 13),
                                       NumberOfGuests = 2
                                   };

            IEnumerable<ErrorInfo> errors =
                DataAnnotationsValidationRunner.GetErrors(instance);

            Assert.That(errors, Is.Empty);
        }

        [Test]
        public void BookingManager_PlaceBooking_Valid()
        {
            Booking instance = new Booking
                                   {
                                       ClientName = "John Doe",
                                       ArrivalDate = new DateTime(2009, 8, 13),
                                       NumberOfGuests = 2
                                   };

            try
            {
                // should not throw a RulesException
                BookingManager.PlaceBooking(instance);
            }
            catch (RulesException)
            {
                Assert.Fail();
            }
        }

        [Test, ExpectedException(typeof(RulesException))]
        public void BookingManager_PlaceBooking_ArrivalDateSunday()
        {
            Booking instance = new Booking
            {
                ClientName = "John Doe",
                ArrivalDate = new DateTime(2009, 8, 9),
                NumberOfGuests = 2
            };

            // will throw RulesException
            BookingManager.PlaceBooking(instance);
        }
    }
}