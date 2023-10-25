using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Litmus.Core.Database;
using Sakila.Models;

namespace Sakila.Data
{
    public class StoreRepository
    {
        private readonly SakilaSqliteDatabaseConnection databaseConnection;

        public StoreRepository(SakilaSqliteDatabaseConnection databaseConnection)
        {
            this.databaseConnection = databaseConnection;
        }

        public async Task<IEnumerable<Store>> GetAllStores(CancellationToken cancellationToken)
        {
            var sql = @"
SELECT
    s.store_id AS StoreId,
    a.address AS Address,
    a.address2 AS Address2,
    a.district AS District,
    a.postal_code AS PostalCode,
    c.city AS City,
    c2.country AS Country
FROM store s
INNER JOIN address a on s.address_id = a.address_id
INNER JOIN city c on a.city_id = c.city_id
INNER JOIN country c2 on c.country_id = c2.country_id
";

            return await databaseConnection.QueryAsync<Store>(sql, cancellationToken: cancellationToken);
        }

        /// <summary>
        /// A lightweight resource validator for checking store ids
        /// </summary>
        /// <param name="storeId">The store identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        public async Task<bool> ValidateStoreIdAsync(int storeId, CancellationToken cancellationToken)
        {
            var sql = @"
SELECT COUNT(store_id)
FROM store
WHERE store_id = @StoreId";
            var parameters = new { StoreId = storeId };
            return await databaseConnection.ExecuteScalarAsync<int>(sql, parameters, cancellationToken) > 0;
        }
    }
}
