using System.Linq;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Litmus.Core.AspNetCore.Documentation.Filters
{
    public class SwaggerIgnoreAttributeFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var excludedParameters = context.MethodInfo.GetParameters().Where(pi =>
                pi.CustomAttributes.Any(a => a.AttributeType == typeof(SwaggerIgnoreAttribute)));

            foreach (var parameterInfo in excludedParameters)
            {
                var parameter = operation.Parameters.FirstOrDefault(p => p.Name == parameterInfo.Name);
                if (parameter != null)
                {
                    operation.Parameters.Remove(parameter);
                }
            }
        }
    }
}
