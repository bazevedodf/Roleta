using Roleta.Dominio;

namespace Roleta.Persistencia.Interface
{
    public interface IBancaPagadoraPersist : IGeralPersist
    {
        Task<BancaPagadora> GetByIdAsync(int id);
        Task<BancaPagadora[]> GetAllByRoletaIdAsync(int roletaId);
        Task<BancaPagadora[]> GetByDataRoletaIdAsync(int roletaId, DateTime data);
    }
}
