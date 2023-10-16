using System;
using System.IO;
using Litmus.Core.AspNetCore.Documentation.Filters;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace Litmus.Core.AspNetCore.Documentation
{
    public static class SwaggerAppStartupExtensions
    {
        /// <summary>
        /// Setup swagger to get our standard Litmus setup
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddLitmusSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo() {Title = "Litmus"});

                var expectedXmlDocPath = Path.Combine(AppContext.BaseDirectory, "Documentation.XML");
                if (File.Exists(expectedXmlDocPath))
                {
                    c.IncludeXmlComments(expectedXmlDocPath);
                }

                c.OperationFilter<SwaggerIgnoreAttributeFilter>();
                c.DocumentFilter<PublicApiAttributeFilter>();
            });

            // This should be what we are doing now as the project migrated away from newtonsoft.
            services.AddSwaggerGenNewtonsoftSupport();

            return services;
        }

        /// <summary>
        /// Setup required routes and UI elements for Swagger
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseLitmusSwagger(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.IndexStream = () => typeof(SwaggerAppStartupExtensions).Assembly.GetManifestResourceStream("Litmus.Core.AspNetCore.Documentation.index.html");
            });

            return app;
        }
    }
}
