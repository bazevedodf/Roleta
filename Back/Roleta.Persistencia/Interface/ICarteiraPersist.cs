using Roleta.Dominio;

namespace Roleta.Persistencia.Interface
{
    public interface ICarteiraPersist : IGeralPersist
    {
        Task<Carteira> GetByIdAsync(int id, bool includeTransacoes = false);
        Task<Carteira> GetByUserIdAsync(Guid userId, bool includeTransacoes = false);
        Task<Carteira[]> GetAllAsync(bool includeTransacoes = false);
    }
}
