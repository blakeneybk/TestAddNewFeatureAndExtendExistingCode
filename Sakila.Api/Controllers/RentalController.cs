using System;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Litmus.Core.AspNetCore.Documentation;
using Litmus.Core.Logging;
using Microsoft.AspNetCore.Http;
using Sakila.Data;
using Sakila.Models;

namespace Sakila.Api.Controllers
{
    [Route("api/rental")]
    [PublicApi]
    public class RentalController : ControllerBase
    {
        private readonly RentalRepository _rentalRepository;
        private readonly IStructuredLogger _logger;

        public RentalController(IStructuredLogger logger, RentalRepository repository)
        {
            this._rentalRepository = repository;
            this._logger = logger;
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
                return Ok(await _rentalRepository.GetOutstandingRentalsByCustomerId(customerId, cancellationToken));
            }
            catch (Exception e)
            {
                _logger.Error(e, e.Message);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }

        }

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
