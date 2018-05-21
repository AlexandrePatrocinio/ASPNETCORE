using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace ASPNETCORE
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = new WebHostBuilder()
                .UseStartup("ASPNETCORE")
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .Build();

            host.Run();
        }
    }
}
