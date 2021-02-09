using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ScrumPoker.Domain.Entities.SalaEntity;

namespace ScrumPoker.Data.Maps
{
    class SalaMap : IEntityTypeConfiguration<Sala>
    {
        public void Configure(EntityTypeBuilder<Sala> builder)
        {
            builder.ToTable(nameof(Sala));

            builder.HasKey(x => x.Id);

            builder
                .Property(x => x.JogoFinalizado)
                .IsRequired();

            builder.HasOne(x => x.Configuracao)
                .WithOne(x => x.Sala);

            builder.HasMany(x => x.Participantes)
                .WithOne(x => x.Sala);
        }
    }
}