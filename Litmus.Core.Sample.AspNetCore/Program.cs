using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Litmus.Core.AspNetCore;
using Litmus.Core.Sample.AspNetCore.Configuration;

namespace Litmus.Core.Sample.AspNetCore
{
    /// <summary>
    /// 
    /// </summary>
    public class Program
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                // Wire up the dependency module 
                .UseDependencyModuleServiceProvider<SampleAspNetCoreDependencyModule>()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
