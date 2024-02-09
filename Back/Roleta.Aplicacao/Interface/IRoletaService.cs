using Roleta.Aplicacao.Dtos;

namespace Roleta.Aplicacao.Interface
{
    public interface IRoletaService
    {
        Task<bool> SaqueCaixa(decimal valorSaque, int roletaId);
        Task<bool> DepositoCaixa(PagamentoDto pagamento, int roletaId);
        Task<bool> ComissaoAfiliado(string emailAfiliado, decimal valorDeposito, string transacionId);

        Task<RoletaSorteDto> UpdateAsync(RoletaSorteDto model);
        Task<RoletaSorteDto> GetByIdAsync(int roletaId, bool includeBancaDia = false, bool includeTransacoes = false);
        Task<GiroRoletaDto> GirarRoleta(int valorAposta, bool freeSpin = false, UserGameDto? user = null);
    }
}
