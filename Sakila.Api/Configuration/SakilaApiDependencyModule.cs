using Litmus.Core.DependencyInjection;
using Litmus.Core.Logging;
using Sakila.Api.Controllers;
using Sakila.Data;

namespace Sakila.Api.Configuration
{
    public class SakilaApiDependencyModule : IDependencyInjectionModule
    {
        public void RegisterDependencies(IDependencyInjectionContainer container)
        {
            container.RegisterType<IStructuredLogger, ConsoleStructuredLogger>();

            container.RegisterType<ArtistRepository>();
            container.RegisterType<CategoryRepository>();
            container.RegisterType<CustomerRepository>();
            container.RegisterType<OutstandingRentalsRepository>();
            container.RegisterType<StoreRepository>();
            container.RegisterType<RentalRepository>();
            container.RegisterType<MovieRepository>();

            container.RegisterType<ArtistController>();
            container.RegisterType<CustomerController>();
            container.RegisterType<CategoryContoller>();
            container.RegisterType<StoreController>();
            container.RegisterType<RentalController>();
            container.RegisterType<MovieController>();
        }
    }
}
