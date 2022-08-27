using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrumPoker.Domain.DTOs.Application
{
    public class ResultadoDto
    {
        public ResultadoDto()
        {
            Sucesso = true;
        }

        public ResultadoDto(ErroDto erro)
        {
            Sucesso = false;
            Erros = new List<ErroDto>() 
            {
                erro
            };
        }

        public ResultadoDto(IEnumerable<ErroDto> erros)
        {
            Sucesso = false;
            Erros = erros;
        }

        public bool Sucesso { get; private set; }
        public IEnumerable<ErroDto> Erros { get; private set; }
    }

    public class ResultadoDto<T> : ResultadoDto
    {
        public ResultadoDto(T resultado) : base()
        {
            Resultado = resultado;
        }

        public ResultadoDto(IEnumerable<ErroDto> erros) : base(erros) { }
        public ResultadoDto(ErroDto erro) : base(erro) { }

        public T Resultado { get; private set; }
    }
}
