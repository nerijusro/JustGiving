using JG.FinTechTest.Options;
using Microsoft.Extensions.Options;
using LiteDB;

namespace JG.FinTechTest.Resources
{
    public class LiteDbContext : ILiteDbContext
    {
        public ILiteDatabase Database { get; }
        public ConnectionString ConnectionString { get; set; }

        public LiteDbContext(IOptions<LiteDbOptions> options)
        {
            ConnectionString = new ConnectionString(options.Value.DatabaseLocation);
            ConnectionString.Connection = ConnectionType.Shared;
            ConnectionString.Upgrade = true;
            Database = new LiteDatabase(ConnectionString);
        }
    }
}
