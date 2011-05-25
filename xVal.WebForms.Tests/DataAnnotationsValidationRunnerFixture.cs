using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using NUnit.Framework;

namespace xVal.WebForms.Tests
{
    [TestFixture]
    public class DataAnnotationsValidationRunnerFixture
    {
        private IValidationRunner _validationRunner;

        [SetUp]
        public void SetUp()
        {
            _validationRunner = new DataAnnotationsValidationRunner();
        }

        [Test]
        public void Booking_GetErrors_ClientName_Null()
        {
            Booking instance = new Booking
                                   {
                                       ArrivalDate = new DateTime(2009, 8, 13),
                                       NumberOfGuests = 2
                                   };

            IEnumerable<ValidationResult> errors = _validationRunner.Validate(instance);

            Assert.That(errors.Count(), Is.EqualTo(1));

            ValidationResult errorInfo = errors.First();
            Assert.That(errorInfo.MemberNames.Contains("ClientName"));
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

            IEnumerable<ValidationResult> errors = _validationRunner.Validate(instance);

            Assert.That(errors.Count(), Is.EqualTo(1));

            ValidationResult errorInfo = errors.First();
            Assert.That(errorInfo.MemberNames.Contains("ClientName"));
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

            IEnumerable<ValidationResult> errors = _validationRunner.Validate(instance);

            Assert.That(errors.Count(), Is.EqualTo(1));

            ValidationResult errorInfo = errors.First();
            Assert.That(errorInfo.MemberNames.Contains("NumberOfGuests"));
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

            IEnumerable<ValidationResult> errors = _validationRunner.Validate(instance);

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
                // should not throw a ValidationException
                BookingManager.PlaceBooking(instance);
            }
            catch (ValidationException)
            {
                Assert.Fail();
            }
        }

        [Test, ExpectedException(typeof(ValidationException))]
        public void BookingManager_PlaceBooking_ArrivalDateSunday()
        {
            Booking instance = new Booking
            {
                ClientName = "John Doe",
                ArrivalDate = new DateTime(2009, 8, 9),
                NumberOfGuests = 2
            };

            // will throw ValidationException
            BookingManager.PlaceBooking(instance);
        }
    }
}