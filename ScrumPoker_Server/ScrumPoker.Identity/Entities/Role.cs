using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace ScrumPoker.Identity.Entities
{
    public class Role : IdentityRole<Guid>
    {
        public IEnumerable<UserRole> UserRoles { get; set; }
    }
}