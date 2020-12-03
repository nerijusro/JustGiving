using Newtonsoft.Json;

namespace JG.FinTechTest.Domain.Responses
{
    public class GetDeclarationResponse
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("postCode")]
        public string PostCode { get; set; }
        [JsonProperty("donationAmount")]
        public decimal DonationAmount { get; set; }
        [JsonProperty("giftAidAmount")]
        public decimal GiftAidAmount { get; set; }
    }
}
