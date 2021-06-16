using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ScrumPoker.Data.Context;
using ScrumPoker.Data.Repositories;
using ScrumPoker.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrumPoker.Data
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDependencyDbContext(this IServiceCollection service,
            IConfiguration configuration)
        {
            service.AddDbContext<ScrumPokerContext>(options =>
                options.UseNpgsql(configuration.GetSection("ConnectionStrings:Npgsql").Value));


            service.AddScoped<ISalaRepository, SalaRepository>();
            service.AddScoped<IParticipanteRepository, ParticipanteRepository>();
            service.AddScoped<ISalaConfiguracaoRepository, SalaConfiguracaoRepository>();
            service.AddScoped<ICartaRepository, CartaRepository>();
            service.AddScoped<IPerfilRepository, PerfilRepository>();

            return service;
        }
    }
}
