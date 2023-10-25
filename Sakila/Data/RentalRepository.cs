using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Litmus.Core.Database;
using Sakila.Models;

namespace Sakila.Data
{
    public class RentalRepository
    {
        private readonly SakilaSqliteDatabaseConnection databaseConnection;

        public RentalRepository(SakilaSqliteDatabaseConnection databaseConnection)
        {
            this.databaseConnection = databaseConnection;
        }

        public async Task<IEnumerable<Rental>> GetOutstandingRentalsByCustomerId(int customerId, CancellationToken cancellationToken)
        {
            var sql = @"
SELECT
    r.rental_id AS RentalId,
    i.store_id AS StoreId,
    r.customer_id AS CustomerId,
    f.title AS Title,
    store_city.city AS StoreName,
    r.rental_date AS RentalDate,
    DATETIME(r.rental_date, '+' || f.rental_duration || ' DAYS') AS DueDate,
    r.return_date AS ReturnDate,
    r.staff_id AS StaffId
FROM rental r
INNER JOIN inventory i ON r.inventory_id = i.inventory_id
INNER JOIN film f ON i.film_id = f.film_id
INNER JOIN (
	SELECT s.store_id, ci.city
	FROM store s
	INNER JOIN address a ON s.address_id = a.address_id
	INNER JOIN city ci ON a.city_id = ci.city_id
) store_city ON i.store_id = store_city.store_id
WHERE r.customer_id = @CustomerId
AND r.return_date IS NULL
ORDER BY f.title ASC";
            var parameters = new { CustomerId = customerId };
            return await databaseConnection.QueryAsync<Rental>(sql, parameters, cancellationToken: cancellationToken);
        }

        public async Task<Rental> GetRentalById(int rentalId, CancellationToken cancellationToken)
        {
            var sql = @"
SELECT
    r.rental_id AS RentalId,
    i.store_id AS StoreId,
    r.customer_id AS CustomerId,
    f.title AS Title,
    store_city.city AS StoreName,
    r.rental_date AS RentalDate,
    DATETIME(r.rental_date, '+' || f.rental_duration || ' DAYS') AS DueDate,
    r.return_date AS ReturnDate,
    r.staff_id AS StaffId
FROM rental r
INNER JOIN inventory i ON r.inventory_id = i.inventory_id
INNER JOIN film f ON i.film_id = f.film_id
INNER JOIN (
	SELECT s.store_id, ci.city
	FROM store s
	INNER JOIN address a ON s.address_id = a.address_id
	INNER JOIN city ci ON a.city_id = ci.city_id
) store_city ON i.store_id = store_city.store_id
WHERE r.rental_id = @RentalId";
            var parameters = new { RentalId = rentalId };
            return (await databaseConnection.QueryAsync<Rental>(sql, parameters, cancellationToken: cancellationToken))
                .FirstOrDefault();
        }

        public async Task<bool> UpdateRentalReturnDate(int rentalId, DateTime? returnDate, CancellationToken cancellationToken)
        {
            var sql = @"
UPDATE rental
SET return_date = @ReturnDate
WHERE rental_id = @RentalId";
            var parameters = new { ReturnDate = returnDate, RentalId = rentalId };
            var affectedRows = await databaseConnection.ExecuteAsync(sql, parameters, cancellationToken: cancellationToken);
    
            return affectedRows > 0;
        }
    }
}
