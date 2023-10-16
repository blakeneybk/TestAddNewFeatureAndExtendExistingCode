using System.Data.Common;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Litmus.Core.Logging;
using Microsoft.Data.Sqlite;

namespace Litmus.Core.Database
{
    public class SakilaSqliteDatabaseConnection : IDatabaseConnection
    {
        private readonly IStructuredLogger structuredLogger;
        private readonly string localDatabasePath;

        // Url of a sqlite database preloaded with sakila
        private const string DatabaseUrl = "https://github.com/siara-cc/sakila_sqlite3/raw/master/sakila.db";

        public SakilaSqliteDatabaseConnection(IStructuredLogger structuredLogger)
        {
            this.structuredLogger = structuredLogger;
            localDatabasePath = Path.Combine(Path.GetTempPath(), "Litmus", "sakila.db");
        }

        public async Task<DbConnection> OpenConnection(CancellationToken cancellationToken)
        {
            await EnsureDatabaseIsDownloaded(cancellationToken);

            var connection = new SqliteConnection($"Data Source={localDatabasePath}");
            await connection.OpenAsync(cancellationToken);

            return connection;
        }

        private async Task EnsureDatabaseIsDownloaded(CancellationToken cancellationToken)
        {
            var parentDirectory = Path.GetDirectoryName(localDatabasePath);
            if (!Directory.Exists(parentDirectory))
            {
                Directory.CreateDirectory(parentDirectory);
            }

            if (!File.Exists(localDatabasePath))
            {
                structuredLogger.Debug("Database file is missing from {localDatabasePath}. Downloading from {DatabaseUrl}", localDatabasePath, DatabaseUrl);

                using (var client = new WebClient())
                using (cancellationToken.Register(() => client.CancelAsync()))
                {
                    await client.DownloadFileTaskAsync(DatabaseUrl, localDatabasePath);
                }
            }
        }

        public void RemoveDatabaseFile()
        {
            if (File.Exists(localDatabasePath))
            {
                structuredLogger.Debug("Cleaning up database file from {localDatabasePath}", localDatabasePath);
                File.Delete(localDatabasePath);
            }
        }
    }
}
