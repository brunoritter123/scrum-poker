using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ScrumPoker.Domain.Entities.UsuarioEntity;
using ScrumPoker.Domain.Identity;

namespace ScrumPoker.Data.Maps
{
    class PerfilMap : IEntityTypeConfiguration<Perfil>
    {
        public void Configure(EntityTypeBuilder<Perfil> builder)
        {
            builder.ToTable(nameof(Perfil));

            builder.HasKey(x => x.Id);

            builder
                .Property(x => x.Nome)
                .IsRequired();

            builder.HasOne(x => x.User)
                .WithOne(x => x.Perfil)
                .HasForeignKey<User>(u => u.PerfilId);
        }
    }
}