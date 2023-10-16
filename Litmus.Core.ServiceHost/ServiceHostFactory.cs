using System.Reflection;
using System.Runtime.InteropServices;
using Litmus.Core.DependencyInjection;
using Topshelf;
using Topshelf.Runtime.DotNetCore;

namespace Litmus.Core.ServiceHost
{
    public class ServiceHostFactory<TDependencyInjectionModule, TServiceHost>
        where TDependencyInjectionModule : IDependencyInjectionModule, new()
        where TServiceHost : IServiceHost

    {
        /// <summary>
        /// Initialize and run the Service Host using the given dependency injection module
        /// </summary>
        /// <param name="isService">Indicates this application is a service.  Mainly used to set an exit code if the Service host doesn't run forever</param>
        /// <returns></returns>
        public static int Run(bool isService)
        {
            var exitCode = HostFactory.Run(topshelfConfigurator =>
            {
                // Special setup when running as Linux.  This allows us to use this to support running as a daemon
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    topshelfConfigurator.UseEnvironmentBuilder(c => new DotNetCoreEnvironmentBuilder(c));
                }

                // Set the service name to the assembly name as a best practice
                topshelfConfigurator.SetServiceName((Assembly.GetEntryAssembly() ?? Assembly.GetExecutingAssembly()).GetName().Name);

                topshelfConfigurator.Service<ServiceHostWrapper<TDependencyInjectionModule, TServiceHost>>(s =>
                {
                    s.ConstructUsing(_ => new ServiceHostWrapper<TDependencyInjectionModule, TServiceHost>(isService));
                    s.WhenStarted(service => service.Start());
                    s.WhenStopped(service => service.Stop());
                });
            });

            return (int)exitCode;
        }
    }
}
