using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Litmus.Core.AspNetCore.Documentation;
using Microsoft.AspNetCore.Mvc;
using Sakila.Data;
using Sakila.Models;

namespace Sakila.Api.Controllers
{
    [Route("api/category")]
    [PublicApi]
    public class CategoryContoller : ControllerBase
    {
        private readonly CategoryRepository repository;

        public CategoryContoller(CategoryRepository repository)
        {
            this.repository = repository;
        }

        /// <summary>
        /// Lists all the movie categories
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet]
        public Task<IEnumerable<Category>> GetAllCategories(CancellationToken cancellationToken)
            => repository.GetAllCategories(cancellationToken);
    }
}
