using System.Linq;
using System.Reflection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Litmus.Core.AspNetCore.Documentation.Filters
{
    // Opt in to swagger.  Checks to see if controller is decorated with PublicApiAttribute before allowing it to appear in swagger docs
    public class PublicApiAttributeFilter : IDocumentFilter
    {
        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            var filteredApis =
                context.ApiDescriptions
                    .Where(a =>
                    {
                        if (!a.TryGetMethodInfo(out MethodInfo mi))
                        {
                            return false;
                        }

                        return mi.GetCustomAttribute<PublicApiAttribute>() != null ||
                               mi.DeclaringType?.GetCustomAttribute<PublicApiAttribute>() != null;
                    });

            // Keep a copy of the current path list and then reset it
            var paths = swaggerDoc.Paths.ToDictionary(kv => kv.Key, kv => kv.Value);
            swaggerDoc.Paths.Clear();

            // Go through all api descriptions decorated with PublicApi attribute and restore those paths
            foreach (var apiDescription in filteredApis)
            {
                var route = "/" + apiDescription.RelativePath;
                if (paths.Any(p => p.Key == route))
                {
                    var path = paths.FirstOrDefault(p => p.Key == route);

                    if (!swaggerDoc.Paths.ContainsKey(path.Key))
                    {
                        swaggerDoc.Paths.Add(path.Key, path.Value);
                    }
                }
            }
        }
    }
}
