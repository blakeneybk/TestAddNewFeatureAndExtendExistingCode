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
    [Route("api/rental")]
    [PublicApi]
    public class RentalController : ControllerBase
    {
        private readonly RentalRepository rentalRepository;
        private readonly CustomerRepository customerRepository;
        private readonly IStructuredLogger logger;

        public RentalController(IStructuredLogger logger, RentalRepository rentalRepository, CustomerRepository customerRepository)
        {
            this.rentalRepository = rentalRepository;
            this.customerRepository = customerRepository;
            this.logger = logger;
        }

        /// <summary>
        /// Gets a list of outstanding rentals for a customer
        /// </summary>
        /// <param name="customerId">Unique identifier of customer</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("{customerId:int}")]
        public async Task<IActionResult> GetOutstandingRentalsByCustomerId(int customerId,
            CancellationToken cancellationToken)
        {
            try
            {
                // best practice to only return empty results if the resource is actually valid, so let's check first
                if (await customerRepository.ValidateCustomerId(customerId, cancellationToken))
                {
                    return Ok(await rentalRepository.GetOutstandingRentalsByCustomerId(customerId, cancellationToken));
                }

                return NotFound();
            }
            catch (Exception e)
            {
                logger.Error(e, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

        }

        /// <summary>
        /// Sets the rental as returned.
        /// </summary>
        /// <param name="rentalId">The rental identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        [HttpPut("{rentalId:int}/return")]
        public async Task<IActionResult> SetRentalAsReturned(int rentalId, CancellationToken cancellationToken)
        {
            try
            {
                var updated = false;
                var rental = await rentalRepository.GetRentalById(rentalId, cancellationToken);

                if (rental == null)
                {
                    return NotFound();
                }

                if (rental.ReturnDate is null)
                {
                    updated = await rentalRepository.UpdateRentalReturnDate(rentalId, DateTime.Now, cancellationToken);
                }

                //return an updated array of rentals so that the FE doesn't need to make a second API call or refresh to update the view
                return updated ? Ok(await rentalRepository.GetOutstandingRentalsByCustomerId(rental.CustomerId, cancellationToken)) : Conflict();
            }
            catch (Exception e)
            {
                logger.Error(e, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
