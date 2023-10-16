using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Litmus.Core.DependencyInjection;
using Litmus.Core.Logging;

namespace Litmus.Core.ServiceHost
{
    public class ServiceHostWrapper<TDependencyInjectionModule, TServiceHost>
        where TDependencyInjectionModule : IDependencyInjectionModule, new()
        where TServiceHost : IServiceHost
    {
        private readonly bool isService;
        private CancellationTokenSource cancellationTokenSource;
        private Task runningTask;
        private IStructuredLogger logger;

        private const int ServiceStopTimeoutMilliseconds = 60000;

        public ServiceHostWrapper(bool isService)
        {
            this.isService = isService;
        }

        public void Start()
        {
            cancellationTokenSource = new CancellationTokenSource();
            runningTask = StartTask();
        }

        private async Task StartTask()
        {
            // Give up this thread so main thread can continue and allow service to start;
            await Task.Yield();
        
            var unityModuleAdapter = new UnityContainerAdapter();
            var container = unityModuleAdapter.RegisterApplicationDependencies(new TDependencyInjectionModule());
            var serviceHost = container.Resolve<TServiceHost>();

            if (container.HasRegistrationFor<IStructuredLogger>())
            {
                logger = container.Resolve<IStructuredLogger>();
            }

            try
            {
                await serviceHost.RunService(cancellationTokenSource.Token);

                if (isService)
                {
                    logger?.Error("Service returned unexpected. You probably are missing a Task.Delay(Timeout.Infinite, token) or need to set isService to false");

                    // Services should not exit gracefully.  This is unexpected
                    Environment.Exit(1);
                }
                else
                {
                    Environment.Exit(0);
                }
            }
            catch (AggregateException e) when (e.InnerException is OperationCanceledException)
            {
                // this is expected
            }
            catch (OperationCanceledException)
            {
                // This is expected
            }
        }

        public void Stop()
        {
            if (cancellationTokenSource == null)
                return;

            Task.Run(() => cancellationTokenSource.Cancel());
            
            var sw = Stopwatch.StartNew();
            while (!runningTask.IsCompleted)
            {
                if (sw.ElapsedMilliseconds > ServiceStopTimeoutMilliseconds)
                {
                    logger?.Error("Service did not stop when requested.  It is likely this application is not checking cancellation tokens.  We are exiting anyways");
                }
            }

            Environment.Exit(0);
        }
    }
}
