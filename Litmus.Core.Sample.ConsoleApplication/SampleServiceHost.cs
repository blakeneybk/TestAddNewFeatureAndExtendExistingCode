using System.Threading;
using System.Threading.Tasks;
using Litmus.Core.Logging;
using Litmus.Core.ServiceHost;

namespace Litmus.Core.Sample.ConsoleApplication
{
    public class SampleServiceHost : IServiceHost
    {
        private readonly IStructuredLogger structuredLogger;

        public SampleServiceHost(IStructuredLogger structuredLogger)
        {
            this.structuredLogger = structuredLogger;
        }

        public async Task RunService(CancellationToken cancellationToken)
        {
            structuredLogger.Debug("Hello!  To stop this service hit Ctrl + C");

            try
            {
                await Task.Delay(Timeout.Infinite, cancellationToken);
            }
            finally
            {
                structuredLogger.Debug("Farewell");
            }
        }
    }
}
