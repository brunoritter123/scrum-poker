using System.ComponentModel.DataAnnotations;

namespace ScrumPoker.Domain.Entities.Usuarios.Requests
{
    public class UserLoginRequest
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
