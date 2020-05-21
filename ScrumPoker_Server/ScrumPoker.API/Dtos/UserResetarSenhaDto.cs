using System.ComponentModel.DataAnnotations;

namespace ScrumPoker.API.Dtos
{
    public class UserResetarSenhaDto
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        [StringLength(10,MinimumLength=4)]
        public string Password { get; set; }

        [Required]
        public string Token { get; set; }
    }
}