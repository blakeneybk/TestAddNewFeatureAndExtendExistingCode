using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Litmus.Core.AspNetCore.Documentation;
using Sakila.Data;
using Sakila.Models;

namespace Sakila.Api.Controllers
{
    [Route("api/rental")]
    [PublicApi]
    public class RentalController : ControllerBase
    {
        private readonly RentalRepository _rentalRepository;

        public RentalController(RentalRepository repository)
        {
            this._rentalRepository = repository;
        }

        /// <summary>
        /// Gets a list of outstanding rentals for a customer
        /// </summary>
        /// <param name="customerId">Unique identifier of customer</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("{customerId:int}")]
        public Task<IEnumerable<Rental>> GetOutstandingRentalsByCustomerId(int customerId, CancellationToken cancellationToken) =>
            _rentalRepository.GetOutstandingRentalsByCustomerId(customerId, cancellationToken);
    }
}
