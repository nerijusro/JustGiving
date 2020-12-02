using JG.FinTechTest.Domain.Interfaces;
using JG.FinTechTest.Domain.Models;
using JG.FinTechTest.Domain.Requests;
using JG.FinTechTest.Domain.Responses;
using JG.FinTechTest.Domain.Utils;
using JG.FinTechTest.Domain.Validators;
using LiteDB;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.ComponentModel.DataAnnotations;
using JG.FinTechTest.Options;

namespace JG.FinTechTest.Controllers
{
    [Route("api/giftaid")]
    [ApiController]
    public class GiftAidController : ControllerBase
    {
        private IGiftAidCalculator _giftAidCalculator;
        private IDonationDeclarationService _donationDeclarationService;
        private IOptionsMonitor<AppSettings> _appSettings;

        public GiftAidController(IOptionsMonitor<AppSettings> settings, IGiftAidCalculator giftAidCalculator, IDonationDeclarationService donationDeclarationService)
        {
            _appSettings = settings;
            _giftAidCalculator = giftAidCalculator;
            _donationDeclarationService = donationDeclarationService;
        }

        [HttpGet]
        public IActionResult GetGiftAidAmount([FromQuery]GetGiftAidAmountRequest request)
        {
            var validationResult = ValidateOperation(request.Amount);
            if(validationResult.StatusCode != 200)
            {
                return validationResult;
            }

            var response = new GetGiftAidAmountResponse
            {
                DonationAmount = Math.Round(request.Amount, 2),
                GiftAidAmount = _giftAidCalculator.CalculateGiftAid(request.Amount)
            };

            return Ok(response);
        }

        [Route("declarations")]
        [HttpPost]
        public IActionResult CreateDonationDeclaration(DonationDeclarationRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var validationResult = ValidateOperation(request.DonationAmount);
            if (validationResult.StatusCode != 200)
            {
                return validationResult;
            }

            var donationDeclaration = new DonationDeclaration
            {
                Name = request.Name,
                PostCode = request.PostCode,
                DonationAmount = request.DonationAmount
            };

            var response = new DonationDeclarationResponse
            {
                DeclarationId = _donationDeclarationService.Insert(donationDeclaration).ToString(),
                GiftAidAmount = _giftAidCalculator.CalculateGiftAid(request.DonationAmount)
            };

            return Ok(response);
        }

        [Route("declarations")]
        [HttpGet]
        public IActionResult GetDonationDeclaration([FromQuery]GetDonationDeclarationRequest request)
        {
            var donationDeclaration = _donationDeclarationService.Get(new ObjectId(request.Id));

            if(donationDeclaration == null)
            {
                return NotFound("Records with id provided were not found");
            }

            var response = new GetDonationDeclarationResponse
            {
                Id = donationDeclaration.Id.ToString(),
                Name = donationDeclaration.Name,
                PostCode = donationDeclaration.PostCode,
                DonationAmount = donationDeclaration.DonationAmount,
                GiftAidAmount = _giftAidCalculator.CalculateGiftAid(donationDeclaration.DonationAmount)
            };

            return Ok(response);
        }

        private ObjectResult ValidateOperation(decimal donationAmount)
        {
            var appSettingsValidationResult = AppSettingsValidator.ValidateAppSettings(_appSettings);
            if (appSettingsValidationResult.Count > 0)
            {
                return StatusCode(500, new ValidationResult("Internal Server Error: Definition of one or more configuration settings of the application are invalid."));
            }

            var requestValidationResult = AmountValidator.ValidateAmount(_appSettings, donationAmount);
            if (requestValidationResult != null)
            {
                return StatusCode(400, requestValidationResult);
            }

            return StatusCode(200, null);
        }
    }
}
