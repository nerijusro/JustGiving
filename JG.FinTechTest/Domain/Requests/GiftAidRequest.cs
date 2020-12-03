using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;

namespace JG.FinTechTest.Domain.Requests
{
    public class GiftAidRequest
    {
        [JsonProperty("amount")]
        [BindRequired]
        public decimal Amount { get; set; }
    }
}
