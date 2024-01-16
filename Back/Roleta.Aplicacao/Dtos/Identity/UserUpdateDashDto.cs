namespace Roleta.Aplicacao.Dtos.Identity
{
    public class UserUpdateDashDto
    {
        public Guid Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? CPF { get; set; }
        public string? TipoChavePix { get; set; }
        public string? ChavePix { get; set; }
        public int Comissao { get; set; } = 25;
        public bool Verified { get; set; }
        public bool isAfiliate { get; set; } = false;
        public string? AfiliateCode { get; set; }
        public bool isBlocked { get; set; } = false;
        public bool DemoAcount { get; set; } = false;
        public DateTime DataCadastro { get; set; } = DateTime.Now;
        public CarteiraDto? Carteira { get; set; }
    }
}
