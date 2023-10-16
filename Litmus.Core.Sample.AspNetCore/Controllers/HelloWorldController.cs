using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Litmus.Core.AspNetCore.Documentation;
using Litmus.Core.Logging;

namespace Litmus.Core.Sample.AspNetCore.Controllers
{
    /// <summary>
    /// Basic hello world operations
    /// </summary>
    [Route("api/demo")]
    [PublicApi]
    public class HelloWorldController : ControllerBase
    {

        private readonly IStructuredLogger logger;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        public HelloWorldController(IStructuredLogger logger)
        {
            this.logger = logger;
        }

        /// <summary>
        /// Hello world API Sample
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("")]
        public Task<int> HelloWorld()
        {
            logger.Debug("Get HelloWorld was called");

            return Task.FromResult(1);
        }


        /// <summary>
        /// Hello world API Sample
        /// </summary>
        /// <param name="value">The value to return</param>
        /// <param name="hidden">This should not appear in swagger.</param>
        /// <returns></returns>
        [HttpPost]
        [Route("")]
        public Task<int> HelloWorldEcho(
            int value,
            [SwaggerIgnore] int hidden = 3)
        {
            logger.Debug("Post HelloWorld was called with {value} and {hidden}", value, hidden);

            return Task.FromResult(value);
        }
    }
}
