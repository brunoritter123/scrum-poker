using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrumPoker.Application.DTOs.InputModels
{
    public class AlterarConfiguracaoSalaInputModel
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
        public IEnumerable<AlterarConfiguracaoSalaCartaInputModel> Cartas { get; set; }
    }
}
