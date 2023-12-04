using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Roleta.Dominio;

namespace Roleta.Persistencia.Mapeamentos
{
    public class CarteiraMap : IEntityTypeConfiguration<Carteira>
    {
        public void Configure(EntityTypeBuilder<Carteira> builder)
        {
            //Nome Tabela
            builder.ToTable("Carteiras");

            //Chave Primaria
            builder.HasKey(x => x.Id);

            //Propriedades
            builder.Property(x => x.SaldoAtual);
            builder.Property(x => x.DataAtualizacao)
                   .HasDefaultValueSql("NOW()")
                   .IsRequired();

            //Relacionamento
            builder.HasOne(x => x.User)
                .WithOne(x => x.Carteira);
        }
    }
}
