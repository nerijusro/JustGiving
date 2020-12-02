using JG.FinTechTest.Domain.Interfaces;
using JG.FinTechTest.Domain.Utils;
using Microsoft.Extensions.Options;
using System;
using JG.FinTechTest.Options;

namespace JG.FinTechTest.Domain.Services
{
    public class GiftAidCalculator : IGiftAidCalculator
    {
        public decimal TaxRatePercentage { get; private set; }

        public GiftAidCalculator(IOptionsMonitor<AppSettings> settings)
        {
            TaxRatePercentage = AppSettingsUtils.GetTaxRatePercentage(settings);
        }

        public decimal CalculateGiftAid(decimal donationAmount)
        {
            return Math.Round(donationAmount * (TaxRatePercentage / (100m - TaxRatePercentage)), 2);
        }
    }
}
