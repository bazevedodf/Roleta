using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Roleta.Aplicacao.Dtos;
using Roleta.Aplicacao.Dtos.Identity;
using Roleta.Aplicacao.Interface;
using Roleta.Dominio.Identity;
using Roleta.Persistencia.Interface;
using Roleta.Persistencia.Models;
using System.Text.RegularExpressions;

namespace Roleta.Aplicacao
{
    public class UserService: IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly IUserPersist _userPersist;
        private readonly IMapper _mapper;

        public UserService(UserManager<User> userManager,
                           RoleManager<Role> roleManager,
                           IUserPersist userPersist, 
                           IMapper mapper)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _userPersist = userPersist;
            _mapper = mapper;
        }

        public async Task<UpdateUserGameDto> PutUserGame(UpdateUserGameDto model)
        {
            try
            {
                var user = await _userPersist.GetByUserLoginAsync(model.Email);
                if (user == null) return null;

                model.Email = user.Email;
                _mapper.Map(model, user);

                _userPersist.Update(user);
                if (await _userPersist.SaveChangeAsync())
                {
                    var retorno = await _userPersist.GetByUserLoginAsync(user.Email);

                    return _mapper.Map<UpdateUserGameDto>(retorno);
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<UserDashBoardDto> UpdateUserDashBoard(UserUpdateDashDto model) 
        {
            try
            {
                var roleAfiliate = "Afiliate";
                var user = await _userPersist.GetByUserLoginAsync(model.Email, false);
                if (user == null) return null;

                model.Carteira.SaldoAtual = user.Carteira.SaldoAtual;
                _mapper.Map(model, user);

                if (string.IsNullOrEmpty(user.AfiliateCode)  && user.isAfiliate)
                {
                    user.AfiliateCode = GetAfiliateCode(user.Id);
                }

                _userPersist.Update(user);
                if (await _userPersist.SaveChangeAsync())
                {
                    if (user.isAfiliate)
                        await _userManager.AddToRoleAsync(user, roleAfiliate);
                    else
                        await _userManager.RemoveFromRoleAsync(user, roleAfiliate);
 
                    var retorno = await GetByUserLoginAsync(user.Email, true);

                    return retorno;
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<UserDashBoardDto> UpdateUserDashBoard(UserDashBoardDto model)
        {
            try
            {
                var roleAfiliate = "Afiliate";
                var user = await _userPersist.GetByUserLoginAsync(model.Email, false);
                if (user == null) return null;

                _mapper.Map(model, user);

                _userPersist.Update(user);
                if (await _userPersist.SaveChangeAsync())
                {
                    var retorno = await GetByUserLoginAsync(user.Email, true);
                    return retorno;
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<UserGameDto> UpdateUserGame(UserGameDto model, bool includeRole = false)
        {
            try
            {
                var user = await _userPersist.GetByUserLoginAsync(model.Email, includeRole);
                if (user == null) return null;

                _mapper.Map(model, user);

                if (string.IsNullOrEmpty(user.AfiliateCode) && user.isAfiliate)
                {
                    user.AfiliateCode = GetAfiliateCode(user.Id);
                }

                _userPersist.Update(user);
                if (await _userPersist.SaveChangeAsync())
                {
                    var retorno = await _userPersist.GetByUserLoginAsync(user.Email, includeRole);

                    return _mapper.Map<UserGameDto>(retorno);
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<int> GetCountByParentEmail(string? parentEmail = null)
        {
            try
            {
                return await _userPersist.GetCountByParentEmail(parentEmail);           
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao consultar usuários. Erro: {ex.Message}");
            }
        }

        public async Task<UserDashBoardDto> GetByUserLoginAsync(string userLogin, bool includeRole = false)
        {
            try
            {
                var result = await _userPersist.GetByUserLoginAsync(userLogin, includeRole);

                return _mapper.Map<UserDashBoardDto>(result);
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao consultar usuários. Erro: {ex.Message}");
            }
        }

        public async Task<UserGameDto> GetUserGameByEmailAsync(string email, bool includeRole = false)
        {
            try
            {
                var result = await _userPersist.GetByUserLoginAsync(email, includeRole);

                return _mapper.Map<UserGameDto>(result);
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao consultar usuários. Erro: {ex.Message}");
            }
        }

        public async Task<PageList<UserDashBoardDto>> GetAllByParentEmailDateAsync(PageParams pageParams, bool includePagamentos = false)
        {
            try
            {
                PageList<User>? users;

                if (string.IsNullOrEmpty(pageParams.Term))
                    users = await _userPersist.GetAllByParentEmailDateAsync(pageParams, includePagamentos);
                else
                    users = await _userPersist.GetAllByNomeDataAsync(pageParams, includePagamentos);

                if (users == null) return null;

                var resultado = _mapper.Map<PageList<UserDashBoardDto>>(users);

                resultado.CurrentPage = users.CurrentPage;
                resultado.TotalPages = users.TotalPages;
                resultado.PageSize = users.PageSize;
                resultado.TotalCount = users.TotalCount;

                return resultado;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao consultar usuários. Erro: {ex.Message}");
            }
        }

        public async Task<UserGameDto> GetByIdAsync(Guid userId)
        {
            try
            {
                var result = await _userPersist.GetByIdAsync(userId);

                return _mapper.Map<UserGameDto>(result);
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao consultar usuários. Erro: {ex.Message}");
            }
        }

        public async Task<UserGameDto> GetByAfiliateCodeAsync(string afiliateCode)
        {
            try
            {
                var result = await _userPersist.GetByAfiliateCodeAsync(afiliateCode);

                return _mapper.Map<UserGameDto>(result);
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao consultar usuários. Erro: {ex.Message}");
            }
        }

        public async Task<UserGameDto> GetUserGameByLoginAsync(string userLogin)
        {
            try
            {
                var result = await _userPersist.GetByUserLoginAsync(userLogin);

                return _mapper.Map<UserGameDto>(result);
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao consultar usuários. Erro: {ex.Message}");
            }
        }

        public string GetAfiliateCode(Guid userId)
        {
            //return Convert.ToBase64String(userId.ToByteArray()).Substring(0, 8);
            return Regex.Replace(Convert.ToBase64String(userId.ToByteArray()), "[/+=]", "").Substring(0, 8);
        }


        //Funcoes para afiliados
        public async Task<bool> ChangeSaldoAfiliados(decimal valor, bool includeBlocks = false)
        {
            try
            {
                var users = await _userPersist.GetAllAfiliatesAsync(includeBlocks);

                foreach (var user in users)
                {
                    user.Carteira.SaldoDemo = valor;
                    user.Carteira.SaldoAtual = 0;
                    user.ValorComissao = 10;
                    _userPersist.Update(user);
                }

                return await _userPersist.SaveChangeAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao zerar o saldo dos Afiliados. Erro: {ex.Message}");
            }
        }

        public async Task<PageList<AfiliadoDto>> GetAllAfiliatesAsync(PageParams pageParams, bool includeBlocks = false)
        {
            try
            {
                PageList<User>? users;

                if (string.IsNullOrEmpty(pageParams.Term))
                    users = await _userPersist.GetAllAfiliatesAsync(pageParams, includeBlocks);
                else
                    users = await _userPersist.GetAllAfiliatesNomeEmailAsync(pageParams, includeBlocks);

                if (users == null) return null;

                var resultado = _mapper.Map<PageList<AfiliadoDto>>(users);

                resultado.CurrentPage = users.CurrentPage;
                resultado.TotalPages = users.TotalPages;
                resultado.PageSize = users.PageSize;
                resultado.TotalCount = users.TotalCount;

                return resultado;
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro ao consultar Afiliados. Erro: {ex.Message}");
            }
        }

    }
}
