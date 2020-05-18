using ScrumPoker.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ScrumPoker.Data.Maps
{
    class SalaMap : IEntityTypeConfiguration<Sala>
    {
        public void Configure(EntityTypeBuilder<Sala> builder)
        {
            builder.ToTable(nameof(Sala));

            builder.HasKey(x => x.Id);

            builder
                .Property(x => x.JogadorFinalizaJogo)
                .IsRequired();

            builder
                .Property(x => x.JogadorRemoveJogador)
                .IsRequired();

            builder
                .Property(x => x.JogadorRemoveAdministrador)
                .IsRequired();

            builder
                .Property(x => x.JogadorRemoveJogador)
                .IsRequired();

            builder.HasMany(x => x.Cartas)
                .WithOne(x => x.Sala);
        }
    }
}