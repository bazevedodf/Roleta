namespace Roleta.Aplicacao.Dtos
{
    public class AfiliadoDto
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? UserName { get; set; }
        public string Email { get; set; }
        public bool isAfiliate { get; set; } = false;
        public int Comissao { get; set; }
        public bool isBlocked { get; set; } = false;
        public bool DemoAcount { get; set; } = false;
        public CarteiraDto Carteira { get; set; }
        public decimal TotalFaturamento { get; set; }
        public int TotalDepositos { get; set; }
    }
}
