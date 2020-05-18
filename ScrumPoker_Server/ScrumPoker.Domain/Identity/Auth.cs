using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace ScrumPoker.Domain.Identity
{
    public class User : IdentityUser<Guid>
    {
        public IEnumerable<UserRole> UserRoles { get; set; }
    }
}
