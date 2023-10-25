﻿using System;
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
    [Route("api/customer")]
    [PublicApi]
    public class CustomerController : ControllerBase
    {
        private readonly CustomerRepository customerRepository;
        private readonly OutstandingRentalsRepository outstandingRentalsRepository;
        private readonly StoreRepository storeRepository;
        private readonly IStructuredLogger logger;

        public CustomerController(IStructuredLogger logger,
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
        /// Returns details for a specific customer
        /// </summary>
        /// <param name="customerId">Unique identifier of customer</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("{customerId:int}")]
        public async Task<CustomerDetails> GetCustomerDetail(int customerId, CancellationToken cancellationToken) =>
            await customerRepository.GetCustomerDetails(customerId, cancellationToken);

        /// <summary>
        /// List customers which currently have rentals checked out
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("with-outstanding-rentals")]
        public async Task<IEnumerable<CustomerOutstandingRentals>> CustomersWithOutstandingRentals(CancellationToken cancellationToken) =>
            await outstandingRentalsRepository.OutstandingRentals(cancellationToken);

        /// <summary>
        /// List customers which currently have rentals checked out, filtered by store Id
        /// </summary>
        /// <param name="storeId">The store identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        [HttpGet("with-outstanding-rentals/store/{storeId:int}")]
        public async Task<IActionResult> CustomersWithOutstandingRentalsByStoreId(int storeId,
            CancellationToken cancellationToken)
        {
            try
            {
                // validate storeId
                if (await storeRepository.ValidateStoreIdAsync(storeId, cancellationToken))
                {
                    return Ok(await outstandingRentalsRepository.OutstandingRentalsByStoreAsync(storeId, cancellationToken));
                }
                return NotFound();
            }
            catch (Exception e)
            {
                logger.Error(e,e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
