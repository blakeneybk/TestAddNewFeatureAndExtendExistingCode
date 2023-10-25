using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Litmus.Core.Database;
using Sakila.Models;

namespace Sakila.Data
{
    public class RentalRepository
    {
        private readonly SakilaSqliteDatabaseConnection _databaseConnection;

        public RentalRepository(SakilaSqliteDatabaseConnection databaseConnection)
        {
            this._databaseConnection = databaseConnection;
        }

        public async Task<IEnumerable<Rental>> GetOutstandingRentalsByCustomerId(int customerId, CancellationToken cancellationToken)
        {
            var sql = $@"
SELECT
    r.rental_id AS RentalId,
    i.store_id AS StoreId,
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
WHERE r.customer_id = {customerId}
AND r.return_date IS NULL
ORDER BY f.title ASC";
    
            return await _databaseConnection.QueryAsync<Rental>(sql, cancellationToken: cancellationToken);
        }
    }
}
