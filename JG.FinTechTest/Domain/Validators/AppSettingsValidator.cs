using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace JG.FinTechTest.Domain.Utils
{
    public class AppSettingsValidator
    {
        public static List<ValidationResult> ValidateAppSettings(IOptionsMonitor<AppSettings.AppSettings> settings)
        {
            var validationResults = new List<ValidationResult>();

            decimal minimumDonationAmount;
            decimal maximumDonationAmount;

            AppSettingsUtils.GetLowerAndHigherRange(settings, out minimumDonationAmount, out maximumDonationAmount);

            validationResults.Add(ValidateDonationAmountRange(minimumDonationAmount, maximumDonationAmount));
            validationResults.Add(ValidateTaxRatePercentage(settings));

            validationResults.RemoveAll(validationResult => validationResult == null);

            return validationResults;
        }

        public static ValidationResult ValidateDonationAmountRange(decimal lowerRange, decimal higherRange)
        {
            if((higherRange != 0) && (lowerRange < higherRange) || (higherRange == 0))
            {
                return ValidationResult.Success;
            }

            return new ValidationResult("Configuration of application contains invalid definition of minimum and maximum donation amount.");
        }

        public static ValidationResult ValidateTaxRatePercentage(IOptionsMonitor<AppSettings.AppSettings> settings)
        {
            decimal taxRatePercentage;
            var isTaxRatePercentageProvided = decimal.TryParse(settings.CurrentValue.TaxRatePercentage, out taxRatePercentage);

            if (isTaxRatePercentageProvided)
            {
                return ValidationResult.Success;
            }

            return new ValidationResult("Configuration of application does not contain tax rate percentage.");
        }
    }
}
