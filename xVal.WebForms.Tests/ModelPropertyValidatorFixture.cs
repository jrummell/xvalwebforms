using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Moq;
using NUnit.Framework;

namespace xVal.WebForms.Tests
{
    [TestFixture]
    public class ModelPropertyValidatorFixture
    {
        #region Setup/Teardown

        [SetUp]
        public void SetUp()
        {
            _mockValidationRunner = new Mock<IValidationRunner>();
            _mockRuleProvider = new Mock<IValidationRuleProvider>();
            _mockScriptManager = new Mock<IValidationScriptManager>();
            _validator =
                new ModelPropertyValidator(
                    _mockValidationRunner.Object, _mockRuleProvider.Object, _mockScriptManager.Object);
        }

        #endregion

        private ModelPropertyValidator _validator;
        private Mock<IValidationScriptManager> _mockScriptManager;
        private Mock<IValidationRuleProvider> _mockRuleProvider;
        private Mock<IValidationRunner> _mockValidationRunner;

        [Test]
        public void ControlPropertiesValid()
        {
            Assert.That(() => _validator.ControlPropertiesValid(), Throws.InvalidOperationException);

            _validator.ControlToValidate = "txtClientName";
            _validator.ModelType = typeof (Booking).AssemblyQualifiedName;

            Assert.That(() => _validator.ControlPropertiesValid(), Throws.InvalidOperationException);

            _validator.PropertyName = "ClientName";

            Assert.That(_validator.ControlPropertiesValid(), Is.True);
        }

        [Test]
        public void RegisterScripts()
        {
            _validator.ControlToValidate = "txtClientName";
            const string propertyName = "ClientName";
            _validator.PropertyName = propertyName;
            Type bookingType = typeof (Booking);
            _validator.ModelType = bookingType.AssemblyQualifiedName;

            ValidationAttribute[] attributes = new ValidationAttribute[] {new RequiredAttribute()};
            _mockValidationRunner.Setup(
                runner => runner.GetValidators(bookingType, propertyName)).Returns(
                    attributes).Verifiable();

            RuleCollection rules = new RuleCollection(new List<Rule> {new Rule {Name = "required", Options = true}});
            _mockRuleProvider.Setup(provider => provider.GetRules(attributes)).Returns(
                rules).Verifiable();

            _mockScriptManager.Setup(manager => manager.RegisterScripts(rules)).Verifiable();

            _validator.RegisterScripts();

            _mockRuleProvider.Verify();
            _mockValidationRunner.Verify();
            _mockScriptManager.Verify();
        }

        [Test]
        public void RegisterScriptsNoAttributes()
        {
            _validator.ControlToValidate = "txtClientName";
            const string propertyName = "ClientName";
            _validator.PropertyName = propertyName;
            Type bookingType = typeof (Booking);
            _validator.ModelType = bookingType.AssemblyQualifiedName;

            ValidationAttribute[] attributes = new ValidationAttribute[0];
            _mockValidationRunner.Setup(
                runner => runner.GetValidators(bookingType, propertyName)).Returns(
                    attributes).Verifiable();

            _validator.RegisterScripts();

            _mockRuleProvider.Verify(provider => provider.GetRules(attributes), Times.Never());
            _mockValidationRunner.Verify();
            _mockScriptManager.Verify(manager => manager.RegisterScripts(null), Times.Never());
        }

        [Test]
        public void Validate()
        {
            throw new NotImplementedException();
        }
    }
}