using Roleta.Aplicacao.Dtos.Identity;

namespace Roleta.Aplicacao.Interface
{
    public interface ITokenService
    {
        Task<string> CreateToken(UserDto userDto);
    }
}
