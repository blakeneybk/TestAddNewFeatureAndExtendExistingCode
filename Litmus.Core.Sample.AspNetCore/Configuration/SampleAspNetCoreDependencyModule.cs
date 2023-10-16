using Litmus.Core.DependencyInjection;
using Litmus.Core.Logging;
using Litmus.Core.Sample.AspNetCore.Controllers;

namespace Litmus.Core.Sample.AspNetCore.Configuration
{
    /// <summary>
    /// Dependency module for Sample ASPNETCore App
    /// </summary>
    public class SampleAspNetCoreDependencyModule : IDependencyInjectionModule
    {
        /// <summary>
        /// Register all the dependencies for this application
        /// </summary>
        /// <param name="container"></param>
        public void RegisterDependencies(IDependencyInjectionContainer container)
        {
            container.RegisterType<IStructuredLogger, ConsoleStructuredLogger>();
            container.RegisterType<HelloWorldController>();
        }
    }
}
