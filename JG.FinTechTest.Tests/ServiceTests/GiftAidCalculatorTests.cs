using JG.FinTechTest.Domain.Services;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;

namespace JG.FinTechTest.Tests.ServiceTests
{
    public class GiftAidCalculatorTests
    {
        private Mock<IOptionsMonitor<AppSettings.AppSettings>> _optionsMonitorMock;

        public GiftAidCalculatorTests()
        {
            _optionsMonitorMock = new Mock<IOptionsMonitor<AppSettings.AppSettings>>();
            _optionsMonitorMock.Setup(o => o.CurrentValue).Returns(new AppSettings.AppSettings() { TaxRatePercentage = "20" });
        }

        [Test]
        public void ShouldSetCorrectTaxRatePercentage()
        {
            var giftAidCalculator = new GiftAidCalculator(_optionsMonitorMock.Object);

            Assert.AreEqual(20, giftAidCalculator.TaxRatePercentage);
        }

        [Test]
        public void ShouldCalculateGiftAid()
        {
            var giftAidCalculator = new GiftAidCalculator(_optionsMonitorMock.Object);
            var result = giftAidCalculator.CalculateGiftAid(800);

            Assert.AreEqual(200, result);
        }
    }
}