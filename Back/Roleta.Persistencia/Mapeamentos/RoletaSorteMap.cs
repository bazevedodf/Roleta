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
            builder.Property(x => x.ValorSaque)
                .HasDefaultValue(0);
            builder.Property(x => x.PercentualBanca)
                .HasDefaultValue(60);

            builder.HasData(
                new RoletaSorte()
                {
                    Id = 1,
                    Nome = "RoletaSorte",
                    SaldoBanca = 0,
                    PremiacaoMaxima = 10,
                    SaldoLucro = 0,
                    ValorSaque= 50,
                    PercentualBanca= 60,
                }
            );
        }
    }
}
