using Microsoft.EntityFrameworkCore;
using Roleta.Dominio.Identity;
using Roleta.Persistencia.Interface;

namespace Roleta.Persistencia
{
    public class UserPersist : GeralPersist, IUserPersist
    {
        private readonly RoletaContext _context;

        public UserPersist(RoletaContext context) : base(context)
        {
            _context = context;
        }

        public async Task<User[]> GetAllAsync(bool includeRole = false)
        {
            IQueryable<User> query = _context.Users;

            if (includeRole)
                query = query.Include(x => x.UserRoles)
                             .ThenInclude(x => x.Role);

            return await query.AsNoTracking().OrderBy(x => x.UserName).ToArrayAsync();
        }

        public async Task<User> GetByIdAsync(Guid id, bool includeRole = false)
        {
            IQueryable<User> query = _context.Users.Where(x => x.Id == id);

            if (includeRole)
                query = query.Include(x => x.UserRoles)
                             .ThenInclude(x => x.Role);

            return await query.AsNoTracking().FirstOrDefaultAsync();
        }

        public async Task<User> GetByUserLoginAsync(string userLogin, bool includeRole = false)
        {
            IQueryable<User> query = _context.Users.Where(x => x.UserName.ToLower() == userLogin.ToLower() || 
                                                               x.Email.ToLower() == userLogin.ToLower());
            if (includeRole)
                query = query.Include(x => x.UserRoles)
                             .ThenInclude(x => x.Role);

            return await query.AsNoTracking().FirstOrDefaultAsync();
        }

        public async Task<User> GetUserGameAsync(string userLogin, bool icludeDados = false)
        {
            IQueryable<User> query = _context.Users.Where(x => x.UserName.ToLower() == userLogin.ToLower() ||
                                                               x.Email.ToLower() == userLogin.ToLower());
            if (icludeDados)
            {
                query = query.Include(x => x.Pagamentos);
                query = query.Include(x => x.Saques);
                query = query.Include(x => x.GirosRoleta);
            }                

            return await query.AsNoTracking().FirstOrDefaultAsync();
        }
    }
}
