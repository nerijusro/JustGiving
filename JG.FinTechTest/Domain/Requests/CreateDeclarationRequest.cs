using System.ComponentModel.DataAnnotations;

namespace JG.FinTechTest.Domain.Requests
{
    public class CreateDeclarationRequest
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string PostCode { get; set; }
        [Required]
        public decimal DonationAmount { get; set; }
    }
}
