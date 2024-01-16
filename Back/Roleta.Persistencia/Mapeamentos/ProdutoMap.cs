using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Roleta.Dominio;

namespace Roleta.Persistencia.Mapeamentos
{
    public class ProdutoMap : IEntityTypeConfiguration<_Produto>
    {
        public void Configure(EntityTypeBuilder<_Produto> builder)
        {
            //Nome Tabela
            builder.ToTable("Produtos");

            //Chave Primaria
            builder.HasKey(x => x.Id);

            //Propriedades
            builder.Property(x => x.TipoProduto);

            builder.Property(x => x.Nome)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(x => x.Descricao)
                .HasMaxLength(200);

            builder.Property(x => x.SaldoDeposito)
                .HasDefaultValue(0);

            builder.Property(x => x.Valor)
                .HasDefaultValue(0);

            builder.Property(x => x.Giros)
                .HasDefaultValue(0);

            builder.Property(x => x.DataCadastro)
                .IsRequired()
                .HasDefaultValueSql("NOW()");
                //.HasDefaultValueSql("GETDATE()");

            builder.Property(x => x.Ativo)
                .HasDefaultValue(false);

            builder.HasData(
                new _Produto()
                {
                    Id= 1,
                    Nome = "R$ 75,00 (25,00 Bônus)",
                    SaldoDeposito = 75M,
                    Valor = 50.00M,
                    Ativo = true
                },
                new _Produto()
                {
                    Id = 2,
                    Nome = "R$ 200,00 (100,00 Bônus)",
                    SaldoDeposito = 200M,
                    Valor = 100.00M,
                    Ativo = true
                },
                new _Produto()
                {
                    Id = 3,
                    Nome = "R$ 500,00 (300,00 Bônus)",
                    SaldoDeposito = 500M,
                    Valor = 200.00M,
                    Ativo = true
                }
            );
        }
    }
}
