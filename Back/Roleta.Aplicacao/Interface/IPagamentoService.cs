using Roleta.Aplicacao.Dtos;
using Roleta.Persistencia.Models;

namespace Roleta.Aplicacao.Interface
{
    public interface IPagamentoService
    {
        Task<PagamentoDto> ConsultaDepositoPix(string transactionId);
        Task<PagamentoDto> ConfirmarDepositoPix(PagamentoDto pagamento);
        Task<PagamentoDto> AddAsync(PagamentoDto model);
        Task<PagamentoDto> UpdateAsync(PagamentoDto model);
        Task<bool> DeleteAsync(int id);

        Task<PagamentoDto> GetByIdAsync(int id);
        Task<PagamentoDto[]> GetAllByUserIdAsync(Guid userId);
        Task<PagamentoDto[]> GetAllByStatusAsync(string status);
        Task<int> GetAllAproveByParentEmailAsync(string? parentEmail = null);
        Task<PagamentoDto> GetByTransactionIdAsync(string transactionId);
        Task<PagamentoDto[]> GetAllAfiliateAsync(PageParams pageParams, bool somentePagos = false);
        Task<PageList<PagamentoDto>> GetAllByParentEmailAsync(PageParams pageParams, bool somentePagos = false);
    }
}
