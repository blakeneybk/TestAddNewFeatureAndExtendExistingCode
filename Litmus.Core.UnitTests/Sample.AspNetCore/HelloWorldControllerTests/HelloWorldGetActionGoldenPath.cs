using System.Threading.Tasks;
using NUnit.Framework;

namespace Litmus.Core.UnitTests.Sample.AspNetCore.HelloWorldControllerTests
{
    [TestFixture]
    public class HelloWorldGetActionGoldenPath : WhenTestingHelloWorldController
    {
        [Test]
        public async Task ReturnsExpectedValue()
        {
            // Given

            // When
            var actual = await Controller.HelloWorld();

            // Then
            Assert.That(actual, Is.EqualTo(1));
        }
    }
}
