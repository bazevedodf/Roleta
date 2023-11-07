using Roleta.Dominio;

namespace Roleta.Persistencia.Interface
{
    public interface IGiroRoletaPersist : IGeralPersist
    {
        Task<GiroRoleta> GetByIdAsync(int id);
    }
}
