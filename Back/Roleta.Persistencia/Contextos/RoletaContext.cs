using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Roleta.Dominio;
using Roleta.Dominio.Identity;
using Roleta.Persistencia.Mapeamentos;
using Roleta.Persistencia.Mapeamentos.Identity;

namespace Roleta.Persistencia
{
    public class RoletaContext : IdentityDbContext<User, Role, Guid, 
                                            IdentityUserClaim<Guid>, UserRole, IdentityUserLogin<Guid>, 
                                            IdentityRoleClaim<Guid>, IdentityUserToken<Guid>>
    {
        public RoletaContext(DbContextOptions<RoletaContext> options) : base(options)
        {

        }

        public DbSet<Produto> Produtos { get; set; }
        public DbSet<Pagamento> Pagamentos { get; set; }
        public DbSet<GiroRoleta> GirosRoleta { get; set; }
        public DbSet<Saque> Saques { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //Identity
            modelBuilder.ApplyConfiguration(new RoleMap());
            modelBuilder.ApplyConfiguration(new UserRoleMap());


            //Aplication
            modelBuilder.ApplyConfiguration(new ProdutoMap());
            modelBuilder.ApplyConfiguration(new PagamentoMap());
            modelBuilder.ApplyConfiguration(new SaqueMap());
        }
    }
}
