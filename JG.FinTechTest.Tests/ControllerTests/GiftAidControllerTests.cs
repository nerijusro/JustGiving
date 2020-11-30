//using JG.FinTechTest.Controllers;
//using JG.FinTechTest.Domain.Interfaces;
//using JG.FinTechTest.Domain.Requests;
//using JG.FinTechTest.Domain.Responses;
//using Microsoft.AspNetCore.Hosting;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.TestHost;
//using Microsoft.Extensions.Options;
//using Moq;
//using Newtonsoft.Json;
//using NUnit.Framework;
//using System;
//using System.ComponentModel.DataAnnotations;
//using System.Net.Http;
//using System.Security.Policy;
//using System.Text;
//using System.Threading.Tasks;

//namespace JG.FinTechTest.Tests.ControllerTests
//{
//    public class GiftAidControllerTests
//    {
//        private Mock<IGiftAidCalculator> _giftAidCalculator;
//        private Mock<IOptionsMonitor<AppSettings.AppSettings>> _optionsMonitorMock;

//        public GiftAidControllerTests()
//        {
//            _optionsMonitorMock = new Mock<IOptionsMonitor<AppSettings.AppSettings>>();

//            _giftAidCalculator = new Mock<IGiftAidCalculator>();
//            _giftAidCalculator.Setup(gac => gac.CalculateGiftAid(800)).Returns(200);
//        }

//        [Test]
//        public async Task ShouldReturnBadRequestResponseWhenConfigurationIsNotValid()
//        {
//            var getGiftAidAmountRequest = new GetGiftAidAmountRequest
//            {
//                Amount = 10
//            };

//            _optionsMonitorMock.Setup(o => o.CurrentValue).Returns(new AppSettings.AppSettings());

//            var expectedResponse = new ObjectResult("Internal Server Error: Definition of one or more configuration settings of the application are invalid.");
//            expectedResponse.StatusCode = 500;

//            var controller = new GiftAidController(_optionsMonitorMock.Object, _giftAidCalculator.Object);
//            var response = controller.GetGiftAidAmount(getGiftAidAmountRequest);
//            var responseContext = response as ObjectResult;

//            Assert.AreEqual(expectedResponse.StatusCode, responseContext.StatusCode);
//            Assert.AreEqual(expectedResponse.Value.ToString(), responseContext.Value.ToString());
//        }

//        //[Test]
//        //public void ShouldSetCorrectTaxRatePercentage()
//        //{
//        //    var getGiftAidAmountRequest = new GetGiftAidAmountRequest
//        //    {
//        //        Amount = 800
//        //    };

//        //    var getGiftAidAmountExpectedResponse = new GetGiftAidAmountResponse
//        //    {
//        //        DonationAmount = 800,
//        //        GiftAidAmount = 200
//        //    };

//        //    _giftAidController = new GiftAidController(_giftAidCalculator.Object);
//        //    var actionResult = _giftAidController.GetGiftAidAmount(getGiftAidAmountRequest);

//        //    var okObjectResult = actionResult as OkObjectResult;
//        //    var response = okObjectResult.Value as GetGiftAidAmountResponse;

//        //    Assert.AreEqual(getGiftAidAmountExpectedResponse.DonationAmount, response.DonationAmount);
//        //    Assert.AreEqual(getGiftAidAmountExpectedResponse.GiftAidAmount, response.GiftAidAmount);
//        //}

//        //Ask Dan about .net core upgrade, since TestServer is not compatible with 2.1
//        //[Test]
//        //public void ShouldReturnBadRequestResponse()
//        //{
//        //    var getGiftAidAmountRequest = new GetGiftAidAmountRequest();

//        //    _giftAidController = new GiftAidController(_giftAidCalculator.Object);
//        //    var actionResult = _giftAidController.GetGiftAidAmount(getGiftAidAmountRequest);

//        //    var okObjectResult = actionResult as ObjectResult;

//        //    Assert.IsNull(okObjectResult);

//        //    //arrange
//        //    var b = new WebHostBuilder()
//        //        .UseStartup<Startup>()
//        //        .UseEnvironment("test");

//        //    var server = new TestServer(b) { BaseAddress = new Uri(Url) };
//        //    var client = server.CreateClient();
//        //    var json = JsonConvert.SerializeObject(yourInvalidModel);
//        //    var content = new StringContent(json, Encoding.UTF8, "application/json");

//        //    //act
//        //    var result = await client.PostAsync("api/yourController", content);

//        //    assert
//        //    Assert.AreEqual(400, (int)result.StatusCode);
//        //}
//    }
//}
