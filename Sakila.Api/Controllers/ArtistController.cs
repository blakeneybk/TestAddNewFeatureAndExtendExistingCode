using System;
using System.Collections.Generic;
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
    /// Controller for managing artist-related operations.
    /// </summary>
    [Route("api/artist")]
    [PublicApi]
    public class ArtistController : ControllerBase
    {
        private readonly IStructuredLogger logger;
        private readonly ArtistRepository artistRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="ArtistController"/> class.
        /// </summary>
        /// <param name="logger">The structured logger instance.</param>
        /// <param name="artistRepository">The artist repository instance.</param>
        public ArtistController(IStructuredLogger logger, ArtistRepository artistRepository)
        {
            this.logger = logger;
            this.artistRepository = artistRepository;
        }

        /// <summary>
        /// Gets all artists.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token for the operation.</param>
        /// <returns>A list of all artists.</returns>
        /// <response code="200">Returns the list of all artists.</response>
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<ArtistDetails>), StatusCodes.Status200OK)]
        public async Task<IEnumerable<ArtistDetails>> GetAll(CancellationToken cancellationToken)
            => await artistRepository.GetAllArtists(cancellationToken);

        /// <summary>
        /// Gets the artist by identifier.
        /// </summary>
        /// <param name="artistId">The artist identifier.</param>
        /// <param name="cancellationToken">The cancellation token for the operation.</param>
        /// <returns>The artist with the specified identifier.</returns>
        /// <response code="200">Returns the artist details.</response>
        /// <response code="404">If the artist is not found.</response>
        /// <response code="500">If there is an internal server error.</response>
        [HttpGet("{artistId:int}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(ArtistDetails), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetArtistById(int artistId, CancellationToken cancellationToken)
        {
            try
            {
                var artist = await artistRepository.GetArtistByIdAsync(artistId, cancellationToken);
                if (artist != null)
                {
                    return Ok(artist);
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
