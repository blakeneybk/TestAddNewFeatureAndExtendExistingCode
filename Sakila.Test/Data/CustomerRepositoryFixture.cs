using System.Threading;
using System.Threading.Tasks;
using Litmus.Core.Database;
using Litmus.Core.Logging;
using NUnit.Framework;
using Sakila.Data;

namespace Sakila.Test.Data
{
    //THIS TEST FIXTURE REQUIRES A FRESH UNMODIFIED DOWNLOAD OF THE SAKILA DB
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
            Assert.That(customer.FavoriteMovieId, Is.EqualTo(663));
            Assert.That(customer.FavoriteCategoryId, Is.EqualTo(4));
        }

        [Test]
        public async Task ValidateCustomerIdAsyncReturnsExistingReturnsTrue()
        {
            Assert.IsTrue(await repository.ValidateCustomerIdAsync(1, CancellationToken.None));
        }

        [Test]
        public async Task ValidateCustomerIdAsyncReturnsNotExistingReturnsFalse()
        {
            Assert.IsFalse(await repository.ValidateCustomerIdAsync(654986498, CancellationToken.None));
        }
    }
}
