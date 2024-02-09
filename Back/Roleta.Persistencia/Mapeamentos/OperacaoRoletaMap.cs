using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Roleta.Dominio;

namespace Roleta.Persistencia.Mapeamentos
{
    public class OperacaoRoletaMap : IEntityTypeConfiguration<OperacaoRoleta>
    {
        public void Configure(EntityTypeBuilder<OperacaoRoleta> builder)
        {
            //Nome Tabela
            builder.ToTable("Operacoes");

            //Chave Primária
            builder.HasKey(x => x.Id);

            //Propriedades
            builder.Property(x => x.Descricao)
                .HasMaxLength(30)
                .IsRequired();
            builder.Property(x => x.Valor)
                .IsRequired();                
            builder.Property(x => x.Data)
                   .IsRequired();
            builder.Property(x => x.TransactionId)
                .HasMaxLength(25);

            //Relacionamento
            builder.HasOne(x => x.User)
                   .WithMany(x => x.OperacoesRoleta);

            builder.HasOne(x => x.RoletaSorte)
                   .WithMany(x => x.OperacoesRoleta);
        }
    }
}
