using System;
using System.Threading;
using System.Threading.Tasks;
using Litmus.Core.AspNetCore.Documentation;
using Litmus.Core.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sakila.Data;
using Sakila.Models;

namespace Sakila.Api.Controllers
{
    /// <summary>
    /// Controller for managing movie-related operations.
    /// </summary>
    [Route("api/movie")]
    [PublicApi]
    public class MovieController : ControllerBase
    {
        private readonly MovieRepository repository;
        private readonly IStructuredLogger logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="MovieController"/> class.
        /// </summary>
        /// <param name="logger">The structured logger instance.</param>
        /// <param name="repository">The movie repository instance.</param>
        public MovieController(IStructuredLogger logger, MovieRepository repository)
        {
            this.logger = logger;
            this.repository = repository;
        }

        /// <summary>
        /// Gets the movie by identifier.
        /// </summary>
        /// <param name="movieId">The movie identifier.</param>
        /// <param name="cancellationToken">The cancellation token for the operation.</param>
        /// <returns>The movie with the specified identifier.</returns>
        /// <response code="200">Returns the movie details.</response>
        /// <response code="404">If the movie is not found.</response>
        /// <response code="500">If there is an internal server error.</response>
        [HttpGet("{movieId:int}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(MovieDetails), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetMovieById(int movieId, CancellationToken cancellationToken)
        {
            try
            {
                var movie = await repository.GetMovieByIdAsync(movieId, cancellationToken);
                if (movie != null)
                {
                    return Ok(movie);
                }

                return NotFound();
            }
            catch (Exception e)
            {
                logger.Error(e, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
