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
    /// Controller for managing movie categories.
    /// </summary>
    [Route("api/category")]
    [PublicApi]
    public class CategoryController : ControllerBase
    {
        private readonly CategoryRepository repository;
        private readonly IStructuredLogger logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="CategoryController"/> class.
        /// </summary>
        /// <param name="logger">The structured logger instance.</param>
        /// <param name="repository">The category repository instance.</param>
        public CategoryController(IStructuredLogger logger, CategoryRepository repository)
        {
            this.logger = logger;
            this.repository = repository;
        }

        /// <summary>
        /// Lists all the movie categories.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token for the operation.</param>
        /// <returns>A list of all movie categories.</returns>
        /// <response code="200">Returns the list of all categories.</response>
        [HttpGet]
        [Produces("application/json")]
        [ProducesResponseType(typeof(IEnumerable<Category>), StatusCodes.Status200OK)]
        public async Task<IEnumerable<Category>> GetAllCategories(CancellationToken cancellationToken)
            => await repository.GetAllCategories(cancellationToken);

        /// <summary>
        /// Gets the category by identifier.
        /// </summary>
        /// <param name="categoryId">The category identifier.</param>
        /// <param name="cancellationToken">The cancellation token for the operation.</param>
        /// <returns>The category with the specified identifier.</returns>
        /// <response code="200">Returns the category details.</response>
        /// <response code="404">If the category is not found.</response>
        /// <response code="500">If there is an internal server error.</response>
        [HttpGet("{categoryId:int}")]
        [Produces("application/json")]
        [ProducesResponseType(typeof(Category), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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
