using ScrumPoker.Domain.Entities.Salas.Configuracoes;
using ScrumPoker.Domain.Entities.Salas.Participantes;
using System;
using System.Collections.Generic;

namespace ScrumPoker.Domain.Entities.Salas
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