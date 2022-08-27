using Microsoft.AspNetCore.Identity;
using System;

namespace ScrumPoker.Identity.Entities
{
    public class UserRole : IdentityUserRole<Guid>
    {
        public User User { get; set;}
        public Role Role { get; set;}
    }
}