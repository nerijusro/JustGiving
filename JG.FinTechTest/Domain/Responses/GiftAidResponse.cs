using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace JG.FinTechTest.Domain.Responses
{
    public class GiftAidResponse
    {
        [Required]
        [JsonProperty("donationAmount")]
        public decimal DonationAmount { get; set; }
        
        [Required]
        [JsonProperty("giftAidAmount")]
        public decimal GiftAidAmount { get; set; }
    }
}
