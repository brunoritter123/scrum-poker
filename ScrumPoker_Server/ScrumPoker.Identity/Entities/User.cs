using Microsoft.AspNetCore.Identity;

namespace ScrumPoker.Identity.Entities
{
    public class User : IdentityUser<Guid>
    {
        public IEnumerable<UserRole> UserRoles { get; set; }

        public User()
        {
            UserRoles = new List<UserRole>();
        }
    }
}
