using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using ScrumPoker.API.Services;
using ScrumPoker.Application.Interfaces.ApplicationServices;
using ScrumPoker.Application.Services;
using ScrumPoker.CrossCutting.Services;
using ScrumPoker.Domain.Interfaces.Application;
using ScrumPoker.Identity.Interfaces;
using ScrumPoker.Identity.Services;
using System;

namespace ScrumPoker.API.Configurations
{
    public static class InjecaoDependenciaConfig
    {
        public static void AdicionarInjecaoDependenciaConfig(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            services.AddScoped<ISalaService, SalaService>();
            services.AddScoped<ISalaConfiguracaoService, SalaConfiguracaoService>();
            services.AddScoped<IParticipanteService, ParticipanteService>();
            services.AddScoped<IPerfilService, PerfilService>();
            services.AddScoped<IUsuarioService, UsuarioService>();

            services.AddScoped<IIdentityService, IdentityService>();

            services.AddSingleton<IUserIdProvider, HubUserIdService>();
            services.AddTransient<IEmailSender, EmailSender>();

        }
    }
}