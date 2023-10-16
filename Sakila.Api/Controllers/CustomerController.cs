using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Litmus.Core.AspNetCore.Documentation;
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

        public CustomerController(
            CustomerRepository customerRepository,
            OutstandingRentalsRepository outstandingRentalsRepository)
        {
            this.customerRepository = customerRepository;
            this.outstandingRentalsRepository = outstandingRentalsRepository;
        }

        /// <summary>
        /// Returns details for a specific customer
        /// </summary>
        /// <param name="customerId">Unique identifier of customer</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("{customerId:int}")]
        public Task<CustomerDetails> GetCustomerDetail(int customerId, CancellationToken cancellationToken) =>
            customerRepository.GetCustomerDetails(customerId, cancellationToken);

        /// <summary>
        /// List customers which currently have rentals checked out
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet("with-outstanding-rentals")]
        public Task<IEnumerable<CustomerOutstandingRentals>> CustomersWithOutstandingRentals(CancellationToken cancellationToken) =>
            outstandingRentalsRepository.OutstandingRentals(cancellationToken);
    }
}
