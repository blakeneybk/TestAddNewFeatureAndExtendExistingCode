using System.Threading;
using System.Threading.Tasks;
using Litmus.Core.Database;
using Litmus.Core.Logging;
using NUnit.Framework;
using Sakila.Data;

namespace Sakila.Test.Data
{
    [TestFixture]
    public class CustomerRepositoryFixture
    {
        private readonly CustomerRepository repository;

        public CustomerRepositoryFixture()
        {
            repository = new CustomerRepository(new SakilaSqliteDatabaseConnection(new ConsoleStructuredLogger()));
        }

        [Test]
        public async Task GetCustomerDetailsReturnsExpectedValues()
        {
            var customer = await repository.GetCustomerDetails(1, CancellationToken.None);

            Assert.That(customer.Email, Is.EqualTo("MARY.SMITH@sakilacustomer.org"));
            Assert.That(customer.FirstName, Is.EqualTo("MARY"));
            Assert.That(customer.LastName, Is.EqualTo("SMITH"));
            Assert.That(customer.FavoriteArtistId, Is.EqualTo(37));
        }
    }
}
