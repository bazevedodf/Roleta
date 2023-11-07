using Roleta.Dominio.Identity;

namespace Roleta.Persistencia.Interface
{
    public interface IUserPersist: IGeralPersist
    {
        Task<User[]> GetAllAsync(bool includeRole = false);
        Task<User> GetByIdAsync(Guid id, bool includeRole = false);
        Task<User> GetByUserLoginAsync(string userLogin, bool includeRole = false);
        Task<User> GetUserGameAsync(string userLogin, bool icludeDados = false);
    }
}
