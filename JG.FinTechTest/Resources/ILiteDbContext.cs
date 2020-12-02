using LiteDB;

namespace JG.FinTechTest.Resources
{
    public interface ILiteDbContext
    {
        ILiteDatabase Database { get; }
    }
}
