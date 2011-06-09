using System;
using System.Web.UI.WebControls;
using NUnit.Framework;

namespace xVal.WebForms.Tests
{
    [TestFixture]
    public class ModelValidatorFixture
    {
        #region Setup/Teardown

        [SetUp]
        public void SetUp()
        {
            _validatorCollection = new FakeValidatorCollection();
            _validator = new ModelValidator(_validatorCollection);
        }

        #endregion

        private ModelValidator _validator;
        private IValidatorCollection _validatorCollection;

        [Test]
        public void ControlPropertiesValid()
        {
            Assert.That(() => _validator.ControlPropertiesValid(), Throws.InvalidOperationException);

            _validator.ControlToValidate = "txtClientName";
            _validator.ModelType = typeof (Booking).AssemblyQualifiedName;

            Assert.That(_validator.ControlPropertiesValid(), Is.True);
        }

        [Test]
        public void ValidateInvalid()
        {
            Booking model = new Booking {ArrivalDate = new DateTime(2011, 6, 4)};
            _validator.ModelType = model.GetType().AssemblyQualifiedName;

            // we need a naming container
            LoginView container = new LoginView();
            container.Controls.Add(new ModelPropertyValidator
                                       {
                                           ModelType = _validator.ModelType,
                                           PropertyName = "ArrivalDate",
                                           ControlToValidate = "txtArrivalDate"
                                       });
            container.Controls.Add(_validator);

            _validator.Validate();

            Assert.That(_validator.IsValid, Is.False);
            Assert.That(_validatorCollection.Count, Is.EqualTo(1));
        }

        [Test]
        public void ValidateValid()
        {
            Booking model = new Booking {ArrivalDate = new DateTime(2011, 6, 1)};
            _validator.ModelType = model.GetType().AssemblyQualifiedName;

            // we need a naming container
            LoginView container = new LoginView();
            container.Controls.Add(new ModelPropertyValidator
                                       {
                                           ModelType = _validator.ModelType,
                                           PropertyName = "ArrivalDate",
                                           ControlToValidate = "txtArrivalDate"
                                       });
            container.Controls.Add(new TextBox {ID = "txtArrivalDate", Text = model.ArrivalDate.ToShortDateString()});
            container.Controls.Add(_validator);

            _validator.Validate();

            Assert.That(_validator.IsValid, Is.True);
            Assert.That(_validatorCollection, Is.Empty);
        }
    }
}