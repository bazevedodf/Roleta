using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Roleta.Dominio;

namespace Roleta.Persistencia.Mapeamentos
{
    public class SaqueMap : IEntityTypeConfiguration<Saque>
    {
        public void Configure(EntityTypeBuilder<Saque> builder)
        {
            //Nome Table
            builder.ToTable("Saques");

            //Chave Primaria
            builder.HasKey("Id");

            //Propriedades
            builder.Property(x => x.TransactionId)
                   .HasMaxLength(15);
            builder.Property(x => x.Valor);
            builder.Property(x => x.Status)
                   .HasDefaultValue("Pendente")
                   .HasMaxLength(20);
            builder.Property(x => x.DataStatus);
            builder.Property(x => x.DataCadastro)
                .IsRequired()
                .HasDefaultValueSql("NOW()");

            //Relacionamento
            builder.HasOne(x => x.User)
                .WithMany(x => x.Saques)
                .HasConstraintName("FK_User_Saque");

            //Index
            builder.HasIndex(x => x.TransactionId)
                .IsUnique();

        }
    }
}
