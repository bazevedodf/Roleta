using Roleta.Dominio.Identity;

namespace Roleta.Dominio
{
    public class Carteira
    {
        public int Id { get; set; }
        public decimal SaldoAtual { get; set; }
        public decimal SaldoDemo { get; set; }
        public Guid UserId { get; set; }
        public User? User { get; set; }
        public DateTime DataCadastro { get; set; } = DateTime.Now;
        public DateTime DataAtualizacao { get; set; }
        public IEnumerable<Transacao>? Transacoes { get; set; }
    }
}
