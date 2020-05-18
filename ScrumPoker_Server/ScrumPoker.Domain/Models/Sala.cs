using System;
using System.Collections.Generic;

namespace ScrumPoker.Domain.Models
{
    public class Sala
    {
        public string Id { get; set; }
        public bool JogadorFinalizaJogo { get; set; }
        public bool JogadorResetaJogo { get; set; }
        public bool JogadorRemoveJogador { get; set; }
        public bool JogadorRemoveAdministrador { get; set; }


        public IEnumerable<Carta> Cartas { get; set; }
    }
}