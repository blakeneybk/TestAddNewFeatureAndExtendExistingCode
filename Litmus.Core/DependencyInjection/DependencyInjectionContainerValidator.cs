using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading;
using Litmus.Core.Logging;

namespace Litmus.Core.DependencyInjection
{
    /// <summary>
    /// This class provides the ability to validate a dependency injection module has all the types required to initialize an application
    /// Utilize this class in a unit test to ensure we do not run into errors at run time.  
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DependencyInjectionContainerValidator<T> where T : IDependencyInjectionContainer
    {
        private static readonly TimeSpan LongRunningConstructorThreshold = TimeSpan.FromSeconds(2);

        private readonly T container;
        private readonly IStructuredLogger structuredLogger;

        public DependencyInjectionContainerValidator(T container, IStructuredLogger structuredLogger)
        {
            this.container = container;
            this.structuredLogger = structuredLogger;
        }

        public void ValidateAllRegisteredTypesCanBeResolved(CancellationToken cancellationToken)
        {
            MethodInfo method = typeof(T).GetMethods().First(m => m.Name == "Resolve" && m.IsGenericMethod);

            var processedCount = 0;

            // Converting RegisteredTypes to an array here and iterating over *that* to fix an issue we were seeing where
            // the foreach loop below was exiting prematurely for unknown reasons and therefore not validating all of the
            // dependencies in the container. This was causing false positive test results.
            var registeredDependencies = container.RegisteredTypes.ToArray();

            foreach (var type in registeredDependencies)
            {
                processedCount++;

                var sw = Stopwatch.StartNew();

                using (cancellationToken.Register(() => structuredLogger.Error("Timeout while resolving {type}", type)))
                {
                    if (type.IsGenericTypeDefinition)
                    {
                        // An open generic type or GenericTypeDefinition (i.e. IEnumerable<>) cannot be instantiated
                        // given that it is missing a type param (i.e. int, string), so, no need to validate
                        continue;
                    }

                    var resolveTypeMethod = method.MakeGenericMethod(type);

                    object resolvedObject = null;

                    try
                    {
                        resolvedObject = resolveTypeMethod.Invoke(container, null);
                    }
                    catch (Exception ex)
                    {
                        throw new TypeInitializationException(type.FullName, ex);
                    }

                    if (resolvedObject == null)
                    {
                        throw new TypeInitializationException(type.FullName, new ArgumentException("Unable to resolve type"));
                    }
                }

                if (sw.Elapsed > LongRunningConstructorThreshold)
                {
                    structuredLogger.Warning("{Type} took {ElapsedMilliseconds} to resolve. There is likely work being done in a constructor", type, sw.ElapsedMilliseconds);
                }
            }

            if (processedCount < registeredDependencies.Length)
            {
                structuredLogger.Warning("The validator erroneously skipped checking {SkippedCount} dependencies", registeredDependencies.Length - processedCount);
            }
        }
    }
}
