using Roleta.Aplicacao.Dtos;
using Roleta.Dominio;

namespace Roleta.Aplicacao.Interface
{
    public interface IPagamentoService
    {
        Task<PagamentoDto> ConfirmarPagamento(string transactionId);
        Task<PagamentoDto> AddAsync(PagamentoDto model);
        Task<PagamentoDto> UpdateAsync(PagamentoDto model);
        Task<bool> DeleteAsync(int id);

        Task<PagamentoDto> GetByIdAsync(int id);
        Task<PagamentoDto[]> GetAllByUserIdAsync(Guid userId);
        Task<PagamentoDto[]> GetAllByStatusAsync(string status);
        Task<PagamentoDto> GetByTransactionIdAsync(string transactionId, bool includeProduto = false);
    }
}
