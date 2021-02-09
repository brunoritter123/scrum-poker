using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using ScrumPoker.API.Services;
using ScrumPoker.Application.Configurations;
using ScrumPoker.CrossCutting.Services;
using ScrumPoker.Domain.Interfaces.Repositories;
using System;

namespace ScrumPoker.API.Configurations
{
    public static class InjecaoDependenciaConfig
    {
        public static void AdicionarInjecaoDependenciaConfig(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            services.AddSingleton<IUserIdProvider, HubUserIdService>();
            services.AddTransient<IEmailSender, EmailSender>();

        }
    }
}