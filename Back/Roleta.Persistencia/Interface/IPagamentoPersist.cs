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
        Task<PageList<Pagamento>> GetAllByParentEmailAsync(DateTime dataIni, DateTime dataFim, PageParams pageParams);
        Task<Pagamento> GetByTransactionIdAsync(string transactionId, bool includeProduto = false);
    }
}
