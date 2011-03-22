using System.ComponentModel.DataAnnotations;
using System.Web.UI.WebControls;
using NUnit.Framework;
using RangeAttribute = System.ComponentModel.DataAnnotations.RangeAttribute;

namespace xVal.WebForms.Tests
{
    [TestFixture]
    public class ValidatorControlFactoryFixture
    {
        [Test]
        public void CreateRegexValidator()
        {
            ValidatorControlFactory factory = new ValidatorControlFactory();
            RegularExpressionAttribute attribute = new RegularExpressionAttribute(@"^.*$");
            BaseValidator control = factory.CreateValidator(attribute);

            Assert.That(control, Is.InstanceOf<RegularExpressionValidator>());

            RegularExpressionValidator controlValidator = (RegularExpressionValidator)control;
            Assert.That(controlValidator.ValidationExpression, Is.EqualTo(attribute.Pattern));
        }

        [Test]
        public void CreateRegexEmailValidator()
        {
            ValidatorControlFactory factory = new ValidatorControlFactory();
            DataTypeAttribute attribute = new DataTypeAttribute(DataType.EmailAddress);
            BaseValidator control = factory.CreateValidator(attribute);

            Assert.That(control, Is.InstanceOf<RegularExpressionValidator>());

            RegularExpressionValidator controlValidator = (RegularExpressionValidator)control;
            Assert.That(controlValidator.ValidationExpression, Is.EqualTo(ValidatorControlFactory.EmailRegex));
        }

        [Test]
        public void CreateRegexPhoneNumberValidator()
        {
            ValidatorControlFactory factory = new ValidatorControlFactory();
            DataTypeAttribute attribute = new DataTypeAttribute(DataType.PhoneNumber);
            BaseValidator control = factory.CreateValidator(attribute);

            Assert.That(control, Is.InstanceOf<RegularExpressionValidator>());

            RegularExpressionValidator controlValidator = (RegularExpressionValidator)control;
            Assert.That(controlValidator.ValidationExpression, Is.EqualTo(ValidatorControlFactory.PhoneNumberRegex));
        }

        [Test]
        public void CreateRegexUrlValidator()
        {
            ValidatorControlFactory factory = new ValidatorControlFactory();
            DataTypeAttribute attribute = new DataTypeAttribute(DataType.Url);
            BaseValidator control = factory.CreateValidator(attribute);

            Assert.That(control, Is.InstanceOf<RegularExpressionValidator>());

            RegularExpressionValidator controlValidator = (RegularExpressionValidator)control;
            Assert.That(controlValidator.ValidationExpression, Is.EqualTo(ValidatorControlFactory.UrlRegex));
        }

        [Test]
        public void CreateRangeValidator()
        {
            ValidatorControlFactory factory = new ValidatorControlFactory();
            RangeAttribute attribute = new RangeAttribute(1, 10);
            BaseValidator control = factory.CreateValidator(attribute);

            Assert.That(control, Is.InstanceOf<RangeValidator>());

            RangeValidator controlValidator = (RangeValidator)control;
            Assert.That(controlValidator.MinimumValue, Is.EqualTo(attribute.Minimum.ToString()));
            Assert.That(controlValidator.MaximumValue, Is.EqualTo(attribute.Maximum.ToString()));
        }
    }
}
