using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using NUnit.Framework;

namespace xVal.WebForms.Tests
{
    [TestFixture]
    public class DataAnnotationsValidationRunnerMetaDataTypeFixture
    {
        private IValidationRunner _validationRunner;

        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            // this required in non-MVC, RIA, DynamicData environments for MetaDataType to work with Validator
            TypeDescriptor.AddProviderTransparent(
                new AssociatedMetadataTypeTypeDescriptionProvider(typeof (BookingWithMetadataType), typeof (IBookingMetadataType)),
                typeof (BookingWithMetadataType));
        }

        [SetUp]
        public void SetUp()
        {
            _validationRunner = new DataAnnotationsValidationRunner();
        }

        [Test]
        public void Booking_GetErrors_ClientName_Null()
        {
            BookingWithMetadataType instance = new BookingWithMetadataType
                                   {
                                       ArrivalDate = new DateTime(2009, 8, 13),
                                       NumberOfGuests = 2
                                   };

            IEnumerable<ValidationResult> errors = _validationRunner.Validate(instance);
            List<ValidationResult> validationResults = new List<ValidationResult>();
            Validator.TryValidateObject(instance, new ValidationContext(instance, null, null), validationResults);

            Assert.That(errors.Count(), Is.EqualTo(1));
            Assert.That(validationResults.Count, Is.EqualTo(1));

            ValidationResult errorInfo = errors.First();
            Assert.That(errorInfo.MemberNames.Contains("ClientName"));
            Assert.That(errorInfo.ErrorMessage, Is.EqualTo(Booking.ClientNameRequiredMessage));
        }

        [Test]
        public void Booking_GetErrors_NumberOfGuests_0()
        {
            BookingWithMetadataType instance = new BookingWithMetadataType
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
            BookingWithMetadataType instance = new BookingWithMetadataType
                                   {
                                       ClientName = "John Doe",
                                       ArrivalDate = new DateTime(2009, 8, 13),
                                       NumberOfGuests = 2
                                   };

            IEnumerable<ValidationResult> errors = _validationRunner.Validate(instance);

            Assert.That(errors, Is.Empty);
        }
    }
}