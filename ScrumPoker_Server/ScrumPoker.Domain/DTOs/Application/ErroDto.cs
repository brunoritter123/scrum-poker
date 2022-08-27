using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrumPoker.Domain.DTOs.Application
{
    public class ErroDto
    {
        public ErroDto(string codigo, string menssagem)
        {
            Codigo = codigo;
            Menssagem = menssagem;              
        }
        public string Codigo { get; set; }
        public string Menssagem { get; set; }
    }
}
