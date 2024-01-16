namespace Roleta.Dominio
{
    public class RoletaSorte
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public decimal SaldoBanca { get; set; }
        public decimal PremiacaoMaxima { get; set; }
        public decimal SaldoLucro { get; set; }
        public int ValorMinimoSaque { get; set; }
        public int ValorMaximoSaque { get; set; }
        public int PercentualBanca { get; set; }
        public int TaxaPerda { get; set; }
        public int ContagemPerda { get; set; }
        public int TaxaSaque { get; set; }
        public IEnumerable<TransacaoRoleta>? TransacoesRoleta { get; set; }
    }
}
