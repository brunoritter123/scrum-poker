using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using ScrumPoker.API.Configurations;
using ScrumPoker.API.HubConfig;
using ScrumPoker.Application.Configurations;
using ScrumPoker.CrossCutting.Services;
using ScrumPoker.Data;
using System.IO;

namespace ScrumPoker.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(opt =>
            {
                opt.AddPolicy("CorsPolicy", builder => builder
                .WithOrigins(new[]
                    {
                        "http://localhost:4200",
                        "http://localhost:5000",
                        "https://scrum-poker-br.herokuapp.com",
                        "http://scrum-poker-br.herokuapp.com",
                        "http://scrumpoker.com.br",
                        "https://scrumpoker.com.br",
                        "http://www.scrumpoker.com.br",
                        "https://www.scrumpoker.com.br",
                        "http://*.scrumpoker.com.br",
                        "https://*.scrumpoker.com.br",
                        "https://master.d3reu51kx5y6mw.amplifyapp.com",
                        "https://master.d3reu51kx5y6mw.amplifyapp.com",
                        "https://*.amplifyapp.com",
                        "https://*.amplifyapp.com",
                        "https://*.herokuapp.com",
                        "http://*.herokuapp.con"
                    })
                .SetIsOriginAllowedToAllowWildcardSubdomains()
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials()
                );
            });

            services.AddSignalR();

            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "wwwroot";
            });

            services.AddControllers()
                .AddNewtonsoftJson(options => options.SerializerSettings.ReferenceLoopHandling =
                Newtonsoft.Json.ReferenceLoopHandling.Ignore);

            services.AddApplicationServicesConfig();
            services.AddDependencyDbContext(Configuration);

            services.Configure<EmailConfig>(Configuration.GetSection("EmailConfig"));
            services.AddAutoMapper(typeof(AutoMapperApiConfig));
            services.AdicionarInjecaoDependenciaConfig();
            services.AdicionarIdentityConfig(Configuration);
            services.AdicionarVersionamentoConfig();
            services.AdicionarSwaggerConfig();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors("CorsPolicy");

            app.UseRouting();

            app.UseAuthorization();
            app.UseAuthentication();

            app.UsarSwaggerConfig();

            if (env.IsProduction())
            {
                app.UseHttpsRedirection();
                app.UseDefaultFiles();

                app.UseStaticFiles(new StaticFileOptions
                {
                    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")),
                });

                app.UseSpaStaticFiles();
            }

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<SalaHub>("/sala-hub");
                endpoints.MapFallbackToFile("/index.html");
            });
        }
    }
}
