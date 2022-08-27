using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrumPoker.Domain.Entities.Usuarios.Requests
{
    public class ConfirmacaoEmailRequest
    {

        [Required]
        [Url]
        public string UrlConfirmaEmail { get; set; }
    }
}
