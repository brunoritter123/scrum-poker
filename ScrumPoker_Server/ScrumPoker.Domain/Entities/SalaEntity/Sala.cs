using System;
using System.Collections.Generic;

namespace ScrumPoker.Domain.Entities.SalaEntity
{
    public class Sala
    {
        public string Id { get; set; }
        public string Titulo { get; set; }
        public bool JogoFinalizado { get; set; }
        public DateTime UltimaDataDeUtilizacao { get; set; } = DateTime.Now;

        public SalaConfiguracao Configuracao { get; set; }
        public IEnumerable<Participante> Participantes { get; set; }
    }
}