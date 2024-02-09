using Roleta.Aplicacao.Dtos;
using Roleta.Aplicacao.Dtos.Identity;
using Roleta.Dominio.Identity;
using Roleta.Persistencia.Models;

namespace Roleta.Aplicacao.Interface
{
    public interface IUserService
    {
        Task<UpdateUserGameDto> PutUserGame(UpdateUserGameDto model);
        Task<UserGameDto> UpdateUserGame(UserGameDto model, bool includeRole = false);
        Task<UserDashBoardDto> UpdateUserDashBoard(UserUpdateDashDto model);
        Task<UserDashBoardDto> UpdateUserDashBoard(UserDashBoardDto model);

        Task<int> GetCountByParentEmail(string? parentEmail = null);
        Task<UserGameDto> GetByIdAsync(Guid userId);
        Task<UserGameDto> GetByAfiliateCodeAsync(string afiliateCode);
        Task<UserGameDto> GetUserGameByLoginAsync(string userLogin);
        Task<UserGameDto> GetUserGameByEmailAsync(string email, bool includeRole = false);
        Task<UserDashBoardDto> GetByUserLoginAsync(string userLogin, bool includeRole = false);
        Task<PageList<UserDashBoardDto>> GetAllByParentEmailDateAsync(PageParams pageParams, bool includePagamentos = false);


        //Funcoes para Afiliados
        Task<bool> ChangeSaldoAfiliados(decimal valor, bool includeBlocks = false);
        Task<PageList<AfiliadoDto>> GetAllAfiliatesAsync(PageParams pageParams, bool includeBlocks = false);
    }
}
