using JG.FinTechTest.Domain.Interfaces;

namespace JG.FinTechTest.Domain.Services
{
    public class GiftAidCalculator : IGiftAidCalculator
    {
        public int TaxRatePercentage { get; private set; }

        public GiftAidCalculator(string taxRatePercentage)
        {
            TaxRatePercentage = int.Parse(taxRatePercentage);
        }

        public decimal CalculateGiftAid(int donationAmount)
        {
            return donationAmount * (TaxRatePercentage / (100 - TaxRatePercentage));
        }
    }
}
