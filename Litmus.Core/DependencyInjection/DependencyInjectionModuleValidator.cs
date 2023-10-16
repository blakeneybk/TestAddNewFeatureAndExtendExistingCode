using System.Threading;
using Litmus.Core.Logging;

namespace Litmus.Core.DependencyInjection
{
    public class DependencyInjectionModuleValidator<T> where T : IDependencyInjectionModule, new()
    {
        public void ValidateAllRegisteredTypesCanBeResolved(CancellationToken cancellationToken = default)
        {
            var container = new UnityContainerAdapter();
            container.RegisterApplicationDependencies(new T());

            var containerValidator =
                new DependencyInjectionContainerValidator<UnityContainerAdapter>(container,
                    new ConsoleStructuredLogger());
            containerValidator.ValidateAllRegisteredTypesCanBeResolved(cancellationToken);
        }
    }
}
