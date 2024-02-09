using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Roleta.Dominio;

namespace Roleta.Persistencia.Mapeamentos
{
    public class BancaPagadoraMap : IEntityTypeConfiguration<BancaPagadora>
    {
        public void Configure(EntityTypeBuilder<BancaPagadora> builder)
        {
            //Nome tabela
            builder.ToTable("BancasPagadora");

            //Chave Primaria
            builder.HasKey(x => x.Id);

            //Propriedades
            builder.Property(x => x.SaldoDia);
            builder.Property(x => x.DataBanca);

            //Mapeamentos
            builder.HasOne(x => x.RoletaSorte)
                .WithMany(x => x.BancasPagadoras);
        }
    }
}
