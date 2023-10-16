using Litmus.Core.ServiceHost;

namespace Litmus.Core.Sample.ConsoleApplication
{
    class Program
    {
        static int Main() =>
            ServiceHostFactory<SampleServiceDependencyInjectionModule, SampleServiceHost>.Run(isService: false);
    }
}
