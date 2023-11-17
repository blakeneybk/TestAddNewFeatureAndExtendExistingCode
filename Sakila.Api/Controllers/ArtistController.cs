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
    [Route("api/artist")]
    [PublicApi]
    public class ArtistController : ControllerBase
    {
        private readonly IStructuredLogger logger;
        private readonly ArtistRepository artistRepository;

        public ArtistController(IStructuredLogger logger, ArtistRepository artistRepository)
        {
            this.logger = logger;
            this.artistRepository = artistRepository;
        }

        /// <summary>
        /// Returns all the artists
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IEnumerable<ArtistDetails>> GetAll(CancellationToken cancellationToken)
            => await artistRepository.GetAllArtists(cancellationToken);

        /// <summary>
        /// Gets the artist by identifier.
        /// </summary>
        /// <param name="artistId">The artist identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        [HttpGet("{artistId:int}")]
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
