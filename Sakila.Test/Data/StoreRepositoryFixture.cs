﻿using System.Linq;
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
    public class StoreRepositoryFixture
    {
        private readonly StoreRepository repository;

        public StoreRepositoryFixture()
        {
            repository = new StoreRepository(new SakilaSqliteDatabaseConnection(new ConsoleStructuredLogger()));
        }

        [Test]
        public async Task GetAllStoresReturnsExpectedValues()
        {
            var stores = await repository.GetAllStores(CancellationToken.None);

            CollectionAssert.IsNotEmpty(stores);
            var firstStore = stores.FirstOrDefault();

            Assert.That(firstStore.StoreId, Is.EqualTo(1));
            Assert.That(firstStore.Address, Is.EqualTo("47 MySakila Drive"));
            Assert.That(firstStore.Address2, Is.Null);
            Assert.That(firstStore.District, Is.EqualTo("Alberta"));
            Assert.That(firstStore.PostalCode, Is.Empty);
            Assert.That(firstStore.City, Is.EqualTo("Lethbridge"));
            Assert.That(firstStore.Country, Is.EqualTo("Canada"));
        }

        [Test]
        public async Task ValidateStoreIdAsyncReturnsExistingReturnsTrue()
        {
            Assert.IsTrue(await repository.ValidateStoreIdAsync(1, CancellationToken.None));
        }

        [Test]
        public async Task ValidateStoreIdAsyncReturnsNotExistingReturnsFalse()
        {
            Assert.IsFalse(await repository.ValidateStoreIdAsync(654986498, CancellationToken.None));
        }
    }
}
