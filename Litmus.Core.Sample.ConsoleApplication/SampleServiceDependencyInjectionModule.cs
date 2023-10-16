using Litmus.Core.DependencyInjection;
using Litmus.Core.Logging;
using Litmus.Core.ServiceHost;

namespace Litmus.Core.Sample.ConsoleApplication
{
    public class SampleServiceDependencyInjectionModule : IDependencyInjectionModule
    {
        public void RegisterDependencies(IDependencyInjectionContainer container)
        {
            container.RegisterType<IStructuredLogger, ConsoleStructuredLogger>();
            container.RegisterType<IServiceHost, SampleServiceHost>();
        }
    }
}
