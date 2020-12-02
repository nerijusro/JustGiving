using JG.FinTechTest.Domain.Interfaces;
using JG.FinTechTest.Domain.Models;
using JG.FinTechTest.Domain.Services;
using JG.FinTechTest.Resources;
using LiteDB;
using Moq;
using NUnit.Framework;

namespace JG.FinTechTest.Tests.ServiceTests
{
    public class DonationDeclarationServiceTests
    {
        private Mock<ILiteDbContext> _liteDbMock;
        private IDonationDeclarationService _donationService;

        public DonationDeclarationServiceTests()
        {
            _liteDbMock = new Mock<ILiteDbContext>();
        }

        [Test]
        public void ShouldGetDonationDeclaration()
        {
            var testObjectId = new ObjectId("5fc74923d9392f07c47d1b87");

            var expectedDonationDeclarationResponse = new DonationDeclaration
            {
                 Id = testObjectId,
                 Name = "Nerijus",
                 PostCode = "12345",
                 DonationAmount = 800,
            };

            _liteDbMock.Setup(x => x.Database.GetCollection<DonationDeclaration>(It.IsAny<string>(), BsonAutoId.ObjectId).FindOne(x => x.Id == testObjectId))
                .Returns(new DonationDeclaration { Id = testObjectId, Name = "Nerijus", PostCode = "12345", DonationAmount = 800});

            _donationService = new DonationDeclarationService(_liteDbMock.Object);

            var response = _donationService.Get(testObjectId);
            
            Assert.AreEqual(expectedDonationDeclarationResponse.ToString(), response.ToString());
        }

        [Test]
        public void ShouldReturnNullWhenDonationDeclarationIsNotPresent()
        {
            var testObjectId = new ObjectId("5fc74923d9392f07c47d1b87");

            _liteDbMock.Setup(x => x.Database.GetCollection<DonationDeclaration>(It.IsAny<string>(), BsonAutoId.ObjectId).FindOne(x => x.Id == testObjectId))
                .Returns((DonationDeclaration)null);

            _donationService = new DonationDeclarationService(_liteDbMock.Object);

            var response = _donationService.Get(testObjectId);

            Assert.IsNull(response);
        }

        [Test]
        public void ShouldInsertSuccessfully()
        {
            var testDeclarationRequest = new DonationDeclaration
            {
                Name = "Nerijus",
                PostCode = "12345",
                DonationAmount = 800,
            };

            var expectedObjectId = "5fc74923d9392f07c47d1b87";

            _liteDbMock.Setup(x => x.Database.GetCollection<DonationDeclaration>(It.IsAny<string>(), BsonAutoId.ObjectId).Insert(testDeclarationRequest))
                .Returns(new ObjectId("5fc74923d9392f07c47d1b87"));

            _donationService = new DonationDeclarationService(_liteDbMock.Object);

            var response = _donationService.Insert(testDeclarationRequest);

            Assert.AreEqual(expectedObjectId, response);
        }
    }
}
