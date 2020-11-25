using System;
using System.Collections.Generic;

namespace ScrumPoker.Domain.Models
{
    public class Sala
    {
        public string Id { get; set; }
        public bool JogoFinalizado { get; set; }

        public SalaConfiguracao Configuracao { get; set; }
        public IEnumerable<SalaParticipante> Participantes { get; set; }
    }
}