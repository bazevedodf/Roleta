using Microsoft.AspNetCore.Identity;

namespace Roleta.Dominio.Identity
{
    public class User : IdentityUser<Guid>
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string Email { get; set; }
        public string? Description { get; set; }
        public string? ImagemUrl { get; set; }
        public int FreeSpin { get; set; }
        public decimal SaldoDeposito { get; set; }
        public decimal SaldoSaque { get; set; }
        public string? ParentEmail { get; set; }
        public bool Verified { get; set; }
        public IEnumerable<UserRole>? UserRoles { get; set; }
        public IEnumerable<Pagamento>? Pagamentos { get; set; }
        public IEnumerable<Saque>? Saques { get; set; }
        public IEnumerable<GiroRoleta>? GirosRoleta { get; set; }
    }
}
