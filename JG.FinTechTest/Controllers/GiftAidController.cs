using JG.FinTechTest.Domain.Interfaces;
using JG.FinTechTest.Domain.Requests;
using JG.FinTechTest.Domain.Responses;
using JG.FinTechTest.Domain.Utils;
using JG.FinTechTest.Domain.Validators;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;

namespace JG.FinTechTest.Controllers
{
    [Route("api/giftaid")]
    [ApiController]
    public class GiftAidController : ControllerBase
    {
        private IGiftAidCalculator _giftAidCalculator;
        private IOptionsMonitor<AppSettings.AppSettings> _appSettings;

        public GiftAidController(IOptionsMonitor<AppSettings.AppSettings> settings, IGiftAidCalculator giftAidCalculator)
        {
            _appSettings = settings;
            _giftAidCalculator = giftAidCalculator;
        }

        [HttpGet]
        public IActionResult GetGiftAidAmount([FromQuery]GetGiftAidAmountRequest request)
        {
            var appSettingsValidationResult = AppSettingsValidator.ValidateAppSettings(_appSettings);
            if(appSettingsValidationResult.Count > 0)
            {
                return StatusCode(500, appSettingsValidationResult);
            }

            var requestValidationResult = AmountValidator.ValidateAmount(_appSettings, request.Amount);
            if(requestValidationResult != null)
            {
                return StatusCode(400, requestValidationResult);
            }

            var response = new GetGiftAidAmountResponse
            {
                DonationAmount = Math.Round(request.Amount, 2),
                GiftAidAmount = _giftAidCalculator.CalculateGiftAid(request.Amount)
            };

            return Ok(response);
        }
    }
}
