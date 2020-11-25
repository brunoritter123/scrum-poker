using ScrumPoker.Data.Repositories;
using ScrumPoker.Domain.Interfaces.Repositories;
using ScrumPoker.API.Services;
using ScrumPoker.API.Interfaces;
using ScrumPoker.Domain.Models;
using Microsoft.Extensions.DependencyInjection;
using System;
using Microsoft.AspNetCore.Identity.UI.Services;
using ScrumPoker.CrossCutting.Services;
using Microsoft.AspNetCore.SignalR;
using ScrumPoker.API.Dtos;

namespace ScrumPoker.API.Configurations
{
    public static class InjecaoDependenciaConfig
    {
        public static void AdicionarInjecaoDependenciaConfig(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            services.AddScoped<ISalaRepository, SalaRepository>();
            services.AddScoped<ISalaParticipanteRepository, SalaParticipanteRepository>();
            services.AddScoped<ISalaConfiguracaoRepository, SalaConfiguracaoRepository>();
            services.AddScoped<ICartaRepository, CartaRepository>();
            services.AddScoped<IPerfilRepository, PerfilRepository>();

            services.AddScoped<ISalaService, SalaService>();
            services.AddScoped<ISalaConfiguracaoService, SalaConfiguracaoService>();
            services.AddScoped<ISalaParticipanteService, SalaParticipanteService>();

            services.AddSingleton<IUserIdProvider, HubUserIdService>();

            services.AddTransient<IEmailSender, EmailSender>();
        }
    }
}