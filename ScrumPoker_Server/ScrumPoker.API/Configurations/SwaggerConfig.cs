using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;

namespace ScrumPoker.API.Configurations
{
    public static class SwaggerConfig
    {
        public static void AdicionarSwaggerConfig(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "ScrumPoker",
                    Version = "v1",
                    Description = "ScrumPoker API Swagger",
                    //TermsOfService = new Uri(""),
                    Contact = new OpenApiContact
                    {
                        Name = "ScrumPoker",
                        Email = "brunolritter123@gmail.com",
                        //Url = new Uri(""),
                    },
                    License = new OpenApiLicense
                    {
                        Name = "Use MIT License",
                        //Url = new Uri(""),
                    }
                });
            });
        }

        public static void UsarSwaggerConfig(this IApplicationBuilder app)
        {
            if (app == null) throw new ArgumentNullException(nameof(app));

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "ScrumPoker");
            });
        }
    }
}