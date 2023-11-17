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
    public class MovieRepository
    {
        private readonly SakilaSqliteDatabaseConnection databaseConnection;

        public MovieRepository(SakilaSqliteDatabaseConnection databaseConnection)
        {
            this.databaseConnection = databaseConnection;
        }

        public async Task<MovieDetails> GetMovieByIdAsync(int movieId, CancellationToken cancellationToken)
        {
            const string sql = @"
SELECT
    film_id AS MovieId,
    title AS Title,
    release_year AS ReleaseYear,
    length AS Length,
    rating AS Rating
FROM
    film
WHERE
    film_id = @MovieId
";
            var parameters = new { MovieId = movieId };

            return (await databaseConnection.QueryAsync<MovieDetails>(sql, parameters, cancellationToken: cancellationToken)).FirstOrDefault();
        }

    }
}
