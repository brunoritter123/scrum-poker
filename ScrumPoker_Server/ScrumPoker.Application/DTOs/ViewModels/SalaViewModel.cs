using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ScrumPoker.Application.DTOs.ViewModels
{
    public class SalaViewModel
    {
        public string Id { get; set; }
        public string Titulo { get; set; }
        public bool JogoFinalizado { get; set; }
        public SalaConfiguracaoViewModel Configuracao { get; set; }
        public IEnumerable<ParticipanteViewModel> Administradores { get; set; }
        public IEnumerable<ParticipanteViewModel> Jogadores { get; set; }
    }
}
