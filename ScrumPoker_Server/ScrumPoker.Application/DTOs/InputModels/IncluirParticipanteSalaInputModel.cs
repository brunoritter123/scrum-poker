using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrumPoker.Application.DTOs.InputModels
{
    public class IncluirParticipanteSalaInputModel
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
