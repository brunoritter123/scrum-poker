using System.ComponentModel.DataAnnotations;

namespace ScrumPoker.Domain.Entities.Usuarios.Requests
{
    public class UserConfirmaEmailRequest
    {
        [Required]
        public string UserName { get; set; }


        [Required]
        public string Token { get; set; }
    }
}