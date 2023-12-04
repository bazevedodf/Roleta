using Roleta.Dominio;

namespace Roleta.Aplicacao.Dtos
{
    public class TransacaoDto
    {
        public int Id { get; set; }
        public string Tipo { get; set; }
        public decimal valor { get; set; }
        public DateTime Data { get; set; }
        public string? TransactionId { get; set; }
        public int CarteiraId { get; set; }
        public Carteira? Carteira { get; set; }
    }
}
