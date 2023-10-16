using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Litmus.Core.Database;
using Litmus.Core.Logging;
using NUnit.Framework;
using Sakila.Data;

namespace Sakila.Test.Data
{
    [TestFixture]
    public class OutstandingRentalsRepositoryFixture
    {
        private readonly OutstandingRentalsRepository repository;

        public OutstandingRentalsRepositoryFixture()
        {
            repository = new OutstandingRentalsRepository(new SakilaSqliteDatabaseConnection(new ConsoleStructuredLogger()));
        }

        [Test]
        public async Task OutstandingRentalsReturnsCustomerRecords()
        {
            var customers = await repository.OutstandingRentals(CancellationToken.None);
            CollectionAssert.IsNotEmpty(customers);

            var firstCustomer = customers.First();

            Assert.IsNotNull(firstCustomer.FirstName);
            Assert.IsNotNull(firstCustomer.LastName);
            Assert.IsNotNull(firstCustomer.Email);
            Assert.That(firstCustomer.OutstandingRentals, Is.GreaterThan(0));
        }
    }
}
