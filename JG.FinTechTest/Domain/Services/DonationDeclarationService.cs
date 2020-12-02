using JG.FinTechTest.Domain.Interfaces;
using JG.FinTechTest.Domain.Models;
using JG.FinTechTest.Resources;
using LiteDB;

namespace JG.FinTechTest.Domain.Services
{
    public class DonationDeclarationService : IDonationDeclarationService
    {
        private ILiteDatabase _liteDb;

        public DonationDeclarationService(ILiteDbContext liteDbContext)
        {
            _liteDb = liteDbContext.Database;
        }

        public DonationDeclaration Get(ObjectId id)
        {
            return _liteDb.GetCollection<DonationDeclaration>("DonationDeclaration")
                .FindOne(x => x.Id == id);
        }

        public string Insert(DonationDeclaration donationDeclaration)
        {
            ObjectId id;

            lock(_liteDb)
            {
                id = _liteDb.GetCollection<DonationDeclaration>("DonationDeclaration").Insert(donationDeclaration);
            }

            return id.ToString();
        }
    }
}
