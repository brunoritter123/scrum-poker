using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ScrumPoker.Data.Context;
using ScrumPoker.Data.Repositories;
using ScrumPoker.Domain.Interfaces.Repositories;

namespace ScrumPoker.Data
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDependencyDbContext(this IServiceCollection service,
            IConfiguration configuration)
        {
            service.AddDbContext<ScrumPokerContext>(options =>
                options.UseInMemoryDatabase("ScrumPoker"));

            service.AddScoped<ISalaRepository, SalaRepository>();
            service.AddScoped<IParticipanteRepository, ParticipanteRepository>();
            service.AddScoped<ISalaConfiguracaoRepository, SalaConfiguracaoRepository>();
            service.AddScoped<ICartaRepository, CartaRepository>();
            service.AddScoped<IPerfilRepository, PerfilRepository>();

            return service;
        }
    }
}