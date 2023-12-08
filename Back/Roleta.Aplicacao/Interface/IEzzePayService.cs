using Roleta.Aplicacao.Dtos;

namespace Roleta.Aplicacao.Interface
{
    public interface IEzzePayService
    {
        bool ValidaAssinaturaWebHook(string reqTimestamp, string requestPayload, string reqSignature);
        Task<string> GetTokenAsync();
        Task<PixDto> GetPixAsync(DadosPixDto dadosPix);
        Task<SaqueDto> SaquePix(SaqueDto saque, UserGameDto user);
        Task<PixDto> ConsultaPixAsync(string transactionId);
    }
}
