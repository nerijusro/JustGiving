using JG.FinTechTest.Domain.Validators;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using System.ComponentModel.DataAnnotations;

namespace JG.FinTechTest.Tests.ValidatorsTests
{
    public class AmountValidatorTests
    {
        private Mock<IOptionsMonitor<AppSettings.AppSettings>> _optionsMonitorMock;

        public AmountValidatorTests()
        {
            _optionsMonitorMock = new Mock<IOptionsMonitor<AppSettings.AppSettings>>();
        }

        [Test]
        public void ShouldValidateAmountWhenDataIsValid()
        {
            _optionsMonitorMock.Setup(o => o.CurrentValue).Returns(new AppSettings.AppSettings()
            {
                MinimumDonationAmount = "2",
                MaximumDonationAmount = "100000"
            });

            var amountValidationResult = AmountValidator.ValidateAmount(_optionsMonitorMock.Object, 5);

            Assert.AreEqual(ValidationResult.Success, amountValidationResult);
        }

        [Test]
        public void ShouldValidateWhenLowerRangeIsZero()
        {
            _optionsMonitorMock.Setup(o => o.CurrentValue).Returns(new AppSettings.AppSettings()
            {
                MaximumDonationAmount = "100000"
            });

            var amountValidationResult = AmountValidator.ValidateAmount(_optionsMonitorMock.Object, 5);

            Assert.AreEqual(ValidationResult.Success, amountValidationResult);
        }

        [Test]
        public void ShouldReturnValidationErrorWhenAmountIsOutOfRange()
        {
            _optionsMonitorMock.Setup(o => o.CurrentValue).Returns(new AppSettings.AppSettings()
            {
                MinimumDonationAmount = "3",
                MaximumDonationAmount = "100000"
            });

            var expectedValidationError = new ValidationResult("Donation amount can not be smaller than 3 and can not be larger than 100000");

            var amountValidationResult = AmountValidator.ValidateAmount(_optionsMonitorMock.Object, 1);

            Assert.AreEqual(expectedValidationError.ErrorMessage, amountValidationResult.ErrorMessage);
        }

        [Test]
        public void ShouldReturnValidationErrorWhenAmountIsOutOfRangeButMaximumAmountIsNotDefined()
        {
            _optionsMonitorMock.Setup(o => o.CurrentValue).Returns(new AppSettings.AppSettings()
            {
                MinimumDonationAmount = "3"
            });

            var expectedValidationError = new ValidationResult("Donation amount can not be smaller than 3");

            var amountValidationResult = AmountValidator.ValidateAmount(_optionsMonitorMock.Object, 1);

            Assert.AreEqual(expectedValidationError.ErrorMessage, amountValidationResult.ErrorMessage);
        }
    }
}
