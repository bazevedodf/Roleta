using Roleta.Aplicacao.Dtos.Identity;
using Microsoft.AspNetCore.Identity;
using Roleta.Aplicacao.Dtos;

namespace Roleta.Aplicacao.Interface
{
    public interface IAccountService
    {
        Task<bool> EmailExists(string email);
        Task<bool> UserExists(string userName);
        Task<bool> CheckRoleAsync(UserDto userDto, string role);
        Task<bool> SetUserRole(UserDto userDto, string roleName);

        Task<UserDto> GetByIdAsync(Guid id, bool includeRole = false);
        Task<UserGameDto> GetUserGameAsync(string userLogin, bool includeDados = false);
        Task<UserDto> GetByUserLoginAsync(string userLogin);
        Task<SignInResult> CheckPasswordAsync(UserDto userDto, string password);
        Task<IdentityResult> ConfirmEmailAsync(UserDto userDto, string token);
        Task<IdentityResult> ResetPasswordAsync(UserDto userDto, string token, string newPassword);
        Task<string?> RecoverPasswordAsync(UserDto userDto);

        Task<UserDto> CreateUserAsync(UserDto userDto);
        Task<UserDto> UpdateUserAsync(UserDto userDto);
    }
}
