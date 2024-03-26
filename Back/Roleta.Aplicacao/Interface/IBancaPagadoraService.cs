using Roleta.Aplicacao.Dtos;

namespace Roleta.Aplicacao.Interface
{
    public interface IBancaPagadoraService
    {
        Task<BancaPagadoraDto> AddAsync(BancaPagadoraDto model);
        Task<BancaPagadoraDto> UpdateAsync(BancaPagadoraDto model);

        Task<BancaPagadoraDto> GetByIdAsync(int id);
        Task<BancaPagadoraDto[]> GetAllByRoletaIdAsync(int roletaId);
        Task<BancaPagadoraDto> GetByDataRoletaIdAsync(int roletaId, DateTime data);
    }
}
