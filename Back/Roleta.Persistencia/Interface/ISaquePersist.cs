using Roleta.Dominio;

namespace Roleta.Persistencia.Interface
{
    public interface ISaquePersist: IGeralPersist
    {
        Task<Saque> GetByIdAsync(int id);
        Task<Saque[]> GetAllByUserIdAsync(Guid userId);
        Task<Saque[]> GetAllByStatusAsync(string status);
        Task<Saque> GetByTransactionIdAsync(string transactionId);
    }
}
