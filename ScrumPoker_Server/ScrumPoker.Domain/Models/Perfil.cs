using System;
using ScrumPoker.Domain.Identity;

namespace ScrumPoker.Domain.Models
{
    public class Perfil
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }


        public User User { get; }
    }
}