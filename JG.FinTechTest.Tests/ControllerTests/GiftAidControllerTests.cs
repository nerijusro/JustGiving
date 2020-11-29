using JG.FinTechTest.Controllers;
using JG.FinTechTest.Domain.Interfaces;
using JG.FinTechTest.Domain.Requests;
using JG.FinTechTest.Domain.Responses;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;

namespace JG.FinTechTest.Tests.ControllerTests
{
    public class GiftAidControllerTests
    {
        private Mock<IGiftAidCalculator> _giftAidCalculator;
        private GiftAidController _giftAidController;

        public GiftAidControllerTests()
        {
            _giftAidCalculator = new Mock<IGiftAidCalculator>();
            _giftAidCalculator.Setup(gac => gac.CalculateGiftAid(800)).Returns(200);
        }

        [Test]
        public void ShouldSetCorrectTaxRatePercentage()
        {
            var getGiftAidAmountRequest = new GetGiftAidAmountRequest
            {
                Amount = 800
            };

            var getGiftAidAmountExpectedResponse = new GetGiftAidAmountResponse
            {
                DonationAmount = 800,
                GiftAidAmount = 200
            };

            _giftAidController = new GiftAidController(_giftAidCalculator.Object);
            var actionResult = _giftAidController.GetGiftAidAmount(getGiftAidAmountRequest);

            var okObjectResult = actionResult as OkObjectResult;
            var response = okObjectResult.Value as GetGiftAidAmountResponse;

            Assert.AreEqual(getGiftAidAmountExpectedResponse.DonationAmount, response.DonationAmount);
            Assert.AreEqual(getGiftAidAmountExpectedResponse.GiftAidAmount, response.GiftAidAmount);
        }

        //Ask Dan about .net core upgrade, since TestServer is not compatible with 2.1
        //[Test]
        //public void ShouldReturnBadRequestResponse()
        //{
        //    var getGiftAidAmountRequest = new GetGiftAidAmountRequest();

        //    _giftAidController = new GiftAidController(_giftAidCalculator.Object);
        //    var actionResult = _giftAidController.GetGiftAidAmount(getGiftAidAmountRequest);

        //    var okObjectResult = actionResult as ObjectResult;

        //    Assert.IsNull(okObjectResult);

        //    //arrange
        //    var b = new WebHostBuilder()
        //        .UseStartup<Startup>()
        //        .UseEnvironment("test");

        //    //var server = new TestServer(b) { BaseAddress = new Uri(Url) };
        //    //var client = server.CreateClient();
        //    //var json = JsonConvert.SerializeObject(yourInvalidModel);
        //    //var content = new StringContent(json, Encoding.UTF8, "application/json");

        //    ////act
        //    //var result = await client.PostAsync("api/yourController", content);

        //    //assert
        //    //Assert.AreEqual(400, (int)result.StatusCode);
        //}
    }
}
