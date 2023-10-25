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
    favoriteActor.actor_id AS favoriteArtistId,
    favoriteFilm.film_id AS FavoriteMovieId,
    favoriteCategory.category_id AS FavoriteCategoryId
FROM customer c
LEFT JOIN
(
    SELECT fa.customer_id, fa.actor_id
    FROM (
             SELECT r.customer_id, fa.actor_id, COUNT(r.rental_id) rental_count
             FROM rental r
             INNER JOIN inventory i ON r.inventory_id = i.inventory_id
             INNER JOIN film f ON i.film_id = f.film_id
             INNER JOIN film_actor fa ON f.film_id = fa.film_id
             WHERE r.customer_id = @CustomerId
             GROUP BY r.customer_id, fa.actor_id
             ORDER BY rental_count DESC
             LIMIT 1
         ) AS fa
) favoriteActor ON c.customer_id = favoriteActor.customer_id
LEFT JOIN
(
    SELECT fm.customer_id, fm.film_id
    FROM 
        (
            SELECT r.customer_id, i.film_id, COUNT(r.rental_id) rental_count
            FROM rental r
            INNER JOIN inventory i ON r.inventory_id = i.inventory_id
            WHERE r.customer_id = @CustomerId 
            GROUP BY r.customer_id, i.film_id
            ORDER BY rental_count DESC
            LIMIT 1
        ) AS fm
) favoriteFilm ON c.customer_id = favoriteFilm.customer_id
LEFT JOIN
(
    SELECT fc.customer_id, fc.category_id
    FROM 
        (
            SELECT r.customer_id, fc.category_id, COUNT(r.rental_id) AS rental_count
            FROM rental r
            INNER JOIN inventory i ON r.inventory_id = i.inventory_id
            INNER JOIN film_category fc ON i.film_id = fc.film_id
            WHERE r.customer_id = @CustomerId 
            GROUP BY r.customer_id, fc.category_id
            ORDER BY rental_count DESC
            LIMIT 1
        ) AS fc
) favoriteCategory ON c.customer_id = favoriteCategory.customer_id
WHERE c.customer_id = @CustomerId;";
            var parameters = new { CustomerId = customerId };
            var results = await databaseConnection.QueryAsync<CustomerDetails>(sql, parameters, cancellationToken);
            return results.FirstOrDefault();
        }
    }
}
