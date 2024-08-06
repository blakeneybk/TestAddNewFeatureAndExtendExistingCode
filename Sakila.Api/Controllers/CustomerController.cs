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
    /// Controller for managing customer-related operations.
    /// </summary>
    [Route("api/customer")]
    [PublicApi]
    public class CustomerController : ControllerBase
    {
        private readonly CustomerRepository customerRepository;
        private readonly OutstandingRentalsRepository outstandingRentalsRepository;
        private readonly StoreRepository storeRepository;
        private readonly IStructuredLogger logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomerController"/> class.
        /// </summary>
        /// <param name="logger">The structured logger instance.</param>
        /// <param name="customerRepository">The customer repository instance.</param>
        /// <param name="outstandingRentalsRepository">The outstanding rentals repository instance.</param>
        /// <param name="storeRepository">The store repository instance.</param>
        public CustomerController(
            IStructuredLogger logger,
            CustomerRepository customerRepository,
            OutstandingRentalsRepository outstandingRentalsRepository,
            StoreRepository storeRepository)
        {
            this.logger = logger;
            this.customerRepository = customerRepository;
            this.outstandingRentalsRepository = outstandingRentalsRepository;
            this.storeRepository = storeRepository;
        }

        /// <summary>
        /// Returns details for a specific customer.
        /// </summary>
        /// <param name="customerId">Unique identifier of the customer.</param>
        /// <param name="cancellationToken">Cancellation token for the operation.</param>
        /// <returns>The details of the specified customer.</returns>
        /// <response code="200">Returns the customer details.</response>
        /// <response code="404">If the customer is not found.</response>
        /// <response code="500">If there is an internal server error.</response>
        [HttpGet("{customerId:int}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(CustomerDetails), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<CustomerDetails> GetCustomerDetail(int customerId, CancellationToken cancellationToken) =>
            await customerRepository.GetCustomerDetails(customerId, cancellationToken);

        /// <summary>
        /// Lists customers who currently have rentals checked out.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token for the operation.</param>
        /// <returns>A list of customers with outstanding rentals.</returns>
        /// <response code="200">Returns the list of customers with outstanding rentals.</response>
        [HttpGet("with-outstanding-rentals")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<CustomerOutstandingRentals>), StatusCodes.Status200OK)]
        public async Task<IEnumerable<CustomerOutstandingRentals>> CustomersWithOutstandingRentals(CancellationToken cancellationToken) =>
            await outstandingRentalsRepository.OutstandingRentals(cancellationToken);

        /// <summary>
        /// Lists customers who currently have rentals checked out, filtered by store ID.
        /// </summary>
        /// <param name="storeId">The store identifier.</param>
        /// <param name="cancellationToken">The cancellation token for the operation.</param>
        /// <returns>A list of customers with outstanding rentals at the specified store.</returns>
        /// <response code="200">Returns the list of customers with outstanding rentals at the specified store.</response>
        /// <response code="404">If the store is not found.</response>
        /// <response code="500">If there is an internal server error.</response>
        [HttpGet("with-outstanding-rentals/store/{storeId:int}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<CustomerOutstandingRentals>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CustomersWithOutstandingRentalsByStoreId(int storeId, CancellationToken cancellationToken)
        {
            try
            {
                if (await storeRepository.ValidateStoreIdAsync(storeId, cancellationToken))
                {
                    return Ok(await outstandingRentalsRepository.OutstandingRentalsByStoreAsync(storeId, cancellationToken));
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
