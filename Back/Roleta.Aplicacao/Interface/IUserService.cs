using Roleta.Aplicacao.Dtos.Identity;
using Roleta.Dominio.Identity;
using Roleta.Persistencia.Models;

namespace Roleta.Aplicacao.Interface
{
    public interface IUserService
    {
        Task<int> GetCountByParentEmail(string? parentEmail = null);

        Task<PageList<UserDashBoardDto>> GetAllByParentEmailDateAsync(DateTime dataIni, DateTime dataFim, PageParams pageParams, bool includePagamentos = false);
    }
}
