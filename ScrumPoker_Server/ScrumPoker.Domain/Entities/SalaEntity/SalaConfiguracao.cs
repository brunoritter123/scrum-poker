using System.Collections.Generic;

namespace ScrumPoker.Domain.Entities.SalaEntity
{
    public class SalaConfiguracao
    {
        public long Id { get; set; }
        public string SalaId { get; set; }
        public bool JogadorFinalizaJogo { get; set; }
        public bool JogadorResetaJogo { get; set; }
        public bool JogadorRemoveJogador { get; set; }
        public bool JogadorRemoveAdministrador { get; set; }


        public Sala Sala { get; }
        public IEnumerable<Carta> Cartas { get; set; }
    }
}