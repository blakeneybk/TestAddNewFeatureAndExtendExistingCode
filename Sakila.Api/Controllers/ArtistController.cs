using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using Litmus.Core.AspNetCore.Documentation;
using Microsoft.AspNetCore.Mvc;
using Sakila.Data;
using Sakila.Models;

namespace Sakila.Api.Controllers
{
    [Route("api/artist")]
    [PublicApi]
    public class ArtistController : ControllerBase
    {
        private readonly ArtistRepository artistRepository;

        public ArtistController(ArtistRepository artistRepository)
        {
            this.artistRepository = artistRepository;
        }

        /// <summary>
        /// Returns all the artists
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet]
        public Task<IEnumerable<ArtistDetails>> GetAll(CancellationToken cancellationToken)
            => artistRepository.GetAllArtists(cancellationToken);
    }
}
