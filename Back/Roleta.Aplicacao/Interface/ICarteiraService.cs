using Roleta.Aplicacao.Dtos;
using Roleta.Dominio;

namespace Roleta.Aplicacao.Interface
{
    public interface ICarteiraService 
    {
        Task<CarteiraDto> AddAsync(CarteiraDto model);
        Task<CarteiraDto> UpdateAsync(CarteiraDto model);
        Task<Transacao> AddTransacaoAsync(Transacao model);

        Task<CarteiraDto> GetByIdAsync(int id, bool includeTransacoes = false);
        Task<CarteiraDto> GetByUserIdAsync(Guid userId, bool includeTransacoes = false);
    }
}
