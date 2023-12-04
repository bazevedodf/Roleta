using Roleta.Dominio;

namespace Roleta.Persistencia.Interface
{
    public interface ITransacaoPersist : IGeralPersist
    {
        Task<Transacao> GetByIdAsync(int id);
    }
}
