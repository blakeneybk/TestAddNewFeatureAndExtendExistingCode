using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Litmus.Core.Database;
using Sakila.Models;

namespace Sakila.Data
{
    public class OutstandingRentalsRepository
    {
        private readonly SakilaSqliteDatabaseConnection databaseConnection;

        public OutstandingRentalsRepository(SakilaSqliteDatabaseConnection databaseConnection)
        {
            this.databaseConnection = databaseConnection;
        }

        public async Task<IEnumerable<CustomerOutstandingRentals>> OutstandingRentals(CancellationToken cancellationToken)
        {
            var sql = @"
SELECT
       c.customer_id AS CustomerId,
       c.first_name AS FirstName,
       c.last_name AS LastName,
       c.email AS Email,
       p.rental_count AS OutstandingRentals
FROM customer c
INNER JOIN (
    SELECT 
        c.customer_id, 
        COUNT(r.rental_id) rental_count
    FROM customer c
    INNER JOIN rental r on c.customer_id = r.customer_id
    INNER JOIN inventory i on r.inventory_id = i.inventory_id
    INNER JOIN film f on i.film_id = f.film_id
    WHERE r.return_date IS NULL
    GROUP BY c.customer_id
) p ON p.customer_id = c.customer_id
ORDER BY p.rental_count DESC";

            return await databaseConnection.QueryAsync<CustomerOutstandingRentals>(sql, cancellationToken: cancellationToken);
        }
    }
}
