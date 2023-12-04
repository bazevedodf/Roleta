using Roleta.Dominio;

namespace Roleta.Persistencia.Interface
{
    public interface ITransacaoRoletaPersist: IGeralPersist
    {
        Task<TransacaoRoleta> GetByIdAsync(int id);
        Task<TransacaoRoleta[]> GetAllRoletaIdAsync(int roletaId);
    }
}
