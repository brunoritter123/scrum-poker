using System;
using System.Collections.Generic;
using System.Text;

namespace ScrumPoker.Application.DTOs.ViewModels
{
    public class SalaConfiguracaoViewModel
    {
        public long Id { get; set; }
        public string SalaId { get; set; }

        public bool JogadorFinalizaJogo { get; set; }
        public bool JogadorResetaJogo { get; set; }
        public bool JogadorRemoveJogador { get; set; }
        public bool JogadorRemoveAdministrador { get; set; }
        public IEnumerable<CartaViewModel> Cartas { get; set; }
    }
}
