using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Roleta.Dominio;

namespace Roleta.Persistencia.Mapeamentos
{
    public class TransacaoRoletaMap : IEntityTypeConfiguration<TransacaoRoleta>
    {
        public void Configure(EntityTypeBuilder<TransacaoRoleta> builder)
        {
            //Nome Tabela
            builder.ToTable("TransacoesRoleta");

            //Chave Primária
            builder.HasKey(x => x.Id);

            //Propriedades
            builder.Property(x => x.valor)
                .IsRequired();
            builder.Property(x => x.Descricao)
                .HasMaxLength(30)
                .IsRequired();
            builder.Property(x => x.Data)
                   .HasDefaultValueSql("NOW()")
                   .IsRequired();
            builder.Property(x => x.TransacaoId)
                .HasMaxLength(15);

            //Relacionamento
            builder.HasOne(x => x.RoletaSorte)
                   .WithMany(x => x.TransacoesRoleta);
        }
    }
}
