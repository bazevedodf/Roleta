using Roleta.Dominio;

namespace Roleta.Aplicacao.Dtos
{
    public class UserGameDto
    {
        public Guid Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? UserName { get; set; }
        public string Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? CPF { get; set; }
        public string? TipoChavePix { get; set; }
        public string? ChavePix { get; set; }
        public bool isAfiliate { get; set; } = false;
        public int Comissao { get; set; }
        public bool isBlocked { get; set; } = false;
        public bool DemoAcount { get; set; } = false;
        public string? ParentEmail { get; set; }
        public DateTime DataCadastro { get; set; } = DateTime.Now;
        public bool Verified { get; set; }
        public CarteiraDto Carteira { get; set; }
        public IEnumerable<Pagamento>? Pagamentos { get; set; }
        public IEnumerable<Saque>? Saques{ get; set; }
    }
}
