using Roleta.Aplicacao.Dtos;
using Roleta.Aplicacao.Dtos.Identity;

namespace Roleta.Aplicacao.Interface
{
    public interface IRoletaService
    {
        Task<GiroRoletaDto> GirarRoleta(int valorAposta, bool freeSpin = false, UserDto? user = null);
        Task<GiroRoletaDto> AddAsync(GiroRoletaDto model);
        Task<GiroRoletaDto> UpdateAsync(GiroRoletaDto model);
        Task<bool> DeleteAsync(int id);

        Task<GiroRoletaDto> GetByIdAsync(int id);
    }
}
