using System.Threading.Tasks;
using NUnit.Framework;

namespace Litmus.Core.UnitTests.Sample.AspNetCore.HelloWorldControllerTests
{
    [TestFixture]
    public class HelloWorldPostActionGoldenPath : WhenTestingHelloWorldController
    {
        [Test]
        public async Task ReturnsExpectedValue()
        {
            // Given
            var expected = 2;
            
            // When
            var actual = await Controller.HelloWorldEcho(expected);

            // Then
            Assert.That(actual, Is.EqualTo(expected));
        }
    }
}
