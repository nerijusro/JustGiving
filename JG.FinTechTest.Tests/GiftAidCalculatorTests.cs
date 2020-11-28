using JG.FinTechTest.Domain.Services;
using NUnit.Framework;

namespace Tests
{
    public class GiftAidCalculatorTests
    {
        [SetUp]
        public void Setup()
        {
        }

        //[Test]
        //public void Test1()
        //{
        //    Assert.Pass();
        //}

        [Test]
        public void ShouldSetCorrectTaxRatePercentage()
        {
            var taxRatePercentage = 20;
            var giftAidCalculator = new GiftAidCalculator(taxRatePercentage.ToString());

            Assert.AreEqual(taxRatePercentage, giftAidCalculator.TaxRatePercentage);
        }

        public void ShouldCalculateGiftAid()
        {
            var taxRatePercentage = 20;
            var giftAidCalculator = new GiftAidCalculator(taxRatePercentage.ToString());
            var result = giftAidCalculator.CalculateGiftAid(800);

            Assert.AreEqual(200, result);
        }
    }
}