using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Roleta.Aplicacao.Dtos;
using Roleta.Aplicacao.Dtos.Identity;
using Roleta.Aplicacao.Interface;
using Roleta.Dominio;
using Roleta.Dominio.Identity;
using Roleta.Persistencia.Interface;

namespace Roleta.Aplicacao
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IUserPersist _userPersist;
        private readonly IEmailService _emailService;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;

        public AccountService(UserManager<User> userManager,
                              SignInManager<User> signInManager,
                              IUserPersist userPersist,
                              IEmailService emailService,
                              ITokenService tokenService,
                              IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _userPersist = userPersist;
            _emailService = emailService;
            _tokenService = tokenService;
            _mapper = mapper;
        }

        public async Task<UserDto> CreateUserAsync(UserDto userDto)
        {
            try
            {
                userDto.UserName = userDto.UserName.IsNullOrEmpty() ? userDto.Email : userDto.UserName;
                var user = _mapper.Map<User>(userDto);

                var result = await _userManager.CreateAsync(user, userDto.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "User");

                    user.Carteira = new Carteira{UserId = user.Id};
                    await _userManager.UpdateAsync(user);

                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                    var retorno = _mapper.Map<UserDto>(user);
                    //retorno.Token = code;//_tokenService.CreateToken(retorno).Result;
                    return retorno;
                }

                return null;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao tentar criar conta. Erro: {ex.Message}");
            }
        }

        public async Task<SignInResult> CheckPasswordAsync(UserDto userDto, string password)
        {
            try
            {
                var user = await _userManager.Users.SingleOrDefaultAsync(user => user.Email.ToLower() == userDto.Email.ToLower());
                return await _signInManager.CheckPasswordSignInAsync(user, password, true); //.CheckPasswordSignInAsync(user, password, true);
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao tentar verificar Password. Erro: {ex.Message}");
            }
        }

        public async Task<IdentityResult> ConfirmEmailAsync(UserDto userDto, string token)
        {
            try
            {
                var user = await _userManager.Users.SingleOrDefaultAsync(user => user.Email.ToLower() == userDto.Email.ToLower());
                if (!await _userManager.IsEmailConfirmedAsync(user))
                    return await _userManager.ConfirmEmailAsync(user, token);
                else
                    return null;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao tentar verificar Password. Erro: {ex.Message}");
            }
        }

        public async Task<IdentityResult> ResetPasswordAsync(UserDto userDto, string token, string newPassword)
        {
            try
            {
                var user = await _userManager.Users.SingleOrDefaultAsync(user => user.Email.ToLower() == userDto.Email.ToLower());
                return await _userManager.ResetPasswordAsync(user, token, newPassword);
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao tentar verificar Password. Erro: {ex.Message}");
            }
        }

        public async Task<string?> RecoverPasswordAsync(UserDto userDto)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(userDto.Email);

                if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
                {
                    // Não revele que o usuário não existe ou não está confirmado
                    return null;
                }

                return await _userManager.GeneratePasswordResetTokenAsync(user);
                
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao tentar verificar Password. Erro: {ex.Message}");
            }
        }

        public async Task<UserDto> UpdateUserAsync(UserDto userDto)
        {
            try
            {
                var user = await _userManager.Users.SingleOrDefaultAsync(user => user.Email.ToLower() == userDto.Email.ToLower());
                if (user == null) return null;

                _mapper.Map(userDto, user);
                
                if(userDto.Password != null)
                {
                    var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                    var result = await _userManager.ResetPasswordAsync(user, token, userDto.Password);
                }                

                _userPersist.Update<User>(user);
                if (await _userPersist.SaveChangeAsync())
                {
                    var retorno = await _userPersist.GetByUserLoginAsync(userDto.Email, true);
                    return _mapper.Map<UserDto>(retorno);
                }

                return null;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao tentar atualizar Usuário. Erro: {ex.Message}");
            }
        }

        public async Task<bool> UserExists(string userName)
        {
            try
            {
                return await _userManager.Users.AnyAsync(user => user.UserName.ToLower() == userName.ToLower());
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao verificar se o Usuário existe. Erro: {ex.Message}");
            }
        }

        public async Task<bool> EmailExists(string email)
        {
            try
            {
                return await _userManager.Users.AnyAsync(user => user.Email.ToLower() == email.ToLower());
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao verificar se o e-mail existe. Erro: {ex.Message}");
            }
        }

        public async Task<bool> CheckRoleAsync(UserDto userDto, string role)
        {
            try
            {
                var user = _mapper.Map<User>(userDto);
                var roles = await _userManager.GetRolesAsync(user);
                if (roles.Contains(role))
                    return true;

                return false;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao tentar verificar Role. Erro: {ex.Message}");
            }
        }

        public async Task<bool> SetUserRole(UserDto userDto, string roleName)
        {
            var user = await _userManager.Users.SingleOrDefaultAsync(user => user.Email.ToLower() == userDto.Email.ToLower());
            if (user == null) return false;

            var result = await _userManager.AddToRoleAsync(user, roleName);

            return result.Succeeded;
        }

        public async Task<UserDto> GetByIdAsync(Guid id, bool includeRole = false)
        {
            try
            {
                var user = await _userPersist.GetByIdAsync(id, includeRole);
                if (user == null) return null;

                var retorno = _mapper.Map<UserDto>(user);
                return retorno;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao tentar encontrar Usuário. Erro: {ex.Message}");
            }
        }

        public async Task<UserDto> GetByUserLoginAsync(string userLogin, bool includeRole = false)
        {
            try
            {
                var user = await _userPersist.GetByUserLoginAsync(userLogin, includeRole);
                if (user == null) return null;

                var retorno = _mapper.Map<UserDto>(user);
                return retorno;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao tentar encontrar Usuário. Erro: {ex.Message}");
            }
        }

        public async Task<UserGameDto> GetUserGameAsync(string userLogin, bool includeDados = false)
        {
            try
            {
                var user = await _userPersist.GetUserGameAsync(userLogin, includeDados);
                if (user == null) return null;

                var retorno = _mapper.Map<UserGameDto>(user);
                return retorno;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao tentar encontrar Usuário. Erro: {ex.Message}");
            }
        }
    }
}
