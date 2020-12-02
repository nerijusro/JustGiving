using LiteDB;

namespace JG.FinTechTest.Domain.Models
{
    public class DonationDeclaration
    {
        [BsonId]
        public ObjectId Id { get; set; }
        public string Name { get; set; }
        public string PostCode { get; set; }
        public decimal DonationAmount { get; set; }
    }
}
