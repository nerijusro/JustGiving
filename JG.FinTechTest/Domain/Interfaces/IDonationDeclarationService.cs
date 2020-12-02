using JG.FinTechTest.Domain.Models;
using LiteDB;

namespace JG.FinTechTest.Domain.Interfaces
{
    public interface IDonationDeclarationService
    {
        DonationDeclaration Get(ObjectId id);
        string Insert(DonationDeclaration donationDeclaration);
    }
}
