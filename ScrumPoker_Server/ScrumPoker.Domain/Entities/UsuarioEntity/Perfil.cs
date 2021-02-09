using ScrumPoker.Domain.Identity;
using System;

namespace ScrumPoker.Domain.Entities.UsuarioEntity
{
    public class Perfil
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }


        public User User { get; }
    }
}