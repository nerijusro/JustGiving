namespace JG.FinTechTest.Domain.Responses
{
    public class GetDonationDeclarationResponse
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string PostCode { get; set; }
        public decimal DonationAmount { get; set; }
        public decimal GiftAidAmount { get; set; }
    }
}
