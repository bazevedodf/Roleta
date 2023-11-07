using Roleta.Dominio.Identity;

namespace Roleta.Aplicacao.Dtos
{
    public class GiroRoletaDto
    {
        public int Id { get; set; }
        public int ValorAposta { get; set; }
        public int Posicao { get; set; }
        public decimal Multiplicador { get; set; }
        public Guid? UserId { get; set; }
        public User? User { get; set; }
        public DateTime Data { get; set; } = DateTime.Now;
    }
}
