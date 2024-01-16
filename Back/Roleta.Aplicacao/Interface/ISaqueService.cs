using Roleta.Aplicacao.Dtos;

namespace Roleta.Aplicacao.Interface
{
    public interface ISaqueService
    {
        Task<SaqueDto> ConfirmarSaquePix(SaqueDto saque);
        Task<SaqueDto> SolicitarSaquePix(UserGameDto user, decimal valor, decimal taxaSaque);
        Task<SaqueDto> AddAsync(SaqueDto model);
        Task<SaqueDto> UpdateAsync(SaqueDto model);
        Task<bool> DeleteAsync(int id);

        Task<SaqueDto> GetByIdAsync(int id);
        Task<SaqueDto[]> GetAllByUserIdAsync(Guid userId);
        Task<SaqueDto[]> GetAllByStatusAsync(string status);
        Task<SaqueDto> GetByTransactionIdAsync(string transactionId);
    }
}
