using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Dapper;

namespace Litmus.Core.Database
{
    /// <summary>
    /// We use MySql (AWS Aurora) and Dapper at Litmus.  While our production database code is a bit more complex, this is representative of what
    /// our developer experience is like from a data access perspective
    /// </summary>
    public static class DatabaseExtensions
    {
        /// <summary>
        /// Executes a sql command
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="sql">Query to execute</param>
        /// <param name="param">Parameters for query</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Number of rows impacted</returns>
        public static async Task<int> ExecuteAsync(this IDatabaseConnection connectionString, string sql, object param = null, CancellationToken cancellationToken = default)
        {
            using (var connection = await connectionString.OpenConnection(cancellationToken: cancellationToken))
            {
                var results = await connection.ExecuteAsync(sql, param);
                connection.Close();
                return results;
            }
        }

        /// <summary>
        /// Executes a sql command and maps the resulting data to the type specified by TQuery
        /// </summary>
        /// <typeparam name="TQuery">The type to map the data result to</typeparam>
        /// <param name="connectionString"></param>
        /// <param name="sql">SQL command to execute</param>
        /// <param name="param">Parameters for query</param>
        /// <param name="cancellationToken"></param>
        /// <returns>Returns of query mapped to the specified type</returns>
        public static async Task<IEnumerable<TQuery>> QueryAsync<TQuery>(this IDatabaseConnection connectionString, string sql, object param = null, CancellationToken cancellationToken = default)
        {
            using (var connection = await connectionString.OpenConnection(cancellationToken: cancellationToken))
            {
                var results = await connection.QueryAsync<TQuery>(sql, param);
                connection.Close();
                return results;
            }
        }
    }
}
