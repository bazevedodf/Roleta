using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Roleta.Dominio;

namespace Roleta.Persistencia.Mapeamentos
{
    public class TransacaoMap : IEntityTypeConfiguration<Transacao>
    {
        public void Configure(EntityTypeBuilder<Transacao> builder)
        {
            //Nome Tabela
            builder.ToTable("Transacoes");

            //Chave Primária
            builder.HasKey(x => x.Id);

            //Propriedades
            builder.Property(x => x.Tipo)
                .HasMaxLength(30)
                .IsRequired();
            builder.Property(x => x.valor);
            builder.Property(x => x.Data)
                   .HasDefaultValue(DateTime.Now)
                   .IsRequired();
            builder.Property(x => x.TransactionId)
                .HasMaxLength(30);

            //Relacionamento
            builder.HasOne(x => x.Carteira)
                   .WithMany(x => x.Transacoes);
        }
    }
}
