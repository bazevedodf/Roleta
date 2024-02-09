using Roleta.Dominio;
using Roleta.Dominio.Identity;

namespace Roleta.Aplicacao.Dtos.Identity
{
    public class UserDashBoardDto
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
        public int Comissao { get; set; }
        public decimal ValorComissao { get; set; }
        public string? ImagemUrl { get; set; }
        public bool Verified { get; set; }
        public bool isAfiliate { get; set; } = false;
        public string? AfiliateCode { get; set; }
        public bool isBlocked { get; set; } = false;
        public bool DemoAcount { get; set; } = false;
        public DateTime DataCadastro { get; set; } = DateTime.Now;
        public string? Token { get; set; }
        public CarteiraDto? Carteira { get; set; }
        public IEnumerable<Role> Roles { get; set; }
        public IEnumerable<PagamentoDto>? Pagamentos { get; set; }
        public IEnumerable<OperacaoRoleta>? OperacoesRoleta { get; set; }
    }
}
