using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using NUnit.Framework;
using NUnit.Mocks;

namespace xVal.WebForms.Tests
{
    [TestFixture]
    public class ModelValidatorFixture
    {
        [Test]
        public void EvaluateIsValid_BookingWithInternalConstructor_NotValid()
        {
            DynamicMock controlValueResolverMock = new DynamicMock(typeof (IControlValueResolver));
            controlValueResolverMock.ExpectAndReturn("GetControlValue", String.Empty, "txtClientName");
            controlValueResolverMock.ExpectAndReturn("GetControlValue", String.Empty, "txtNumberOfGuests");
            controlValueResolverMock.ExpectAndReturn("GetControlValue", "01/01/2009", "txtArrivalDate");

            DynamicMock validatorsMock = new DynamicMock(typeof (IValidatorCollection));
            validatorsMock.Expect("Add", new ValidationError("ClientName: " + Booking.ClientNameRequiredMessage));
            validatorsMock.Expect("Add", new ValidationError("NumberOfGuests: " + Booking.NumberOfGuestsRequiredMessage));

            DynamicMock scriptManagerMock = new DynamicMock(typeof (IValidationScriptManager));
            scriptManagerMock.Expect("SupressValidation");
            scriptManagerMock.Expect("RegisterValidationScript");

            ModelValidator validator = new ModelValidator(
                (IControlValueResolver) controlValueResolverMock.MockInstance,
                (IValidatorCollection) validatorsMock.MockInstance,
                (IValidationScriptManager) scriptManagerMock.MockInstance)
                                           {
                                               ModelType = typeof (BookingWithInternalConstructor).AssemblyQualifiedName
                                           };

            validator.ModelProperties.Add(new ModelProperty
                                              {ControlToValidate = "txtClientName", PropertyName = "ClientName"});
            validator.ModelProperties.Add(new ModelProperty
                                              {ControlToValidate = "txtNumberOfGuests", PropertyName = "NumberOfGuests"});
            validator.ModelProperties.Add(new ModelProperty
                                              {ControlToValidate = "txtArrivalDate", PropertyName = "ArrivalDate"});

            validator.Validate();

            controlValueResolverMock.Verify();
            validatorsMock.Verify();

            Assert.That(!validator.IsValid);
        }

        [Test]
        public void EvaluateIsValid_BookingWithInternalConstructor_Valid()
        {
            DynamicMock controlValueResolverMock = new DynamicMock(typeof (IControlValueResolver));
            controlValueResolverMock.ExpectAndReturn("GetControlValue", "Joe Shmoe", "txtClientName");
            controlValueResolverMock.ExpectAndReturn("GetControlValue", "5", "txtNumberOfGuests");
            controlValueResolverMock.ExpectAndReturn("GetControlValue", "01/01/2009", "txtArrivalDate");

            DynamicMock validatorsMock = new DynamicMock(typeof (IValidatorCollection));

            DynamicMock scriptManagerMock = new DynamicMock(typeof (IValidationScriptManager));
            scriptManagerMock.Expect("SupressValidation");
            scriptManagerMock.Expect("RegisterValidationScript");

            ModelValidator validator = new ModelValidator(
                (IControlValueResolver) controlValueResolverMock.MockInstance,
                (IValidatorCollection) validatorsMock.MockInstance,
                (IValidationScriptManager) scriptManagerMock.MockInstance)
                                           {
                                               ModelType = typeof (BookingWithInternalConstructor).AssemblyQualifiedName
                                           };

            validator.ModelProperties.Add(new ModelProperty
                                              {ControlToValidate = "txtClientName", PropertyName = "ClientName"});
            validator.ModelProperties.Add(new ModelProperty
                                              {ControlToValidate = "txtNumberOfGuests", PropertyName = "NumberOfGuests"});
            validator.ModelProperties.Add(new ModelProperty
                                              {ControlToValidate = "txtArrivalDate", PropertyName = "ArrivalDate"});

            validator.Validate();

            controlValueResolverMock.Verify();
            validatorsMock.Verify();

            Assert.That(validator.IsValid);
        }

