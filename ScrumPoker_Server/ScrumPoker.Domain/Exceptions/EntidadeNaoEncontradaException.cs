using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrumPoker.Core.Exceptions
{
    public class EntidadeNaoEncontradaException : ApplicationException
    {
        public string Entidade { get; set; }
        public string Chave { get; set; }

        public EntidadeNaoEncontradaException(string entidade, string chave)
            : this(entidade, chave, $"Não foi encontrado uma '{entidade}' com a chave '{chave}'") { }

        public EntidadeNaoEncontradaException(string entidade, string chave, string mensagem)
            : base(mensagem)
        {
            Entidade = entidade;
            Chave = chave;
        }
    }
}
