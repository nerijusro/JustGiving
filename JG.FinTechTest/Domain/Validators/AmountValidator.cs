using JG.FinTechTest.Domain.Utils;
using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations;
using JG.FinTechTest.Options;

namespace JG.FinTechTest.Domain.Validators
{
    public class AmountValidator
    {
        public static ValidationResult ValidateAmount(IOptionsMonitor<AppSettings> settings, decimal amount)
        {
            decimal minimumDonationAmount;
            decimal maximumDonationAmount;

            AppSettingsUtils.GetLowerAndHigherRange(settings, out minimumDonationAmount, out maximumDonationAmount);

            var isLowerRangeValid = amount >= minimumDonationAmount;
            var largerRangeExists = maximumDonationAmount != 0;
            var isLargerRangeValid = !largerRangeExists ? true : amount <= maximumDonationAmount;

            if (isLowerRangeValid && isLargerRangeValid)
            {
                return ValidationResult.Success;
            }

            return new ValidationResult("Donation amount can not be smaller than " + minimumDonationAmount + (largerRangeExists ? (" and can not be larger than " + maximumDonationAmount) : ""));
        }
    }
}
