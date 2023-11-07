using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Roleta.Dominio;

namespace Roleta.Persistencia.Mapeamentos
{
    public class GiroRoletaMap : IEntityTypeConfiguration<GiroRoleta>
    {
        public void Configure(EntityTypeBuilder<GiroRoleta> builder)
        {
            //Nome Tabela
            builder.ToTable("GirosRoleta");

            //Chave Primaria
            builder.HasKey(x => x.Id);

            //Propriedades
            builder.Property(x => x.ValorAposta);
            builder.Property(x => x.Multiplicador);

            //Relacionamento
            builder.HasOne(x => x.User)
                .WithMany()
                .HasConstraintName("FK_Giros_User");

            builder.HasIndex(x => x.UserId);
        }
    }
}
