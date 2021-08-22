using System;
using System.Collections.Generic;
using System.Text;

namespace ScrumPoker.Application.DTOs.ViewModels
{
    public class CartaViewModel
    {
        public int Ordem { get; set; }
        public string Value { get; set; }
        public bool Especial { get; set; }
    }
}
