namespace Roleta.Aplicacao.Dtos
{
    public class CarteiraDto
    {
        public int Id { get; set; }
        public decimal SaldoAtual { get; set; }
        public decimal SaldoDemo { get; set; }
        public Guid UserId { get; set; }
        public DateTime DataCadastro { get; set; } = DateTime.Now;
        public DateTime DataAtualizacao { get; set; } = DateTime.Now;
        public IEnumerable<TransacaoDto>? Transacoes { get; set; }
    }
}
