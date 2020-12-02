using System;
using JG.FinTechTest.Controllers;
using JG.FinTechTest.Domain.Interfaces;
using JG.FinTechTest.Domain.Requests;
using JG.FinTechTest.Domain.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using System.Threading.Tasks;
using JG.FinTechTest.Domain.Models;
using JG.FinTechTest.Options;
using LiteDB;

namespace JG.FinTechTest.Tests.ControllerTests
{
    public class GiftAidControllerTests
    {
        private Mock<IGiftAidCalculator> _giftAidCalculatorMock;
        private Mock<IDonationDeclarationService> _donationDeclarationServiceMock;
        private Mock<IOptionsMonitor<AppSettings>> _optionsMonitorMock;

        public GiftAidControllerTests()
        {
            _optionsMonitorMock = new Mock<IOptionsMonitor<AppSettings>>();
            _donationDeclarationServiceMock = new Mock<IDonationDeclarationService>();
            _giftAidCalculatorMock = new Mock<IGiftAidCalculator>();

            _giftAidCalculatorMock.Setup(gac => gac.CalculateGiftAid(800)).Returns(200);
        }

        [Test]
        public async Task ShouldReturnInternalServerErrorWhenConfigurationIsNotValid()
        {
            var getGiftAidAmountRequest = new GetGiftAidAmountRequest
            {
                Amount = 10
            };

            _optionsMonitorMock.Setup(o => o.CurrentValue).Returns(new AppSettings());

            var expectedResponse = new ObjectResult("Internal Server Error: Definition of one or more configuration settings of the application are invalid.");
            expectedResponse.StatusCode = 500;

            var controller = new GiftAidController(_optionsMonitorMock.Object, _giftAidCalculatorMock.Object, _donationDeclarationServiceMock.Object);
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

            _optionsMonitorMock.Setup(o => o.CurrentValue).Returns(new AppSettings
            {
                TaxRatePercentage = "20",
                MinimumDonationAmount = "20",
                MaximumDonationAmount = "50"
            });

            var expectedResponse = new ObjectResult("Donation amount can not be smaller than 20 and can not be larger than 50");
            expectedResponse.StatusCode = 400;

            var controller = new GiftAidController(_optionsMonitorMock.Object, _giftAidCalculatorMock.Object, _donationDeclarationServiceMock.Object);
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

            _optionsMonitorMock.Setup(o => o.CurrentValue).Returns(new AppSettings
            {
                TaxRatePercentage = "20",
                MinimumDonationAmount = "20",
                MaximumDonationAmount = "50"
            });

            var controller = new GiftAidController(_optionsMonitorMock.Object, _giftAidCalculatorMock.Object, _donationDeclarationServiceMock.Object);
            var response = controller.GetGiftAidAmount(getGiftAidAmountRequest);
            var responseContext = response as ObjectResult;

            Assert.AreEqual(200, responseContext.StatusCode);
            Assert.AreEqual(expectedResponse.ToString(), responseContext.Value.ToString());
        }

        [Test]
        public async Task CreateDonationShouldReturnBadRequestSinceRequestBodyIsIncomplete()
        {
            var testCreateDonationDeclarationRequest = new DonationDeclarationRequest
            {
                PostCode = "testPostCode",
                DonationAmount = 2
            };

            _optionsMonitorMock.Setup(o => o.CurrentValue).Returns(new AppSettings
            {
                TaxRatePercentage = "20",
                MinimumDonationAmount = "20",
                MaximumDonationAmount = "50"
            });

            var controller = new GiftAidController(_optionsMonitorMock.Object, _giftAidCalculatorMock.Object, _donationDeclarationServiceMock.Object);
            var response = controller.CreateDonationDeclaration(testCreateDonationDeclarationRequest);
            var responseContext = response as ObjectResult;

            Assert.AreEqual(400, responseContext.StatusCode);
        }

        [Test]
        public async Task CreateDonationShouldReturnInternalServerErrorWhenConfigurationIsNotValid()
        {
            var testCreateDonationDeclarationRequest = new DonationDeclarationRequest
            {
                PostCode = "testPostCode",
                DonationAmount = 2,
                Name = "TestName"
            };

            _optionsMonitorMock.Setup(o => o.CurrentValue).Returns(new AppSettings());

            var expectedResponse = new ObjectResult("Internal Server Error: Definition of one or more configuration settings of the application are invalid.");
            expectedResponse.StatusCode = 500;

            var controller = new GiftAidController(_optionsMonitorMock.Object, _giftAidCalculatorMock.Object, _donationDeclarationServiceMock.Object);
            var response = controller.CreateDonationDeclaration(testCreateDonationDeclarationRequest);
            var responseContext = response as ObjectResult;

            Assert.AreEqual(expectedResponse.StatusCode, responseContext.StatusCode);
            Assert.AreEqual(expectedResponse.Value.ToString(), responseContext.Value.ToString());
        }

