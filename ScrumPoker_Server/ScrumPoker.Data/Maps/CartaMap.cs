using ScrumPoker.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ScrumPoker.Data.Maps
{
    class CartaMap : IEntityTypeConfiguration<Carta>
    {
        public void Configure(EntityTypeBuilder<Carta> builder)
        {
            builder.ToTable(nameof(Carta));

            builder.HasKey(x => x.Id);

            builder
                .Property(x => x.Value)
                .IsRequired();

            builder
                .Property(x => x.Ordem)
                .IsRequired();

            builder
                .Property(x => x.SalaConfiguracaoId)
                .IsRequired();

            builder
                .Property(x => x.Especial)
                .IsRequired();
        }
    }
}