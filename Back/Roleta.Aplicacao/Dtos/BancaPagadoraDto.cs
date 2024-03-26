namespace Roleta.Aplicacao.Dtos
{
    public class BancaPagadoraDto
    {
        public int Id { get; set; }
        public decimal SaldoDia { get; set; }
        public DateTime DataBanca { get; set; } = DateTime.Now;
    }
}
