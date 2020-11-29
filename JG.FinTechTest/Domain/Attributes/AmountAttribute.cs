using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations;

namespace JG.FinTechTest.Domain.Attributes
{
    public class AmountAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var settings = (IOptionsMonitor<AppSettings.AppSettings>)validationContext.GetService(typeof(IOptionsMonitor<AppSettings.AppSettings>));

            var minimumDonationAmount = decimal.Parse(settings.CurrentValue.MinimumDonationAmount);
            var maximumDonationAmount = decimal.Parse(settings.CurrentValue.MaximumDonationAmount);

            var amount = decimal.Parse(value.ToString());
            var isValid = (amount > minimumDonationAmount) && (amount < maximumDonationAmount);

            if (isValid)
            {
                return ValidationResult.Success;
            }

            return new ValidationResult("Donation amount can not be smaller than " + minimumDonationAmount + " and can not be larger than " + maximumDonationAmount);
        }
    }
}
