using Microsoft.AspNetCore.Identity;
using System;

namespace ScrumPoker.Domain.Identity
{
    public class UserRole : IdentityUserRole<Guid>
    {
        public User User { get; set;}
        public Role Role { get; set;}
    }
}