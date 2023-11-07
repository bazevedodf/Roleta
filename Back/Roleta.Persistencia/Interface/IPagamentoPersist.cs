using Roleta.Dominio;

namespace Roleta.Persistencia.Interface
{
    public interface IPagamentoPersist: IGeralPersist
    {
        Task<Pagamento> GetByIdAsync(int id);
        Task<Pagamento[]> GetAllByUserIdAsync(Guid userId);
        Task<Pagamento[]> GetAllByStatusAsync(string status);
        Task<Pagamento> GetByTransactionIdAsync(string transactionId, bool includeProduto = false);
    }
}
