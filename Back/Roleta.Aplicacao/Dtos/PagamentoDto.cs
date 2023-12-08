using Roleta.Aplicacao.Dtos.Identity;

namespace Roleta.Aplicacao.Dtos
{
    public class PagamentoDto
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string CPF { get; set; }
        public string TransactionId { get; set; }
        public string QrCode { get; set; }
        public string QrCodeText { get; set; }
        public decimal Valor { get; set; }
        public string? Status { get; set; }
        public DateTime? DataStatus { get; set; }
        public DateTime? DataCadastro { get; set; }
        public Guid UserId { get; set; }
        public UserDto? User { get; set; }
    }
}
