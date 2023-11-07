using Roleta.Dominio;

namespace Roleta.Aplicacao.Dtos
{
    public class UserGameDto
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? UserName { get; set; }
        public string Email { get; set; }
        public int FreeSpin { get; set; }
        public decimal SaldoDeposito { get; set; }
        public decimal SaldoSaque { get; set; }
        public bool Verified { get; set; }
        public IEnumerable<Pagamento>? Pagamentos { get; set; }
        public IEnumerable<Saque>? Saques{ get; set; }
        public IEnumerable<GiroRoleta>? GirosRoleta { get; set; }
    }
}
