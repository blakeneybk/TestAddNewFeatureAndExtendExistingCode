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
        public async Task<bool> ValidateCustomerIdAsync(int customerId, CancellationToken cancellationToken)
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
            var parameters = new { CustomerId = customerId };
            // Query customer details first
            const string customerSql = @"
                SELECT
                    customer_id AS CustomerId,
                    first_name AS FirstName,
                    last_name AS LastName,
                    email AS Email
                FROM customer
                WHERE customer_id = @CustomerId";

            var customer = (await databaseConnection.QueryAsync<CustomerDetails>(customerSql, parameters, cancellationToken)).FirstOrDefault();

            // If the customer does not exist, return null
            if (customer == null) return null;

            var favoriteInfo = new FavoriteInfo();

            // SQL query to identify the actor associated with the films most rented by a specific customer. 
            // It calculates this based on the frequency of rentals for films that certain actors star in.
            const string favoriteActorSql = @"
                SELECT fa.actor_id
                FROM rental r
                INNER JOIN inventory i ON r.inventory_id = i.inventory_id
                INNER JOIN film f ON i.film_id = f.film_id
                INNER JOIN film_actor fa ON f.film_id = fa.film_id
                -- Where filter is an optimization to improve query performance
                WHERE r.customer_id = @CustomerId
                GROUP BY fa.actor_id
                ORDER BY COUNT(r.rental_id) DESC
                LIMIT 1";
            
            favoriteInfo.ActorId = await GetFavoriteEntityIdAsync(favoriteActorSql);

            // SQL query to retrieve the film ID most frequently rented by a specific customer, 
            // considering all inventory items as instances of the same film, not as unique items.
            const string favoriteFilmSql = @"
                SELECT i.film_id
                FROM rental r
                INNER JOIN inventory i ON r.inventory_id = i.inventory_id
                WHERE r.customer_id = @CustomerId
                GROUP BY i.film_id
                ORDER BY COUNT(r.rental_id) DESC
                LIMIT 1";

            favoriteInfo.FilmId = await GetFavoriteEntityIdAsync(favoriteFilmSql);

            // SQL query to determine the customer's favorite film category based on their rental history.
            // This identifies the most frequently rented category by counting rentals within each film category for the customer.
            const string favoriteCategorySql = @"
                SELECT fc.category_id
                FROM rental r
                INNER JOIN inventory i ON r.inventory_id = i.inventory_id
                INNER JOIN film_category fc ON i.film_id = fc.film_id
                WHERE r.customer_id = @CustomerId
                GROUP BY fc.category_id
                ORDER BY COUNT(r.rental_id) DESC
                LIMIT 1";

            favoriteInfo.CategoryId = await GetFavoriteEntityIdAsync(favoriteCategorySql);

            // Populate the customer details object with favorite information
            customer.FavoriteArtistId = favoriteInfo.ActorId;
            customer.FavoriteMovieId = favoriteInfo.FilmId;
            customer.FavoriteCategoryId = favoriteInfo.CategoryId;

            return customer;

            async Task<int> GetFavoriteEntityIdAsync(string query)
            {
                return await databaseConnection.ExecuteScalarAsync<int>(query, parameters, cancellationToken);
            }
        }
    }
}
