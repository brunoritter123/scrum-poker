using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ScrumPoker.Domain.Identity;
using ScrumPoker.Data.Maps;
using ScrumPoker.Domain.Models;
using System;

namespace ScrumPoker.Data.Context
{
    public class ScrumPokerContext : IdentityDbContext<
        User, Role, Guid,
        IdentityUserClaim<Guid>, UserRole, IdentityUserLogin<Guid>,
        IdentityRoleClaim<Guid>, IdentityUserToken<Guid>>
    {
        public ScrumPokerContext(DbContextOptions<ScrumPokerContext> options) : base(options) { }

        public DbSet<Sala> Sala { get; set; }
        public DbSet<Carta> Carta { get; set; }
        public DbSet<Perfil> Perfil { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new SalaMap());
            modelBuilder.ApplyConfiguration(new CartaMap());
            modelBuilder.ApplyConfiguration(new UserRoleMap());
            modelBuilder.ApplyConfiguration(new PerfilMap());
        }
    }
}
