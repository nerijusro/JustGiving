using JG.FinTechTest.Domain.Interfaces;
using Microsoft.Extensions.Options;
using System;

namespace JG.FinTechTest.Domain.Services
{
    public class GiftAidCalculator : IGiftAidCalculator
    {
        private AppSettings.AppSettings _appSettings;
        public decimal TaxRatePercentage { get; private set; }

        public GiftAidCalculator(IOptionsMonitor<AppSettings.AppSettings> settings)
        {
            _appSettings = settings.CurrentValue;
            TaxRatePercentage = decimal.Parse(_appSettings.TaxRatePercentage);
        }

        public decimal CalculateGiftAid(decimal donationAmount)
        {
            return Math.Round(donationAmount * (TaxRatePercentage / (100m - TaxRatePercentage)), 2);
        }
    }
}
