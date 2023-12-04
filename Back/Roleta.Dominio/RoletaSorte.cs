namespace Roleta.Dominio
{
    public class RoletaSorte
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public decimal SaldoBanca { get; set; }
        public int PremiacaoMaxima { get; set; }
        public decimal SaldoLucro { get; set; }
        public int ValorSaque { get; set; }
        public int PercentualBanca { get; set; }
        public IEnumerable<TransacaoRoleta>? TransacoesRoleta { get; set; }
    }
}
