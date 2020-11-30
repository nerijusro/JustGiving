using JG.FinTechTest.Domain.Utils;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;

namespace JG.FinTechTest.Tests.UtilsTests
{
    public class AppSettingsUtilsTests
    {
        private Mock<IOptionsMonitor<AppSettings.AppSettings>> _optionsMonitorMock;

        public AppSettingsUtilsTests()
        {
            _optionsMonitorMock = new Mock<IOptionsMonitor<AppSettings.AppSettings>>();
        }

        [Test]
        public void ShouldGetLowerAndHigherRange()
        {
            _optionsMonitorMock.Setup(o => o.CurrentValue).Returns(new AppSettings.AppSettings()
            {
                MinimumDonationAmount = "2",
                MaximumDonationAmount = "100000"
            });

            decimal minimumDonationAmount;
            decimal maximumDonationAmount;
            AppSettingsUtils.GetLowerAndHigherRange(_optionsMonitorMock.Object, out minimumDonationAmount, out maximumDonationAmount);

            Assert.AreEqual(2, minimumDonationAmount);
            Assert.AreEqual(100000, maximumDonationAmount);
        }

        [Test]
        public void ShouldGetLowerRangeWhenHigherIsNotPresent()
        {
            _optionsMonitorMock.Setup(o => o.CurrentValue).Returns(new AppSettings.AppSettings()
            {
                MinimumDonationAmount = "2",
            });

            decimal minimumDonationAmount;
            decimal maximumDonationAmount;
            AppSettingsUtils.GetLowerAndHigherRange(_optionsMonitorMock.Object, out minimumDonationAmount, out maximumDonationAmount);

            Assert.AreEqual(2, minimumDonationAmount);
            Assert.AreEqual(0, maximumDonationAmount);
        }

        [Test]
        public void ShouldGetHigherRangeWhenLowerIsNotPresent()
        {
            _optionsMonitorMock.Setup(o => o.CurrentValue).Returns(new AppSettings.AppSettings()
            {
                MaximumDonationAmount = "2",
            });

            decimal minimumDonationAmount;
            decimal maximumDonationAmount;
            AppSettingsUtils.GetLowerAndHigherRange(_optionsMonitorMock.Object, out minimumDonationAmount, out maximumDonationAmount);

            Assert.AreEqual(2, maximumDonationAmount);
        }

        [Test]
        public void ShouldSetLowerRangeToZeroWhenItsNotPresentInAppSettings()
        {
            _optionsMonitorMock.Setup(o => o.CurrentValue).Returns(new AppSettings.AppSettings()
            {
                MaximumDonationAmount = "2",
            });

            decimal minimumDonationAmount;
            decimal maximumDonationAmount;
            AppSettingsUtils.GetLowerAndHigherRange(_optionsMonitorMock.Object, out minimumDonationAmount, out maximumDonationAmount);

            Assert.AreEqual(0, minimumDonationAmount);
        }

        [Test]
        public void ShouldGetTaxRatePercentage()
        {
            _optionsMonitorMock.Setup(o => o.CurrentValue).Returns(new AppSettings.AppSettings()
            {
                TaxRatePercentage = "20",
            });

            var taxRate = AppSettingsUtils.GetTaxRatePercentage(_optionsMonitorMock.Object);

            Assert.AreEqual(20, taxRate);
        }

        [Test]
        public void ShouldGetZeroWhenTaxRatePercentageIsNotPresentInAppSettings()
        {
            _optionsMonitorMock.Setup(o => o.CurrentValue).Returns(new AppSettings.AppSettings()
            {
            });

            var taxRate = AppSettingsUtils.GetTaxRatePercentage(_optionsMonitorMock.Object);

            Assert.AreEqual(0, taxRate);
        }
    }
}
