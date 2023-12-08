using Roleta.Dominio;
using Roleta.Persistencia.Models;

namespace Roleta.Persistencia.Interface
{
    public interface IPagamentoPersist: IGeralPersist
    {
        Task<Pagamento> GetByIdAsync(int id);
        Task<Pagamento[]> GetAllByUserIdAsync(Guid userId);
        Task<Pagamento[]> GetAllByStatusAsync(string status);
        Task<int> GetAllAproveByParentEmailAsync(string? parentEmail = null);
        Task<Pagamento> GetByTransactionIdAsync(string transactionId);
        Task<PageList<Pagamento>> GetAllByParentEmailAsync(PageParams pageParams, bool somentePagos = false);
    }
}
