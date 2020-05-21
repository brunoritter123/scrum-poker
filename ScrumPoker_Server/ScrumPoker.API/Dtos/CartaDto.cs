using System.ComponentModel.DataAnnotations;

namespace ScrumPoker.API.Dtos
{
    public class CartaDto
    {
        public int Id { get; set; }

        [Required]
        [Range( 0, int.MaxValue)]
        public int Ordem { get; set; }

        [Required]
        public string Value { get; set; }

        [Required]
        public bool Especial { get; set;}

        public string SalaId { get; set; }
    }
}