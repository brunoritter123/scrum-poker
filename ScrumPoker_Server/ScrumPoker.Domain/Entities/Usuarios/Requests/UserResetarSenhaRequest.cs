using System.ComponentModel.DataAnnotations;

namespace ScrumPoker.Domain.Entities.Usuarios.Requests
{
    public class UserResetarSenhaRequest
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        [StringLength(10, MinimumLength = 4)]
        public string Password { get; set; }

        [Required]
        public string Token { get; set; }
    }
}
