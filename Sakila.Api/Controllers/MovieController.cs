using System;
using System.Threading;
using System.Threading.Tasks;
using Litmus.Core.AspNetCore.Documentation;
using Litmus.Core.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Sakila.Data;

namespace Sakila.Api.Controllers
{
    [Route("api/movie")]
    [PublicApi]
    public class MovieController : ControllerBase
    {
        private readonly MovieRepository repository;
        private readonly IStructuredLogger logger;

        public MovieController(IStructuredLogger logger, MovieRepository repository)
        {
            this.logger = logger;
            this.repository = repository;
        }

        /// <summary>
        /// Gets the movie by identifier.
        /// </summary>
        /// <param name="movieId">The movie identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        [HttpGet("{movieId:int}")]
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
