using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ScrumPoker.API.Dtos
{
    public class SalaConfiguracaoDto
    {

        [Required]
        public long Id { get; set; }

        [Required]
        public string SalaId { get; set; }

        [Required]
        public bool JogadorFinalizaJogo { get; set; }

        [Required]
        public bool JogadorResetaJogo { get; set; }

        [Required]
        public bool JogadorRemoveJogador { get; set; }

        [Required]
        public bool JogadorRemoveAdministrador { get; set; }


        [Required]
        public IEnumerable<CartaDto> Cartas { get; set; }
    }
}