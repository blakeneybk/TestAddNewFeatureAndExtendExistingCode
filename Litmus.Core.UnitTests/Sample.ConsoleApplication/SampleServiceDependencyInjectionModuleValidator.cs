using Litmus.Core.DependencyInjection;
using Litmus.Core.Sample.ConsoleApplication;
using NUnit.Framework;

namespace Litmus.Core.UnitTests.Sample.ConsoleApplication
{
    [TestFixture]
    public class SampleServiceDependencyInjectionModuleValidator
    {
        [Test]
        public void ValidateAllTypesCanBeResolved()
        {
            var validator = new DependencyInjectionModuleValidator<SampleServiceDependencyInjectionModule>();
            validator.ValidateAllRegisteredTypesCanBeResolved();
        }
    }
}
