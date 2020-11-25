using System;
using Microsoft.AspNetCore.Http;

namespace ScrumPoker.API.Dtos
{
    public class PerfilDto
    {
        public Guid Id { get; set; }
        //public IFormFile Image { get; set; }
        public string Nome { get; set; }
        public string Email {get; set;}
    }
}