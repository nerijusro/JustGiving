using JG.FinTechTest.Domain.Attributes;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace JG.FinTechTest.Domain.Requests
{
    public class GetGiftAidAmountRequest
    {
        [BindRequired]
        [Amount]
        public decimal Amount { get; set; }
    }
}
