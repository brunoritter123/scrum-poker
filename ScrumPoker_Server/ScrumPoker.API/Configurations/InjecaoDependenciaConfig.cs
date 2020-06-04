using ScrumPoker.Data.Repositories;
using ScrumPoker.Domain.Interfaces.Repositories;
using ScrumPoker.Domain.Models;
using Microsoft.Extensions.DependencyInjection;
using System;
using Microsoft.AspNetCore.Identity.UI.Services;
using ScrumPoker.CrossCutting.Services;

namespace ScrumPoker.API.Configurations
{
    public static class InjecaoDependenciaConfig
    {
        public static void AdicionarInjecaoDependenciaConfig(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            services.AddScoped<ISalaRepository, SalaRepository>();
            services.AddScoped<ICartaRepository, CartaRepository>();
            services.AddScoped<IPerfilRepository, PerfilRepository>();

            services.AddTransient<IEmailSender, EmailSender>();
        }
    }
}