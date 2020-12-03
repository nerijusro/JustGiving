using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace JG.FinTechTest.Domain.Responses
{
    public class CreateDeclarationResponse
    {
        [JsonProperty("declarationId")]
        [Required]
        public string DeclarationId { get; set; }

        [JsonProperty("giftAidAmount")]
        [Required]
        public decimal GiftAidAmount { get; set; }
    }
}
