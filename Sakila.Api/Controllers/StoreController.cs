using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Litmus.Core.AspNetCore.Documentation;
using Microsoft.AspNetCore.Mvc;
using Sakila.Data;
using Sakila.Models;

namespace Sakila.Api.Controllers
{

    [Route("api/store")]
    [PublicApi]
    public class StoreController : ControllerBase
    {
        private readonly StoreRepository repository;

        public StoreController(StoreRepository repository)
        {
            this.repository = repository;
        }

        /// <summary>
        /// List of all the stores
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet]
        public Task<IEnumerable<Store>> GetAllStores(CancellationToken cancellationToken)
            => repository.GetAllStores(cancellationToken);
    }
}
