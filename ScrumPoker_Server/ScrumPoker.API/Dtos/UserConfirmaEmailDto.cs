using System.ComponentModel.DataAnnotations;

namespace ScrumPoker.API.Dtos
{
    public class UserConfirmaEmailDto
    {
        [Required]
        public string UserName { get; set; }


        [Required]
        public string Token { get; set; }
    }
}