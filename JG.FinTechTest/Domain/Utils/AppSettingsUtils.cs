using Microsoft.Extensions.Options;

namespace JG.FinTechTest.Domain.Utils
{
    public class AppSettingsUtils
    {
        public static void GetLowerAndHigherRange(IOptionsMonitor<AppSettings.AppSettings> settings, out decimal lowerRange, out decimal higherRange)
        {
            decimal.TryParse(settings.CurrentValue.MinimumDonationAmount, out lowerRange);
            decimal.TryParse(settings.CurrentValue.MaximumDonationAmount, out higherRange);
        }

        public static decimal GetTaxRatePercentage(IOptionsMonitor<AppSettings.AppSettings> settings)
        {
            decimal taxRatePercentage;
            decimal.TryParse(settings.CurrentValue.TaxRatePercentage, out taxRatePercentage);
            return taxRatePercentage;
        }
    }
}
