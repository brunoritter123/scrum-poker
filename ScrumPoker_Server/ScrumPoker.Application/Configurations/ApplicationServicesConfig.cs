using AutoMapper.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ScrumPoker.Application.Services;
using ScrumPoker.Application.Interfaces.ApplicationServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics.CodeAnalysis;

namespace ScrumPoker.Application.Configurations
{
    [ExcludeFromCodeCoverage]
    public static class ApplicationServicesConfig
    {
        public static IServiceCollection AddApplicationServicesConfig(this IServiceCollection services)
        {
            services.AddScoped<ISalaService, SalaService>();
            services.AddScoped<ISalaConfiguracaoService, SalaConfiguracaoService>();
            services.AddScoped<IParticipanteService, ParticipanteService>();
            services.AddScoped<IPerfilService, PerfilService>();
            return services;
        }
    }
}
