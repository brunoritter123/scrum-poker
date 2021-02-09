using Microsoft.AspNetCore.Identity;
using ScrumPoker.Domain.Entities.UsuarioEntity;
using System;
using System.Collections.Generic;

namespace ScrumPoker.Domain.Identity
{
    public class User : IdentityUser<Guid>
    {
        public IEnumerable<UserRole> UserRoles { get; set; }


        public Guid PerfilId { get; set; }
        public Perfil Perfil { get; set; }
    }
}
