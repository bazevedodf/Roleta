using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Roleta.Dominio;

namespace Roleta.Persistencia.Mapeamentos
{
    public class PagamentoMap : IEntityTypeConfiguration<Pagamento>
    {
        public void Configure(EntityTypeBuilder<Pagamento> builder)
        {
            //Nome Tabela
            builder.ToTable("Pagamentos");
            
            //Chave Primaria
            builder.HasKey(x => x.Id);

            //Propriedades
            builder.Property(x => x.Nome)
                .IsRequired()
                .HasMaxLength(80);
            builder.Property(x => x.CPF)
                .IsRequired()
                .HasMaxLength(11);
            builder.Property(x => x.TransactionId)
                .IsRequired()
                .HasMaxLength(110);
            builder.Property(x => x.QrCode);
            builder.Property(x => x.QrCodeText);
            builder.Property(x => x.Valor)
                .IsRequired();
            builder.Property(x => x.Status);
            builder.Property(x => x.DataStatus);
            builder.Property(x => x.DataCadastro)
                .IsRequired()
                .HasDefaultValue(DateTime.Now);

            //Relacionamento
            builder.HasOne(x => x.User)
                .WithMany(x => x.Pagamentos)
                .HasConstraintName("FK_User_Pagamento");

            //builder.HasOne(x => x.Produto)
            //    .WithMany()
            //    .HasConstraintName("FK_Produto_pagamento");

            //Index
            builder.HasIndex(x => x.TransactionId)
                .IsUnique();

        }
    }
}