        [Test]
        public void EvaluateIsValid_Booking_NotValid()
        {
            DynamicMock controlValueResolverMock = new DynamicMock(typeof (IControlValueResolver));
            controlValueResolverMock.ExpectAndReturn("GetControlValue", String.Empty, "txtClientName");
            controlValueResolverMock.ExpectAndReturn("GetControlValue", String.Empty, "txtNumberOfGuests");
            controlValueResolverMock.ExpectAndReturn("GetControlValue", "01/01/2009", "txtArrivalDate");

            DynamicMock validatorsMock = new DynamicMock(typeof (IValidatorCollection));
            validatorsMock.Expect("Add", new ValidationError("ClientName: " + Booking.ClientNameRequiredMessage));
            validatorsMock.Expect("Add", new ValidationError("NumberOfGuests: " + Booking.NumberOfGuestsRequiredMessage));

            DynamicMock scriptManagerMock = new DynamicMock(typeof (IValidationScriptManager));
            scriptManagerMock.Expect("SupressValidation");
            scriptManagerMock.Expect("RegisterValidationScript");

            ModelValidator validator = new ModelValidator(
                (IControlValueResolver) controlValueResolverMock.MockInstance,
                (IValidatorCollection) validatorsMock.MockInstance,
                (IValidationScriptManager) scriptManagerMock.MockInstance)
                                           {
                                               ModelType = typeof (BookingWithInternalConstructor).AssemblyQualifiedName
                                           };

            validator.ModelProperties.Add(new ModelProperty
                                              {ControlToValidate = "txtClientName", PropertyName = "ClientName"});
            validator.ModelProperties.Add(new ModelProperty
                                              {ControlToValidate = "txtNumberOfGuests", PropertyName = "NumberOfGuests"});
            validator.ModelProperties.Add(new ModelProperty
                                              {ControlToValidate = "txtArrivalDate", PropertyName = "ArrivalDate"});

            validator.Validate();

            controlValueResolverMock.Verify();
            validatorsMock.Verify();

            Assert.That(!validator.IsValid);
        }

        [Test]
        public void EvaluateIsValid_Booking_Valid()
        {
            DynamicMock controlValueResolverMock = new DynamicMock(typeof (IControlValueResolver));
            controlValueResolverMock.ExpectAndReturn("GetControlValue", "Joe Shmoe", "txtClientName");
            controlValueResolverMock.ExpectAndReturn("GetControlValue", "5", "txtNumberOfGuests");
            controlValueResolverMock.ExpectAndReturn("GetControlValue", "01/01/2009", "txtArrivalDate");
            controlValueResolverMock.ExpectAndReturn("GetControlValue", "01/01/2010", "txtDepartureDate");

            DynamicMock validatorsMock = new DynamicMock(typeof (IValidatorCollection));

            DynamicMock scriptManagerMock = new DynamicMock(typeof (IValidationScriptManager));
            scriptManagerMock.Expect("SupressValidation");
            scriptManagerMock.Expect("RegisterValidationScript");

            ModelValidator validator = new ModelValidator(
                (IControlValueResolver) controlValueResolverMock.MockInstance,
                (IValidatorCollection) validatorsMock.MockInstance,
                (IValidationScriptManager) scriptManagerMock.MockInstance)
                                           {
                                               ModelType = typeof (BookingWithInternalConstructor).AssemblyQualifiedName
                                           };

            validator.ModelProperties.Add(new ModelProperty
                                              {ControlToValidate = "txtClientName", PropertyName = "ClientName"});
            validator.ModelProperties.Add(new ModelProperty
                                              {ControlToValidate = "txtNumberOfGuests", PropertyName = "NumberOfGuests"});
            validator.ModelProperties.Add(new ModelProperty
                                              {ControlToValidate = "txtArrivalDate", PropertyName = "ArrivalDate"});
            validator.ModelProperties.Add(new ModelProperty
                                              {ControlToValidate = "txtDepartureDate", PropertyName = "DepartureDate"});

            validator.Validate();

            controlValueResolverMock.Verify();
            validatorsMock.Verify();

            Assert.That(validator.IsValid);
        }

