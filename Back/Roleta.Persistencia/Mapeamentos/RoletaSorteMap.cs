using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Roleta.Dominio;

namespace Roleta.Persistencia.Mapeamentos
{
    public class RoletaSorteMap : IEntityTypeConfiguration<RoletaSorte>
    {
        public void Configure(EntityTypeBuilder<RoletaSorte> builder)
        {
            //Chave Primaria
            builder.ToTable("Roletas");

            //Propriedades
            builder.Property(x => x.Nome)
                .HasMaxLength(30)
                .IsRequired();

            builder.Property(x => x.SaldoBanca)
                .HasDefaultValue(0);
            builder.Property(x => x.PremiacaoMaxima)
                .HasDefaultValue(0);
            builder.Property(x => x.SaldoLucro)
                .HasDefaultValue(0);
            builder.Property(x => x.ValorMinimoSaque)
                .HasDefaultValue(0);
            builder.Property(x => x.ValorMaximoSaque)
                .HasDefaultValue(0);
            builder.Property(x => x.PercentualBanca)
                .HasDefaultValue(60);
            builder.Property(x => x.TaxaPerda)
                .HasDefaultValue(10);
            builder.Property(x => x.ContagemPerda)
                .HasDefaultValue(0);
            builder.Property(x => x.TaxaSaque)
                .HasDefaultValue(0);

            builder.HasData(
                new RoletaSorte()
                {
                    Id = 1,
                    Nome = "RoletaSorte",
                    SaldoBanca = 0,
                    PremiacaoMaxima = 10,
                    SaldoLucro = 0,
                    ValorMinimoSaque = 50,
                    ValorMaximoSaque = 500,
                    PercentualBanca = 60,
                    TaxaSaque = 5
                }
            );
        }
    }
}
