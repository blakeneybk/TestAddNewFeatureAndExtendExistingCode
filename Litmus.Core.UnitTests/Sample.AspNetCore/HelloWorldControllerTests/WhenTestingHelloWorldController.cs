using Litmus.Core.Logging;
using Litmus.Core.Sample.AspNetCore.Controllers;
using NSubstitute;

namespace Litmus.Core.UnitTests.Sample.AspNetCore.HelloWorldControllerTests
{
    public class WhenTestingHelloWorldController
    {
        protected IStructuredLogger StructuredLogger { get; }
        protected HelloWorldController Controller { get; }

        public WhenTestingHelloWorldController()
        {
            StructuredLogger = Substitute.For<IStructuredLogger>();
            Controller = new HelloWorldController(StructuredLogger);
        }

    }
}
