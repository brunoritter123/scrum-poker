using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrumPoker.Application.DTOs.InputModels
{
    public class AlterarConfiguracaoSalaCartaInputModel
    {
        public int Id { get; set; }

        [Required]
        [Range(0, int.MaxValue)]
        public int Ordem { get; set; }

        [Required]
        public string Value { get; set; }

        [Required]
        public bool Especial { get; set; }

        public string SalaId { get; set; }
    }
}