        [Test]
        public async Task CreateDonationShouldReturnBadRequestWhenAmountIsNotValid()
        {
            var testCreateDonationDeclarationRequest = new DonationDeclarationRequest
            {
                PostCode = "testPostCode",
                DonationAmount = 2,
                Name = "TestName"
            };

            _optionsMonitorMock.Setup(o => o.CurrentValue).Returns(new AppSettings
            {
                TaxRatePercentage = "20",
                MinimumDonationAmount = "20",
                MaximumDonationAmount = "50"
            });

            var expectedResponse = new ObjectResult("Donation amount can not be smaller than 20 and can not be larger than 50");
            expectedResponse.StatusCode = 400;

            var controller = new GiftAidController(_optionsMonitorMock.Object, _giftAidCalculatorMock.Object, _donationDeclarationServiceMock.Object);
            var response = controller.CreateDonationDeclaration(testCreateDonationDeclarationRequest);
            var responseContext = response as ObjectResult;

            Assert.AreEqual(expectedResponse.StatusCode, responseContext.StatusCode);
            Assert.AreEqual(expectedResponse.Value.ToString(), responseContext.Value.ToString());
        }

        [Test]
        public async Task ShouldCreateDonationSuccessfully()
        {
            var testCreateDonationDeclarationRequest = new DonationDeclarationRequest
            {
                PostCode = "testPostCode",
                DonationAmount = 800,
                Name = "TestName"
            };

            _optionsMonitorMock.Setup(o => o.CurrentValue).Returns(new AppSettings
            {
                TaxRatePercentage = "20",
                MinimumDonationAmount = "20",
                MaximumDonationAmount = "1000"
            });

            _donationDeclarationServiceMock.Setup(x => x.Insert(It.IsAny<DonationDeclaration>())).Returns("12345");

            var expectedResponse = new DonationDeclarationResponse
            {
                DeclarationId = "12345",
                GiftAidAmount = 200
            };

            var controller = new GiftAidController(_optionsMonitorMock.Object, _giftAidCalculatorMock.Object, _donationDeclarationServiceMock.Object);
            var response = controller.CreateDonationDeclaration(testCreateDonationDeclarationRequest);
            var responseContext = response as ObjectResult;

            Assert.AreEqual(expectedResponse.ToString(), responseContext.Value.ToString());
        }

        [Test]
        public async Task GetDeclarationShouldReturnNotFoundCodeWhenGivenIdIsInvalid()
        {
            var testRequest = new GetDonationDeclarationRequest
            {
                Id = "123456789012345678901234"
            };

            _donationDeclarationServiceMock.Setup(x => x.Get(new ObjectId("123456789012345678901234")))
                .Returns((DonationDeclaration) null);

            var controller = new GiftAidController(_optionsMonitorMock.Object, _giftAidCalculatorMock.Object, _donationDeclarationServiceMock.Object);

            var response = controller.GetDonationDeclaration(testRequest);
            var responseContext = response as ObjectResult;

            Assert.AreEqual(404, responseContext.StatusCode);
        }

        [Test]
        public async Task GetDeclarationShouldReturnIdLengthValidationError()
        {
            var testRequest = new GetDonationDeclarationRequest
            {
                Id = "12345"
            };

            var controller = new GiftAidController(_optionsMonitorMock.Object, _giftAidCalculatorMock.Object, _donationDeclarationServiceMock.Object);

            Assert.Throws<ArgumentException>(() => controller.GetDonationDeclaration(testRequest));
        }

        [Test]
        public async Task GetDeclarationShouldReturnDeclarationData()
        {
            var testRequest = new GetDonationDeclarationRequest
            {
                Id = "123456789012345678901234"
            };

            var expectedResponse = new GetDonationDeclarationResponse
            {
                Id = "123456789012345678901234",
                Name = "Nerijus",
                PostCode = "testPostCode",
                DonationAmount = 800,
                GiftAidAmount = 200
            };

            _donationDeclarationServiceMock.Setup(x => x.Get(new ObjectId("123456789012345678901234")))
                .Returns(new DonationDeclaration
            {
                Id = new ObjectId("123456789012345678901234"),
                Name = "Nerijus",
                PostCode = "testPostCode",
                DonationAmount = 800
            });

            var controller = new GiftAidController(_optionsMonitorMock.Object, _giftAidCalculatorMock.Object, _donationDeclarationServiceMock.Object);

            var response = controller.GetDonationDeclaration(testRequest);
            var responseContext = response as ObjectResult;

            Assert.AreEqual(expectedResponse.ToString(), responseContext.Value.ToString());
        }
    }
}
