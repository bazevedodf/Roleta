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
        public string? ImagemUrl { get; set; }
        public bool Verified { get; set; }
        public string? AfiliateCode { get; set; }
        public DateTime DataCadastro { get; set; } = DateTime.Now;
        public string? Token { get; set; }
        public IEnumerable<PagamentoDto> Pagamentos { get; set; }
    }
}
