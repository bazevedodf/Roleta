using Microsoft.AspNetCore.Identity;

namespace Roleta.Dominio.Identity
{
    public class Role : IdentityRole<Guid>
    {
        public IEnumerable<UserRole> UserRoles { get; set; }
    }
}
