namespace JG.FinTechTest.Domain.Interfaces
{
    public interface IGiftAidCalculator
    {
        decimal TaxRatePercentage { get; }
        decimal CalculateGiftAid(decimal donationAmount);
    }
}
