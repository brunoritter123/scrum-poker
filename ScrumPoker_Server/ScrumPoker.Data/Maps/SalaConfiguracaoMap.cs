using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ScrumPoker.Domain.Entities.SalaEntity;

namespace ScrumPoker.Data.Maps
{
    class SalaConfiguracaoMap : IEntityTypeConfiguration<SalaConfiguracao>
    {
        public void Configure(EntityTypeBuilder<SalaConfiguracao> builder)
        {
            builder.ToTable(nameof(SalaConfiguracao));

            builder.HasKey(x => x.Id);

            builder
                .Property(x => x.JogadorFinalizaJogo)
                .IsRequired();

            builder
                .Property(x => x.JogadorResetaJogo)
                .IsRequired();

            builder
                .Property(x => x.JogadorRemoveAdministrador)
                .IsRequired();

            builder
                .Property(x => x.JogadorRemoveJogador)
                .IsRequired();

            builder.HasMany(x => x.Cartas)
                .WithOne(x => x.SalaConfiguracao);
        }
    }
}