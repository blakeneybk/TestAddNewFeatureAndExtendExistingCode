using Litmus.Core.DependencyInjection;
using Litmus.Core.Sample.AspNetCore.Configuration;
using NUnit.Framework;

namespace Litmus.Core.UnitTests.Sample.AspNetCore
{
    [TestFixture]
    public class SampleAspNetCoreDependencyModuleValidator
    {
        [Test]
        public void ValidateAllDependenciesAreValidated()
        {
            var validator = new DependencyInjectionModuleValidator<SampleAspNetCoreDependencyModule>();
            validator.ValidateAllRegisteredTypesCanBeResolved();
        }
    }
}
