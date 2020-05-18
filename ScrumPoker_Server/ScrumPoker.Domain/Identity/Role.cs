using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace ScrumPoker.Domain.Identity
{
    public class Role : IdentityRole<Guid>
    {
        public IEnumerable<UserRole> UserRoles { get; set; }
    }
}