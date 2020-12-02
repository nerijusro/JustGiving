using JG.FinTechTest.Domain.Utils;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using JG.FinTechTest.Options;

namespace JG.FinTechTest.Tests.ValidatorsTests
{
    public class AppSettingsValidatorTests
    {
        private Mock<IOptionsMonitor<AppSettings>> _optionsMonitorMock;

        public AppSettingsValidatorTests()
        {
            _optionsMonitorMock = new Mock<IOptionsMonitor<AppSettings>>();
        }

        [Test]
        public void ShouldValidateSuccessfulyDonationAmountRange()
        {
            var donationAmountValidationResult = AppSettingsValidator.ValidateDonationAmountRange(2, 100000);

            Assert.AreEqual(ValidationResult.Success, donationAmountValidationResult);
        }

        [Test]
        public void ShouldValidateSuccessfulyDonationAmountRangeWhenLowerRangeIsUndefined()
        {
            var donationAmountValidationResult = AppSettingsValidator.ValidateDonationAmountRange(0, 100000);

            Assert.AreEqual(ValidationResult.Success, donationAmountValidationResult);
        }

        [Test]
        public void ShouldValidateSuccessfulyDonationAmountRangeWhenHigherRangeIsUndefined()
        {
            var donationAmountValidationResult = AppSettingsValidator.ValidateDonationAmountRange(2, 0);

            Assert.AreEqual(ValidationResult.Success, donationAmountValidationResult);
        }

        [Test]
        public void ShouldValidateUnsuccessfulySinceLowerRangeIsLargerThanHigherRange()
        {
            var expectedValidationResult = new ValidationResult("Configuration of application contains invalid definition of minimum and maximum donation amount.");

            var donationAmountValidationResult = AppSettingsValidator.ValidateDonationAmountRange(5, 2);

            Assert.AreEqual(expectedValidationResult.ErrorMessage, donationAmountValidationResult.ErrorMessage);
        }

        [Test]
        public void ShouldValidateTaxRatePercentage()
        {
            _optionsMonitorMock.Setup(o => o.CurrentValue).Returns(new AppSettings()
            {
                TaxRatePercentage = "20"
            });

            var donationAmountValidationResult = AppSettingsValidator.ValidateTaxRatePercentage(_optionsMonitorMock.Object);

            Assert.AreEqual(ValidationResult.Success, donationAmountValidationResult);
        }

        [Test]
        public void ShouldValidateUnsuccessfulyWhenTaxRateIsNotPresentInAppSettings()
        {
            _optionsMonitorMock.Setup(o => o.CurrentValue).Returns(new AppSettings()
            {
            });

            var expectedValidationResult = new ValidationResult("Configuration of application does not contain tax rate percentage.");

            var donationAmountValidationResult = AppSettingsValidator.ValidateTaxRatePercentage(_optionsMonitorMock.Object);

            Assert.AreEqual(expectedValidationResult.ErrorMessage, donationAmountValidationResult.ErrorMessage);
        }

        [Test]
        public void ShouldValidateAppSettingsSuccessfuly()
        {
            _optionsMonitorMock.Setup(o => o.CurrentValue).Returns(new AppSettings()
            {
                MinimumDonationAmount = "2",
                MaximumDonationAmount = "100000",
                TaxRatePercentage = "20"
            });

            var expectedValidationResult = new List<ValidationResult>();

            var donationAmountValidationResult = AppSettingsValidator.ValidateAppSettings(_optionsMonitorMock.Object);

            Assert.AreEqual(expectedValidationResult, donationAmountValidationResult);
        }

        [Test]
        public void ValidateAppSettingsShouldReturnTaxRatePercentageValidationError()
        {
            _optionsMonitorMock.Setup(o => o.CurrentValue).Returns(new AppSettings()
            {
                MinimumDonationAmount = "2",
                MaximumDonationAmount = "100000"
            });

            var expectedValidationResult = new List<ValidationResult>()
            {
                new ValidationResult("Configuration of application does not contain tax rate percentage.")
            };

            var donationAmountValidationResult = AppSettingsValidator.ValidateAppSettings(_optionsMonitorMock.Object);

            Assert.AreEqual(1, donationAmountValidationResult.Count);
            Assert.AreEqual(expectedValidationResult[0].ErrorMessage, donationAmountValidationResult[0].ErrorMessage);
        }

        [Test]
        public void ValidateAppSettingsShouldReturnAmountRangeValidationError()
        {
            _optionsMonitorMock.Setup(o => o.CurrentValue).Returns(new AppSettings()
            {
                TaxRatePercentage = "20",
                MinimumDonationAmount = "2",
                MaximumDonationAmount = "1"
            });

            var expectedValidationResult = new List<ValidationResult>()
            {
                new ValidationResult("Configuration of application contains invalid definition of minimum and maximum donation amount.")
            };

            var donationAmountValidationResult = AppSettingsValidator.ValidateAppSettings(_optionsMonitorMock.Object);

            Assert.AreEqual(1, donationAmountValidationResult.Count);
            Assert.AreEqual(expectedValidationResult[0].ErrorMessage, donationAmountValidationResult[0].ErrorMessage);
        }

        [Test]
        public void ValidateAppSettingsShouldReturnBothAmountRangeAndTaxRateValidationError()
        {
            _optionsMonitorMock.Setup(o => o.CurrentValue).Returns(new AppSettings()
            {
                MinimumDonationAmount = "2",
                MaximumDonationAmount = "1"
            });

            var expectedValidationResult = new List<ValidationResult>()
            {
                new ValidationResult("Configuration of application contains invalid definition of minimum and maximum donation amount."),
                new ValidationResult("Configuration of application does not contain tax rate percentage.")
            };

            var donationAmountValidationResult = AppSettingsValidator.ValidateAppSettings(_optionsMonitorMock.Object);

            Assert.AreEqual(2, donationAmountValidationResult.Count);
            Assert.AreEqual(expectedValidationResult[0].ErrorMessage, donationAmountValidationResult[0].ErrorMessage);
            Assert.AreEqual(expectedValidationResult[1].ErrorMessage, donationAmountValidationResult[1].ErrorMessage);
        }
    }
}
