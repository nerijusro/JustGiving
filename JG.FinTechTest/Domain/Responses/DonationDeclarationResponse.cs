using Newtonsoft.Json;

namespace JG.FinTechTest.Domain.Responses
{
    public class DonationDeclarationResponse
    {
        [JsonProperty("declarationId")]
        public string DeclarationId { get; set; }
        [JsonProperty("giftAidAmount")]
        public decimal GiftAidAmount { get; set; }
    }
}
