using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace JG.FinTechTest
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
            InitiateGiftAidCalculator();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();

        private static void InitiateGiftAidCalculator()
        {

        }
    }
}
