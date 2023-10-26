using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Litmus.Core.Database;
using Litmus.Core.Logging;
using NUnit.Framework;
using Sakila.Data;

namespace Sakila.Test.Data
{
    //THIS TEST FIXTURE REQUIRES A FRESH UNMODIFIED DOWNLOAD OF THE SAKILA DB
    [TestFixture]
    public class MovieRepositoryFixture
    {
        private readonly MovieRepository repository;

        public MovieRepositoryFixture()
        {
            repository = new MovieRepository(new SakilaSqliteDatabaseConnection(new ConsoleStructuredLogger()));
        }

        [Test]
        public async Task GetMovieByIdAsyncGoodIdReturnsCorrectResult()
        {
            var movie = await repository.GetMovieByIdAsync(1,CancellationToken.None);

            Assert.IsNotNull(movie);

            Assert.That(movie.MovieId, Is.EqualTo(1));
            Assert.That(movie.Title, Is.EqualTo("ACADEMY DINOSAUR"));
            Assert.That(movie.Length, Is.EqualTo(86));
            Assert.That(movie.ReleaseYear, Is.EqualTo(2006));
            Assert.That(movie.Rating, Is.EqualTo("PG"));
        }

        [Test]
        public async Task GetMovieByIdAsyncBadIdReturnsCorrectResult()
        {
            var movie = await repository.GetMovieByIdAsync(56447844,CancellationToken.None);

            Assert.IsNull(movie);
        }
    }
}
