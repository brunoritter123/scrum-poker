using ScrumPoker.Data.Repositories;
using ScrumPoker.Domain.Interfaces.Repositories;
using ScrumPoker.Domain.Models;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace ScrumPoker.API.Configurations
{
    public static class InjecaoDependenciaConfig
    {
        public static void AdicionarInjecaoDependenciaConfig(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            services.AddTransient<ISalaRepository, SalaRepository>();
            services.AddScoped<ICartaRepository, CartaRepository>();
        }
    }
}