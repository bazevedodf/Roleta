namespace Roleta.Dominio
{
    public class Transacao
    {
        public int Id { get; set; }
        public string Tipo { get; set; }
        public decimal valor { get; set; }
        public DateTime Data { get; set; } = DateTime.Now;
        public string? TransactionId { get; set; }
        public int CarteiraId { get; set; }
        public Carteira? Carteira { get; set; }
    }
}
