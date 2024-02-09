namespace Roleta.Dominio
{
    public class BancaPagadora
    {
        public int Id { get; set; }
        public decimal SaldoDia { get; set; }
        public DateTime DataBanca { get; set; } = DateTime.Now;
        public int RoletaSorteId { get; set; }
        public RoletaSorte? RoletaSorte { get; set; }
    }
}
