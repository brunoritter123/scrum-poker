using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ScrumPoker.Domain.Entities.Perfis;

namespace ScrumPoker.Data.Maps
{
    class PerfilMap : IEntityTypeConfiguration<Perfil>
    {
        public void Configure(EntityTypeBuilder<Perfil> builder)
        {
            builder.ToTable(nameof(Perfil));

            builder.HasKey(x => x.Login);

            builder
                .Property(x => x.Nome)
                .IsRequired();
        }
    }
}