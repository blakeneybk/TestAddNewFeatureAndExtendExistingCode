using System.Threading;
using System.Threading.Tasks;
using Litmus.Core.Logging;
using Litmus.Core.Sample.ConsoleApplication;
using NSubstitute;
using NUnit.Framework;

namespace Litmus.Core.UnitTests.Sample.ConsoleApplication.SampleServiceHostTests
{
    [TestFixture]
    public class WritesDebugLogEntries
    {
        private CancellationToken cancellationToken = new(true);
        private IStructuredLogger structuredLogger;
        private SampleServiceHost sampleServiceHost;

        public WritesDebugLogEntries()
        {
            structuredLogger = Substitute.For<IStructuredLogger>();
            sampleServiceHost = new SampleServiceHost(structuredLogger);
        }

        [Test]
        public void ValidateLogDebugEntries()
        {
            //Given

            //When
            Assert.ThrowsAsync<TaskCanceledException>(async () => 
                await sampleServiceHost.RunService(cancellationToken));

            //Then
            structuredLogger
                .Received(1)
                .Debug(Arg.Is<string>(log => log.Contains("Hello")));

            structuredLogger
                .Received(1)
                .Debug(Arg.Is("Farewell"));
        }
    }
}
