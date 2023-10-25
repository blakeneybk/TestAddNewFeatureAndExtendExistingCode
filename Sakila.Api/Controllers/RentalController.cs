﻿using System;
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
        private readonly RentalRepository _rentalRepository;
        private readonly CustomerRepository _customerRepository;
        private readonly IStructuredLogger _logger;

        public RentalController(IStructuredLogger logger, RentalRepository repository, CustomerRepository customerRepository)
        {
            _rentalRepository = repository;
            _customerRepository = customerRepository;
            _logger = logger;
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
                if (await _customerRepository.ValidateCustomerId(customerId, cancellationToken))
                {
                    return Ok(await _rentalRepository.GetOutstandingRentalsByCustomerId(customerId, cancellationToken));
                }

                return NotFound();
            }
            catch (Exception e)
            {
                _logger.Error(e, e.Message);
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
                var rental = await _rentalRepository.GetRentalById(rentalId, cancellationToken);

                if (rental == null)
                {
                    return NotFound();
                }

                if (rental.ReturnDate is null)
                {
                    updated = await _rentalRepository.UpdateRentalReturnDate(rentalId, DateTime.Now, cancellationToken);
                }

                //return an updated array of rentals so that the FE doesn't need to make a second API call or refresh to update the view
                return updated ? Ok(await _rentalRepository.GetOutstandingRentalsByCustomerId(rental.CustomerId, cancellationToken)) : Conflict();
            }
            catch (Exception e)
            {
                _logger.Error(e, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
