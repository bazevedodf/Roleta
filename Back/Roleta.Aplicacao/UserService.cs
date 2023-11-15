using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Roleta.Aplicacao.Dtos.Identity;
using Roleta.Aplicacao.Interface;
using Roleta.Dominio.Identity;
using Roleta.Persistencia.Interface;
using Roleta.Persistencia.Models;

namespace Roleta.Aplicacao
{
    public class UserService: IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly IUserPersist _userPersist;
        private readonly IMapper _mapper;

        public UserService(UserManager<User> userManager, IUserPersist userPersist, IMapper mapper)
        {
            _userManager = userManager;
            _userPersist = userPersist;
            _mapper = mapper;
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

        //public async Task<UserDashBoardDto[]> GetAllByParentEmailDateAsync(DateTime dataIni, DateTime dataFim, string? parentEmail = null, bool includePagamentos = false)
        public async Task<PageList<UserDashBoardDto>> GetAllByParentEmailDateAsync(DateTime dataIni, DateTime dataFim, PageParams pageParams, bool includePagamentos = false)
        {
            try
            {
                var users = await _userPersist.GetAllByParentEmailDateAsync(dataIni, dataFim, pageParams, includePagamentos);
                
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

    }
}
