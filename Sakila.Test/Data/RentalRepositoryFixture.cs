using System;
using System.Linq;
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
    public class RentalRepositoryFixture
    {
        private readonly RentalRepository repository;

        public RentalRepositoryFixture()
        {
            repository = new RentalRepository(new SakilaSqliteDatabaseConnection(new ConsoleStructuredLogger()));
        }

        [Test]
        public async Task GetOutstandingRentalsByCustomerIdAsyncGoodIdReturnsCorrectResult()
        {
            var rentals = await repository.GetOutstandingRentalsByCustomerIdAsync(75, CancellationToken.None);
            
            Assert.IsNotNull(rentals);
            CollectionAssert.IsNotEmpty(rentals);
            var firstRental = rentals.FirstOrDefault();
            Assert.IsNotNull(firstRental);

            Assert.That(firstRental.StoreId, Is.EqualTo(2));
            Assert.That(firstRental.Title, Is.EqualTo("LUST LOCK"));
            Assert.That(firstRental.StoreName, Is.EqualTo("Woodridge"));
            Assert.That(firstRental.StoreId, Is.EqualTo(2));
            Assert.That(firstRental.CustomerId, Is.EqualTo(75));
            Assert.That(firstRental.RentalDate, Is.EqualTo(DateTime.Parse("2006-02-14T15:16:03")));
            Assert.That(firstRental.DueDate, Is.EqualTo(DateTime.Parse("2006-02-17T15:16:03")));
            Assert.That(firstRental.ReturnDate, Is.Null);
            Assert.That(firstRental.StaffId, Is.EqualTo(1));
        }

        [Test]
        public async Task GetOutstandingRentalsByCustomerIdAsyncBadIdReturnsEmpty()
        {
            var rentals = await repository.GetOutstandingRentalsByCustomerIdAsync(755464654, CancellationToken.None);
            
            Assert.IsNotNull(rentals);
            CollectionAssert.IsEmpty(rentals);
        }

        [Test]
        public async Task GetRentalByIdAsyncGoodIdReturnsCorrect()
        {
            var rental = await repository.GetRentalByIdAsync(13534, CancellationToken.None);
            
            Assert.IsNotNull(rental);

            Assert.That(rental.StoreId, Is.EqualTo(2));
            Assert.That(rental.Title, Is.EqualTo("LUST LOCK"));
            Assert.That(rental.StoreName, Is.EqualTo("Woodridge"));
            Assert.That(rental.StoreId, Is.EqualTo(2));
            Assert.That(rental.CustomerId, Is.EqualTo(75));
            Assert.That(rental.RentalDate, Is.EqualTo(DateTime.Parse("2006-02-14T15:16:03")));
            Assert.That(rental.DueDate, Is.EqualTo(DateTime.Parse("2006-02-17T15:16:03")));
            Assert.That(rental.ReturnDate, Is.Null);
            Assert.That(rental.StaffId, Is.EqualTo(1));
        }

        [Test]
        public async Task GetRentalByIdAsyncBadIdReturnsCorrect()
        {
            var rental = await repository.GetRentalByIdAsync(465461851, CancellationToken.None);
            
            Assert.IsNull(rental);
        }

        [Test]
        public async Task UpdateRentalReturnDateAsync_GoodId_Return_UndoReturn_ReturnsTrue()
        {
            var now = DateTime.Now;
            bool isUpdated1 = await repository.UpdateRentalReturnDateAsync(13534, now, CancellationToken.None);
            Assert.IsTrue(isUpdated1);

            var rental = await repository.GetRentalByIdAsync(13534, CancellationToken.None);
            
            Assert.IsNotNull(rental);

            Assert.That(rental.StoreId, Is.EqualTo(2));
            Assert.That(rental.Title, Is.EqualTo("LUST LOCK"));
            Assert.That(rental.StoreName, Is.EqualTo("Woodridge"));
            Assert.That(rental.StoreId, Is.EqualTo(2));
            Assert.That(rental.CustomerId, Is.EqualTo(75));
            Assert.That(rental.RentalDate, Is.EqualTo(DateTime.Parse("2006-02-14T15:16:03")));
            Assert.That(rental.DueDate, Is.EqualTo(DateTime.Parse("2006-02-17T15:16:03")));
            Assert.That(rental.ReturnDate, Is.EqualTo(now));
            Assert.That(rental.StaffId, Is.EqualTo(1));

            bool isUpdated2 = await repository.UpdateRentalReturnDateAsync(13534, null, CancellationToken.None);
            Assert.IsTrue(isUpdated2);

            rental = await repository.GetRentalByIdAsync(13534, CancellationToken.None);
            
            Assert.IsNotNull(rental);

            Assert.That(rental.StoreId, Is.EqualTo(2));
            Assert.That(rental.Title, Is.EqualTo("LUST LOCK"));
            Assert.That(rental.StoreName, Is.EqualTo("Woodridge"));
            Assert.That(rental.StoreId, Is.EqualTo(2));
            Assert.That(rental.CustomerId, Is.EqualTo(75));
            Assert.That(rental.RentalDate, Is.EqualTo(DateTime.Parse("2006-02-14T15:16:03")));
            Assert.That(rental.DueDate, Is.EqualTo(DateTime.Parse("2006-02-17T15:16:03")));
            Assert.That(rental.ReturnDate, Is.Null);
            Assert.That(rental.StaffId, Is.EqualTo(1));
        }

        [Test]
        public async Task UpdateRentalReturnDateAsync_BadId_ReturnsFalse()
        {
            var now = DateTime.Now;
            bool isUpdated1 = await repository.UpdateRentalReturnDateAsync(65465465, now, CancellationToken.None);
            Assert.IsFalse(isUpdated1);

        }
    }
}
