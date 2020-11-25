using System.Xml.Linq;
using System.Security.Cryptography.X509Certificates;
using ScrumPoker.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ScrumPoker.Data.Maps
{
    class SalaParticipanteMap : IEntityTypeConfiguration<SalaParticipante>
    {
        public void Configure(EntityTypeBuilder<SalaParticipante> builder)
        {
            builder.ToTable(nameof(SalaParticipante));

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