using Roleta.Dominio;
using Roleta.Dominio.Identity;

namespace Roleta.Persistencia.Interface
{
    public interface IProdutoPersist : IGeralPersist
    {
        Task<Produto> GetByIdAsync(int id);
        Task<Produto[]> GetAllAsync(bool includeInativos = false);
    }
}
