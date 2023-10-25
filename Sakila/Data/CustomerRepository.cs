using System.Threading;
using System.Threading.Tasks;
using Litmus.Core.Database;
using Sakila.Models;
using System.Linq;

namespace Sakila.Data
{
    public class CustomerRepository
    {
        private readonly SakilaSqliteDatabaseConnection databaseConnection;

        public CustomerRepository(SakilaSqliteDatabaseConnection databaseConnection)
        {
            this.databaseConnection = databaseConnection;
        }

        /// <summary>
        /// A lightweight resource validator for checking customer ids
        /// </summary>
        /// <param name="customerId">The store identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public async Task<bool> ValidateCustomerId(int customerId, CancellationToken cancellationToken)
        {
            var sql = @"
SELECT COUNT(customer_id)
FROM customer
WHERE customer_id = @CustomerId";
            var parameters = new { CustomerId = customerId };
            return await databaseConnection.ExecuteScalarAsync<int>(sql, parameters, cancellationToken) > 0;
        }

        public async Task<CustomerDetails> GetCustomerDetails(int customerId, CancellationToken cancellationToken)
        {
            var sql = @"
SELECT
       c.customer_id AS CustomerId,
       c.first_name AS FirstName,
       c.last_name AS LastName,
       c.email AS Email,
       favoriteActor.actor_id AS FavoriteArtistId
FROM customer c
LEFT JOIN
(
    SELECT customer_id, actor_id
    FROM (
             SELECT r.customer_id, fa.actor_id, COUNT(r.rental_id) rental_count
             FROM rental r
             INNER JOIN inventory i on r.inventory_id = i.inventory_id
             INNER JOIN film f on i.film_id = f.film_id
             INNER JOIN film_actor fa on f.film_id = fa.film_id
             -- Where filter is an optimization to improve query performance
             WHERE r.customer_id = @customerId
             GROUP BY fa.actor_id, r.customer_id
         ) AS fa
    ORDER BY rental_count DESC
    LIMIT 1
) favoriteActor ON favoriteActor.customer_id = c.customer_id
WHERE c.customer_id = @customerId
";

            var results = await databaseConnection.QueryAsync<CustomerDetails>(sql, new {customerId}, cancellationToken);
            return results.FirstOrDefault();
        }
    }
}
