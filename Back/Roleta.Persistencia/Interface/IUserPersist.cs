using Roleta.Dominio.Identity;
using Roleta.Persistencia.Models;

namespace Roleta.Persistencia.Interface
{
    public interface IUserPersist: IGeralPersist
    {
        Task<User[]> GetAllAsync(bool includeRole = false);
        Task<User> GetByIdAsync(Guid id, bool includeRole = false);
        Task<int> GetCountByParentEmail(string? parentEmail = null);
        Task<User> GetByUserLoginAsync(string userLogin, bool includeRole = false);
        Task<User> GetUserGameAsync(string userLogin, bool icludeDados = false);
        Task<User[]> GetAllByParentEmailAsync(string parentEmail, bool includeRole = false);
        Task<PageList<User>> GetAllByParentEmailDateAsync(DateTime dataIni, DateTime dataFim, PageParams pageParams, bool includePagamentos = false);
    }
}
