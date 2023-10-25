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
    [Route("api/category")]
    [PublicApi]
    public class CategoryContoller : ControllerBase
    {
        private readonly CategoryRepository repository;
        private readonly IStructuredLogger logger;

        public CategoryContoller(IStructuredLogger logger, CategoryRepository repository)
        {
            this.logger = logger;
            this.repository = repository;
        }

        /// <summary>
        /// Lists all the movie categories
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IEnumerable<Category>> GetAllCategories(CancellationToken cancellationToken)
            => await repository.GetAllCategories(cancellationToken);

        /// <summary>
        /// Gets the category by identifier.
        /// </summary>
        /// <param name="categoryId">The category identifier.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        [HttpGet("{categoryId:int}")]
        public async Task<IActionResult> GetCategoryById(int categoryId, CancellationToken cancellationToken)
        {
            try
            {
                var category = await repository.GetCategoryByIdAsync(categoryId, cancellationToken);
                if (category != null)
                {
                    return Ok(category);
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
