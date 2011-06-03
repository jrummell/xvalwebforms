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
            _modelPropertyValidator =
                new ModelPropertyValidator(
                    _mockValidationRunner.Object, _mockRuleProvider.Object, _mockScriptManager.Object);
        }

        #endregion

        private ModelPropertyValidator _modelPropertyValidator;
        private Mock<IValidationScriptManager> _mockScriptManager;
        private Mock<IValidationRuleProvider> _mockRuleProvider;
        private Mock<IValidationRunner> _mockValidationRunner;

        [Test]
        public void RegisterScripts()
        {
            _modelPropertyValidator.ControlToValidate = "txtClientName";
            const string propertyName = "ClientName";
            _modelPropertyValidator.PropertyName = propertyName;
            Type bookingType = typeof (Booking);
            _modelPropertyValidator.ModelType = bookingType.AssemblyQualifiedName;

            ValidationAttribute[] attributes = new ValidationAttribute[] {new RequiredAttribute()};
            _mockValidationRunner.Setup(
                runner => runner.GetValidators(bookingType, propertyName)).Returns(
                    attributes).Verifiable();

            RuleCollection rules = new RuleCollection(new List<Rule> {new Rule {Name = "required", Options = true}});
            _mockRuleProvider.Setup(provider => provider.GetRules(attributes)).Returns(
                rules).Verifiable();

            _mockScriptManager.Setup(manager => manager.RegisterScripts(rules)).Verifiable();

            _modelPropertyValidator.RegisterScripts();

            _mockRuleProvider.Verify();
            _mockValidationRunner.Verify();
            _mockScriptManager.Verify();
        }

        [Test]
        public void RegisterScriptsNoAttributes()
        {
            _modelPropertyValidator.ControlToValidate = "txtClientName";
            const string propertyName = "ClientName";
            _modelPropertyValidator.PropertyName = propertyName;
            Type bookingType = typeof (Booking);
            _modelPropertyValidator.ModelType = bookingType.AssemblyQualifiedName;

            ValidationAttribute[] attributes = new ValidationAttribute[0];
            _mockValidationRunner.Setup(
                runner => runner.GetValidators(bookingType, propertyName)).Returns(
                    attributes).Verifiable();

            _modelPropertyValidator.RegisterScripts();

            _mockRuleProvider.Verify(provider => provider.GetRules(attributes), Times.Never());
            _mockValidationRunner.Verify();
            _mockScriptManager.Verify(manager => manager.RegisterScripts(null), Times.Never());
        }
    }
}