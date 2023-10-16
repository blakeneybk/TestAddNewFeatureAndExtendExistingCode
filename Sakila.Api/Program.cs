using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Sakila.Api.Configuration;
using Litmus.Core.AspNetCore;

namespace Sakila.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseDependencyModuleServiceProvider<SakilaApiDependencyModule>()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
