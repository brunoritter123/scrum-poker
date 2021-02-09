using System;
using System.Collections.Generic;
using System.Text;

namespace ScrumPoker.Application.DTOs.ViewModels
{
    public class ParticipanteViewModel
    {
        public string Id { get; set; }
        public string SalaId { get; set; }
        public string Nome { get; set; }
        public bool Jogador { get; set; }
        public bool Online { get; set; }
        public string VotoCartaValor { get; set; }
    }
}
