using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;

namespace Litmus.Core.Database
{
    public class LocalSqliteDatabaseConnection : IDatabaseConnection
    {
        public async Task<DbConnection> OpenConnection(CancellationToken cancellationToken)
        {
            var connection = new SqliteConnection("Data Source=Sqlite.db");
            await connection.OpenAsync(cancellationToken);

            return connection;
        }
    }
}
