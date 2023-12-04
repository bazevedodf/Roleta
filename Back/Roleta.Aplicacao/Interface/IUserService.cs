using Roleta.Aplicacao.Dtos;
using Roleta.Aplicacao.Dtos.Identity;
using Roleta.Persistencia.Models;

namespace Roleta.Aplicacao.Interface
{
    public interface IUserService
    {
        Task<UserGameDto> UpdateUserGame(UserGameDto model, bool includeRole = false);
        Task<UserDashBoardDto> UpdateUserDashBoard(UserUpdateDashDto model, bool includeRole = false);

        Task<int> GetCountByParentEmail(string? parentEmail = null);
        Task<UserGameDto> GetUserGameByLoginAsync(string userLogin);
        Task<UserGameDto> GetUserGameByEmailAsync(string email, bool includeRole = false);
        Task<UserDashBoardDto> GetByUserLoginAsync(string userLogin, bool includeRole = false);
        Task<PageList<UserDashBoardDto>> GetAllByParentEmailDateAsync(PageParams pageParams, bool includePagamentos = false);
    }
}
