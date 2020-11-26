using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ScrumPoker.API.Dtos
{
    public class SalaDto
    {

        [Required]
        public string Id { get; set; }

        public string titulo { get; set; }

        [Required]
        public bool JogoFinalizado { get; set; }

        [Required]
        public SalaConfiguracaoDto Configuracao { get; set; }
        public IEnumerable<SalaParticipanteDto> Administradores { get; set; }
        public IEnumerable<SalaParticipanteDto> Jogadores { get; set; }
    }
}