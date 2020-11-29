using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace JG.FinTechTest.Domain.Requests
{
    public class GetGiftAidAmountRequest
    {
        [BindRequired]
        public decimal Amount { get; set; }
    }
}
