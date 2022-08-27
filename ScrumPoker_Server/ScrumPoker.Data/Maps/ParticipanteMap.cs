using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ScrumPoker.Domain.Entities.Salas.Participantes;

namespace ScrumPoker.Data.Maps
{
    class ParticipanteMap : IEntityTypeConfiguration<Participante>
    {
        public void Configure(EntityTypeBuilder<Participante> builder)
        {
            builder.ToTable(nameof(Participante));

            builder.HasKey(x => x.Id);

            builder
                .Property(x => x.Nome)
                .IsRequired();

            builder
                .Property(x => x.SalaId)
                .IsRequired();
        }
    }
}