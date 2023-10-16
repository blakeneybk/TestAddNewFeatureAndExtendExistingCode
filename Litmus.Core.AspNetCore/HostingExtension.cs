using Litmus.Core.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Unity.Microsoft.DependencyInjection;

namespace Litmus.Core.AspNetCore
{
    public static class HostingExtension
    {
        /// <summary>
        /// Wires up our DI framework as the DI system for ASP.NET Core
        /// </summary>
        /// <param name="hostBuilder"></param>
        /// <param name="dependencyInjectionModule">Populated Dependency Injection Module for Application</param>
        /// <returns></returns>
        public static IHostBuilder UseDependencyModuleServiceProvider(
            this IHostBuilder hostBuilder,
            IDependencyInjectionModule dependencyInjectionModule)
        {
            var unityModuleAdapter = new UnityContainerAdapter();
            unityModuleAdapter.RegisterApplicationDependencies(dependencyInjectionModule);
            return hostBuilder.UseUnityServiceProvider(unityModuleAdapter.UnityContainer);
        }

        /// <summary>
        /// Wires up our DI framework as the DI system for ASP.NET Core 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="hostBuilder"></param>
        /// <returns></returns>
        public static IHostBuilder UseDependencyModuleServiceProvider<T>(
            this IHostBuilder hostBuilder) where T : IDependencyInjectionModule, new() =>
            hostBuilder.UseDependencyModuleServiceProvider(new T());
    }
}
