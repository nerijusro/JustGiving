using Newtonsoft.Json;

namespace JG.FinTechTest.Domain.Responses
{
    public class GetGiftAidAmountResponse
    {
        [JsonProperty("donationAmount")]
        public decimal DonationAmount { get; set; }

        [JsonProperty("giftAidAmount")]
        public decimal GiftAidAmount { get; set; }
    }
}
