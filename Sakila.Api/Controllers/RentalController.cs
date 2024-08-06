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
    /// <summary>
    /// Controller for managing rental operations.
    /// </summary>
    [Route("api/rental")]
    [PublicApi]
    public class RentalController : ControllerBase
    {
        private readonly RentalRepository rentalRepository;
        private readonly CustomerRepository customerRepository;
        private readonly IStructuredLogger logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="RentalController"/> class.
        /// </summary>
        /// <param name="logger">The structured logger.</param>
        /// <param name="rentalRepository">The rental repository.</param>
        /// <param name="customerRepository">The customer repository.</param>
        public RentalController(IStructuredLogger logger, RentalRepository rentalRepository, CustomerRepository customerRepository)
        {
            this.rentalRepository = rentalRepository;
            this.customerRepository = customerRepository;
            this.logger = logger;
        }

        /// <summary>
        /// Gets a list of outstanding rentals for a customer.
        /// </summary>
        /// <param name="customerId">Unique identifier of the customer.</param>
        /// <param name="cancellationToken">Cancellation token for the operation.</param>
        /// <returns>A list of outstanding rentals for the specified customer.</returns>
        /// <response code="200">Returns the list of outstanding rentals.</response>
        /// <response code="404">If the customer is not found.</response>
        /// <response code="500">If there is an internal server error.</response>
        [HttpGet("{customerId:int}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetOutstandingRentalsByCustomerId(int customerId, CancellationToken cancellationToken)
        {
            try
            {
                if (await customerRepository.ValidateCustomerIdAsync(customerId, cancellationToken))
                {
                    return Ok(await rentalRepository.GetOutstandingRentalsByCustomerIdAsync(customerId, cancellationToken));
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
        /// <param name="cancellationToken">Cancellation token for the operation.</param>
        /// <returns>An updated list of outstanding rentals for the customer.</returns>
        /// <response code="200">Returns the updated list of outstanding rentals.</response>
        /// <response code="404">If the rental is not found.</response>
        /// <response code="409">If the rental return update fails due to a conflict.</response>
        /// <response code="500">If there is an internal server error.</response>
        [HttpPut("{rentalId:int}/return")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> SetRentalAsReturned(int rentalId, CancellationToken cancellationToken)
        {
            try
            {
                var updated = false;
                var rental = await rentalRepository.GetRentalByIdAsync(rentalId, cancellationToken);

                if (rental == null)
                {
                    return NotFound();
                }

                if (rental.ReturnDate is null)
                {
                    updated = await rentalRepository.UpdateRentalReturnDateAsync(rentalId, DateTime.Now, cancellationToken);
                }

                //return an updated array of rentals so that the FE doesn't need to make a second API call or refresh to update the view
                return updated ? Ok(await rentalRepository.GetOutstandingRentalsByCustomerIdAsync(rental.CustomerId, cancellationToken)) : Conflict();
            }
            catch (Exception e)
            {
                logger.Error(e, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
