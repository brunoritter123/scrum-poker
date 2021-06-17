using ScrumPoker.Application.DTOs.ViewModels;
using System.ComponentModel.DataAnnotations;

namespace ScrumPoker.Application.DTOs.InputModels
{
    public class GerarSalaPadraoInputModel
    {
        [Required]
        [Display(Name = "Id da Sala")]
        public string Id { get; set; }

        [Display(Name = "Configuração Sala Armazenada no Local Storage (usada caso a sala foi deletada do banco de dados)")]
        public SalaConfiguracaoViewModel SalaConfiguracao { get; set; }
    }
}
