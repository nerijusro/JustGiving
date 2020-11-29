using JG.FinTechTest.Domain.Interfaces;
using JG.FinTechTest.Domain.Requests;
using JG.FinTechTest.Domain.Responses;
using Microsoft.AspNetCore.Mvc;
using System;

namespace JG.FinTechTest.Controllers
{
    [Route("api/giftaid")]
    [ApiController]
    public class GiftAidController : ControllerBase
    {
        private IGiftAidCalculator _giftAidCalculator;

        public GiftAidController(IGiftAidCalculator giftAidCalculator)
        {
            _giftAidCalculator = giftAidCalculator;
        }

        [HttpGet]
        public IActionResult GetGiftAidAmount([FromQuery]GetGiftAidAmountRequest request)
        {
            if (ModelState.IsValid)
            {
                var response = new GetGiftAidAmountResponse
                {
                    DonationAmount = Math.Round(request.Amount, 2),
                    GiftAidAmount = _giftAidCalculator.CalculateGiftAid(request.Amount)
                };

                return Ok(response);
            }

            return BadRequest();
        }
    }
}
