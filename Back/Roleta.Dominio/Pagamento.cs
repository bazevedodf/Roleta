using Roleta.Dominio.Identity;

namespace Roleta.Dominio
{
    public class Pagamento
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string CPF { get; set; }
        public string TransactionId { get; set; }
        public string QrCode { get; set; }
        public string QrCodeText { get; set; }
        public decimal Valor { get; set; }
        public string Status { get; set; }
        public DateTime DataStatus { get; set; }
        public DateTime DataCadastro { get; set; }
        public Guid UserId { get; set; }
        public User? User { get; set; }
        //public int ProdutoId { get; set; }
        //public Produto? Produto { get; set; }
    }
}
