using Roleta.Dominio.Enums;

namespace Roleta.Dominio
{
    public class Produto
    {
        public int Id { get; set; }
        public TipoProduto TipoProduto { get; set; }
        public string Nome { get; set; }
        public string? Descricao { get; set; }
        public decimal SaldoDeposito { get; set; }
        public decimal Valor { get; set; }
        public int Giros { get; set; }
        public DateTime DataCadastro { get; set; }
        public bool Ativo { get; set; }
    }
}