        [Test]
        public void RegisterValidationScript_EnableClientScript_False()
        {
            DynamicMock controlValueResolverMock = new DynamicMock(typeof (IControlValueResolver));
            DynamicMock validatorsMock = new DynamicMock(typeof (IValidatorCollection));

            DynamicMock scriptManagerMock = new DynamicMock(typeof (IValidationScriptManager));
            scriptManagerMock.ExpectNoCall("SupressValidation");
            scriptManagerMock.ExpectNoCall("RegisterValidationScript");

            Page page = new Page();

            TextBox txtClientName = new TextBox {ID = "txtClientName"};
            TextBox txtNumberOfGuests = new TextBox {ID = "txtNumberOfGuests"};
            TextBox txtArrivalDate = new TextBox {ID = "txtArrivalDate"};

            page.Controls.Add(txtClientName);
            page.Controls.Add(txtNumberOfGuests);
            page.Controls.Add(txtArrivalDate);

            ModelValidator validator = new ModelValidator(
                (IControlValueResolver) controlValueResolverMock.MockInstance,
                (IValidatorCollection) validatorsMock.MockInstance,
                (IValidationScriptManager) scriptManagerMock.MockInstance)
                                           {
                                               ModelType = typeof (BookingWithInternalConstructor).AssemblyQualifiedName,
                                               EnableClientScript = false
                                           };

            page.Controls.Add(validator);

            validator.ModelProperties.Add(new ModelProperty
                                              {ControlToValidate = "txtClientName", PropertyName = "ClientName"});
            validator.ModelProperties.Add(new ModelProperty
                                              {ControlToValidate = "txtNumberOfGuests", PropertyName = "NumberOfGuests"});
            validator.ModelProperties.Add(new ModelProperty
                                              {ControlToValidate = "txtArrivalDate", PropertyName = "ArrivalDate"});

            validator.RegisterValidationScriptIfClientScriptEnabled();

            controlValueResolverMock.Verify();
            validatorsMock.Verify();
        }

        [Test]
        public void RegisterValidationScript_EnableClientScript_True()
        {
            DynamicMock controlValueResolverMock = new DynamicMock(typeof (IControlValueResolver));
            DynamicMock validatorsMock = new DynamicMock(typeof (IValidatorCollection));

            DynamicMock scriptManagerMock = new DynamicMock(typeof (IValidationScriptManager));
            scriptManagerMock.Expect("SupressValidation");
            scriptManagerMock.Expect("RegisterValidationScript");

            Page page = new Page();

            TextBox txtClientName = new TextBox {ID = "txtClientName"};
            TextBox txtNumberOfGuests = new TextBox {ID = "txtNumberOfGuests"};
            TextBox txtArrivalDate = new TextBox {ID = "txtArrivalDate"};

            page.Controls.Add(txtClientName);
            page.Controls.Add(txtNumberOfGuests);
            page.Controls.Add(txtArrivalDate);

            ModelValidator validator = new ModelValidator(
                (IControlValueResolver) controlValueResolverMock.MockInstance,
                (IValidatorCollection) validatorsMock.MockInstance,
                (IValidationScriptManager) scriptManagerMock.MockInstance)
                                           {
                                               ModelType = typeof (BookingWithInternalConstructor).AssemblyQualifiedName,
                                               EnableClientScript = true
                                           };

            page.Controls.Add(validator);

            validator.ModelProperties.Add(new ModelProperty
                                              {ControlToValidate = "txtClientName", PropertyName = "ClientName"});
            validator.ModelProperties.Add(new ModelProperty
                                              {ControlToValidate = "txtNumberOfGuests", PropertyName = "NumberOfGuests"});
            validator.ModelProperties.Add(new ModelProperty
                                              {ControlToValidate = "txtArrivalDate", PropertyName = "ArrivalDate"});

            validator.RegisterValidationScriptIfClientScriptEnabled();

            controlValueResolverMock.Verify();
            validatorsMock.Verify();
        }
    }
}