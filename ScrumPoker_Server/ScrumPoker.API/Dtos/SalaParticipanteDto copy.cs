using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ScrumPoker.API.Dtos
{
    public class PostParticipanteDto
    {

        [Required]
        public string Id { get; set; }

        [Required]
        public string SalaId { get; set; }

        [Required]
        public string Nome { get; set; }

        [Required]
        public bool Jogador { get; set; }

        public bool Online { get; set; } = true;

    }
}