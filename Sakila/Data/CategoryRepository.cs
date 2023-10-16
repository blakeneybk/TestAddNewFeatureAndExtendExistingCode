using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Litmus.Core.Database;
using Sakila.Models;

namespace Sakila.Data
{
    public class CategoryRepository
    {
        private readonly SakilaSqliteDatabaseConnection databaseConnection;

        public CategoryRepository(SakilaSqliteDatabaseConnection databaseConnection)
        {
            this.databaseConnection = databaseConnection;
        }

        public async Task<IEnumerable<Category>> GetAllCategories(CancellationToken cancellationToken)
        {
            var sql = @"
    SELECT
        category_id AS CategoryId,
        name
    FROM Category
";
            return await databaseConnection.QueryAsync<Category>(sql, cancellationToken: cancellationToken);
        }
    }
}
