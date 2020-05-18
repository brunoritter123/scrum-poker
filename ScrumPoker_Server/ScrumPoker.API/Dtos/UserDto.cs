using System.ComponentModel.DataAnnotations;

namespace ScrumPoker.API.Dtos
{
    public class UserDto
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(10,MinimumLength=4)]
        public string Password { get; set; }
    }
}