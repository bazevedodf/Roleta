using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Roleta.Dominio.Identity;

namespace Roleta.Persistencia.Mapeamentos.Identity
{
    public class RoleMap : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.HasData(
                new Role()
                {
                    Id = Guid.NewGuid(),
                    Name = "Admin",
                    NormalizedName = "ADMIN"
                },
                new Role()
                {
                    Id = Guid.NewGuid(),
                    Name = "Player",
                    NormalizedName = "PLAYER"
                },
                new Role()
                {
                    Id = Guid.NewGuid(),
                    Name = "Afiliate",
                    NormalizedName = "AFILIATE"
                }
            );
        }
    }
}
