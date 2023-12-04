namespace Roleta.Dominio
{
    public class TransacaoRoleta
    {
        public int Id { get; set; }
        public decimal valor { get; set; }
        public string Descricao { get; set; }
        public DateTime Data { get; set; } = DateTime.Now;
        public int TransacaoId{ get; set; }
        public Transacao? TransacaoUser { get; set; }
        public int RoletaId { get; set; }
        public RoletaSorte? RoletaSorte { get; set; }
    }
}
