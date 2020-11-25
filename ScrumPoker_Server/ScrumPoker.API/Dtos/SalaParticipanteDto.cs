using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ScrumPoker.API.Dtos
{
    public class SalaParticipanteDto
    {

        [Required]
        public string Id { get; set; }

        [Required]
        public string SalaId { get; set; }

        [Required]
        public string Nome { get; set; }

        public bool Jogador { get; set; }
        public bool Online { get; set; }

        public string VotoCartaValor { get; set; }
    }
}