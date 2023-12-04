namespace Roleta.Aplicacao.Dtos
{
    public class TransacaoRoletaDto
    {
        public int Id { get; set; }
        public decimal valor { get; set; }
        public DateTime Data { get; set; } = DateTime.Now;
        public int TransacaoId { get; set; }
        public TransacaoDto? TransacaoUser { get; set; }
        public int RoletaId { get; set; }
        public RoletaSorteDto? RoletaSorte { get; set; }
    }
}
