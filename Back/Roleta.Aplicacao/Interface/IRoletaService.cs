using Roleta.Aplicacao.Dtos;
using Roleta.Dominio;

namespace Roleta.Aplicacao.Interface
{
    public interface IRoletaService
    {
        Task<RoletaSorteDto> GetByIdAsync(int roletaId, bool includeTransacoes = false);
        Task<GiroRoletaDto> GirarRoleta(int valorAposta, bool freeSpin = false, UserGameDto? user = null);
    }
}
