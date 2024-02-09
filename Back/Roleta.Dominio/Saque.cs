using Roleta.Dominio.Identity;

namespace Roleta.Dominio
{
    public class Saque
    {
        public int Id { get; set; }
        public string? TransactionId { get; set; }
        public decimal Valor { get; set; }
        public string Status { get; set; }
        public string? Description { get; set; }
        public string? TextoInformativo { get; set; }
        public DateTime? DataStatus { get; set; }
        public DateTime? DataCadastro { get; set; } = DateTime.Now;
        public Guid UserId { get; set; }
        public User? User { get; set; }
    }
}
