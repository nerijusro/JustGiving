using JG.FinTechTest.Controllers;
using JG.FinTechTest.Domain.Interfaces;
using JG.FinTechTest.Domain.Requests;
using JG.FinTechTest.Domain.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;

using System.Threading.Tasks;

namespace JG.FinTechTest.Tests.ControllerTests
{
    public class GiftAidControllerTests
    {
        private Mock<IGiftAidCalculator> _giftAidCalculator;
        private Mock<IOptionsMonitor<AppSettings.AppSettings>> _optionsMonitorMock;

        public GiftAidControllerTests()
        {
            _optionsMonitorMock = new Mock<IOptionsMonitor<AppSettings.AppSettings>>();

            _giftAidCalculator = new Mock<IGiftAidCalculator>();
            _giftAidCalculator.Setup(gac => gac.CalculateGiftAid(800)).Returns(200);
        }

        [Test]
        public async Task ShouldReturnInternalServerErrorWhenConfigurationIsNotValid()
        {
            var getGiftAidAmountRequest = new GetGiftAidAmountRequest
            {
                Amount = 10
            };

            _optionsMonitorMock.Setup(o => o.CurrentValue).Returns(new AppSettings.AppSettings());

            var expectedResponse = new ObjectResult("Internal Server Error: Definition of one or more configuration settings of the application are invalid.");
            expectedResponse.StatusCode = 500;

            var controller = new GiftAidController(_optionsMonitorMock.Object, _giftAidCalculator.Object);
            var response = controller.GetGiftAidAmount(getGiftAidAmountRequest);
            var responseContext = response as ObjectResult;

            Assert.AreEqual(expectedResponse.StatusCode, responseContext.StatusCode);
            Assert.AreEqual(expectedResponse.Value.ToString(), responseContext.Value.ToString());
        }

        [Test]
        public async Task ShouldReturnBadRequestWhenAmountIsNotValid()
        {
            var getGiftAidAmountRequest = new GetGiftAidAmountRequest
            {
                Amount = 10
            };

            _optionsMonitorMock.Setup(o => o.CurrentValue).Returns(new AppSettings.AppSettings
            {
                TaxRatePercentage = "20",
                MinimumDonationAmount = "20",
                MaximumDonationAmount = "50"
            });

            var expectedResponse = new ObjectResult("Donation amount can not be smaller than 20 and can not be larger than 50");
            expectedResponse.StatusCode = 400;

            var controller = new GiftAidController(_optionsMonitorMock.Object, _giftAidCalculator.Object);
            var response = controller.GetGiftAidAmount(getGiftAidAmountRequest);
            var responseContext = response as ObjectResult;

            Assert.AreEqual(expectedResponse.StatusCode, responseContext.StatusCode);
            Assert.AreEqual(expectedResponse.Value.ToString(), responseContext.Value.ToString());
        }

        [Test]
        public async Task ShouldReturnValidCalculateGiftAidResponse()
        {
            var getGiftAidAmountRequest = new GetGiftAidAmountRequest
            {
                Amount = 30
            };

            var expectedResponse = new GetGiftAidAmountResponse
            {
                DonationAmount = 30,
                GiftAidAmount = 7.5m
            };

            _optionsMonitorMock.Setup(o => o.CurrentValue).Returns(new AppSettings.AppSettings
            {
                TaxRatePercentage = "20",
                MinimumDonationAmount = "20",
                MaximumDonationAmount = "50"
            });

            var controller = new GiftAidController(_optionsMonitorMock.Object, _giftAidCalculator.Object);
            var response = controller.GetGiftAidAmount(getGiftAidAmountRequest);
            var responseContext = response as ObjectResult;

            Assert.AreEqual(200, responseContext.StatusCode);
            Assert.AreEqual(expectedResponse.ToString(), responseContext.Value.ToString());
        }
    }
}
