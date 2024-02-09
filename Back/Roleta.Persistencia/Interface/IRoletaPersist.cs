using Roleta.Dominio;

namespace Roleta.Persistencia.Interface
{
    public interface IRoletaPersist: IGeralPersist
    {
        Task<RoletaSorte> GetByIdAsync(int id, bool includeBancaDia = false, bool includeTransacoes = false);
    }
}
