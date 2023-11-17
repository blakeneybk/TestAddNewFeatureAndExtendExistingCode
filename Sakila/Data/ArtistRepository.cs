using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Litmus.Core.Database;
using Sakila.Models;

namespace Sakila.Data
{
    public class ArtistRepository
    {
        private readonly SakilaSqliteDatabaseConnection databaseConnection;

        public ArtistRepository(SakilaSqliteDatabaseConnection databaseConnection)
        {
            this.databaseConnection = databaseConnection;
        }

        public async Task<IEnumerable<ArtistDetails>> GetAllArtists(CancellationToken cancellationToken)
        {
            const string sql = @"
SELECT
    actor_id as ArtistId,
    first_name as FirstName,
    last_name as LastName
FROM actor
";
            return await databaseConnection.QueryAsync<ArtistDetails>(sql, cancellationToken: cancellationToken);
        }

        public async Task<ArtistDetails> GetArtistByIdAsync(int artistId, CancellationToken cancellationToken)
        {
            const string sql = @"
SELECT
    actor_id as ArtistId,
    first_name as FirstName,
    last_name as LastName
FROM actor
WHERE actor_id = @ActorId
";
            //artists are actually actors in the database
            var parameters = new { ActorId = artistId };
            return (await databaseConnection.QueryAsync<ArtistDetails>(sql, parameters, cancellationToken: cancellationToken)).FirstOrDefault();
        }
    }
}
