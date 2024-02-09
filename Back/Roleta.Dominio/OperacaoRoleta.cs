using Roleta.Dominio.Identity;

namespace Roleta.Dominio
{
    public class OperacaoRoleta
    {
        public int Id { get; set; }
        public string Descricao { get; set; }
        public decimal Valor { get; set; }
        public DateTime Data { get; set; } = DateTime.Now;
        public string? TransactionId { get; set; }
        public Guid UserId { get; set; }
        public User? User { get; set; }
        public int RoletaSorteId { get; set; }
        public RoletaSorte? RoletaSorte { get; set; }
    }
}
