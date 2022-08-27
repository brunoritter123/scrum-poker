using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ScrumPoker.Data.Maps;
using ScrumPoker.Domain.Entities.Perfis;
using ScrumPoker.Domain.Entities.Salas;
using ScrumPoker.Domain.Entities.Salas.Cartas;
using ScrumPoker.Domain.Entities.Salas.Configuracoes;
using ScrumPoker.Identity.Entities;
using System;

namespace ScrumPoker.Data.Context
{
    public class ScrumPokerContext : IdentityDbContext<
        User, Role, Guid,
        IdentityUserClaim<Guid>, UserRole, IdentityUserLogin<Guid>,
        IdentityRoleClaim<Guid>, IdentityUserToken<Guid>>
    {
        public ScrumPokerContext(DbContextOptions<ScrumPokerContext> options) : base(options)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        }

        public DbSet<Sala> Sala { get; set; }
        public DbSet<SalaConfiguracao> SalaConfiguracao { get; set; }
        public DbSet<Carta> Carta { get; set; }
        public DbSet<Perfil> Perfil { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new SalaMap());
            modelBuilder.ApplyConfiguration(new SalaConfiguracaoMap());
            modelBuilder.ApplyConfiguration(new ParticipanteMap());
            modelBuilder.ApplyConfiguration(new CartaMap());
            modelBuilder.ApplyConfiguration(new UserRoleMap());
            modelBuilder.ApplyConfiguration(new PerfilMap());
        }
    }
}
