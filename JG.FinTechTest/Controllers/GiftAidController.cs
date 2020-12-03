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
using Swashbuckle.AspNetCore.Annotations;

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

        /// <summary>
        /// Get the amount of gift aid reclaimable for donation amount
        /// </summary>
        /// <response code="200">Returns donationAmount together with calculated giftAid amount.</response>
        /// <response code="400">Amount does not belong to valid amount range.</response>
        /// <response code="500">Returned when configuration in appsettings.json is not appropriate.</response>
        [HttpGet]
        [SwaggerOperation(Summary = "Get the amount of gift aid reclaimable for donation amount")]
        [ProducesResponseType(typeof(GiftAidResponse), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Produces("application/json")]
        public IActionResult GetGiftAidAmount([FromQuery]GiftAidRequest request)
        {
            var validationResult = ValidateOperation(request.Amount);
            if(validationResult.StatusCode != 200)
            {
                return validationResult;
            }

            var response = new GiftAidResponse
            {
                DonationAmount = Math.Round(request.Amount, 2),
                GiftAidAmount = _giftAidCalculator.CalculateGiftAid(request.Amount)
            };

            return Ok(response);
        }

        /// <summary>
        /// Stores user's information once donation with Gift Aid is made
        /// </summary>
        /// <response code="200">Returns the declaration's id together with calculated giftAid amount.</response>
        /// <response code="400">Returned when one of text field is missing or when amount is not in a valid amount range.</response>
        /// <response code="500">Returned when configuration in appsettings.json is not appropriate.</response>
        [Route("declarations")]
        [HttpPost]
        [SwaggerOperation(Summary = "Stores user's information once donation with Gift Aid is made")]
        [ProducesResponseType(typeof(CreateDeclarationResponse), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        [Produces("application/json")]
        public IActionResult CreateDonationDeclaration(CreateDeclarationRequest request)
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

            var response = new CreateDeclarationResponse
            {
                DeclarationId = _donationDeclarationService.Insert(donationDeclaration).ToString(),
                GiftAidAmount = _giftAidCalculator.CalculateGiftAid(request.DonationAmount)
            };

            return Ok(response);
        }

        /// <summary>
        /// Get the record of user's donation declaration
        /// </summary>
        /// <response code="200">Returns the information provided in the donation declaration request.</response>
        /// <response code="400">Returned when provided id's length is not 24.</response>
        /// <response code="404">Returned when there was no records found by the id given.</response>
        [Route("declarations")]
        [HttpGet]
        [SwaggerOperation(Summary = "Get the record of user's donation declaration")]
        [ProducesResponseType(typeof(GetDeclarationResponse), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [Produces("application/json")]
        public IActionResult GetDonationDeclaration([FromQuery]GetDeclarationRequest request)
        {
            var donationDeclaration = _donationDeclarationService.Get(new ObjectId(request.Id));

            if(donationDeclaration == null)
            {
                return NotFound("Records with id provided were not found");
            }

            var response = new GetDeclarationResponse
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
