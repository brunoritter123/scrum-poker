using System.ComponentModel.DataAnnotations;

namespace ScrumPoker.Application.DTOs.InputModels
{
    public class ConectarSalaInputModel
    {
        [Required]
        public string IdSala { get; set; }

        [Required]
        public string IdParticipante { get; set; }

        [Required]
        public string SalaId { get; set; }

        [Required]
        public string Nome { get; set; }

        [Required]
        public bool Jogador { get; set; }
    }
}
