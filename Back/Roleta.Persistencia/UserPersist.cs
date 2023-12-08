using Microsoft.EntityFrameworkCore;
using Roleta.Dominio.Identity;
using Roleta.Persistencia.Interface;
using Roleta.Persistencia.Models;

namespace Roleta.Persistencia
{
    public class UserPersist : GeralPersist, IUserPersist
    {
        private readonly RoletaContext _context;

        public UserPersist(RoletaContext context) : base(context)
        {
            _context = context;
        }

        public async Task<int> GetCountByParentEmail(string? parentEmail = null)
        {
            var count = !string.IsNullOrEmpty(parentEmail)
                ? await _context.Users.CountAsync(x => x.ParentEmail.ToLower() == parentEmail.ToLower())
                : await _context.Users.CountAsync();

            return count;
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

            query = query.Include(x => x.Carteira);

            if (includeRole)
                query = query.Include(x => x.UserRoles)
                             .ThenInclude(x => x.Role);

            return await query.AsNoTracking().FirstOrDefaultAsync();
        }

        public async Task<User> GetByUserLoginAsync(string userLogin, bool includeRole = false)
        {
            IQueryable<User> query = _context.Users.Where(x => x.UserName.ToLower() == userLogin.ToLower() || 
                                                               x.Email.ToLower() == userLogin.ToLower());
            query = query.Include(x => x.Carteira);

            if (includeRole)
                query = query.Include(x => x.UserRoles)
                             .ThenInclude(x => x.Role);

            return await query.AsNoTracking().FirstOrDefaultAsync();
        }

        public async Task<User[]> GetAllByParentEmailAsync(string parentEmail, bool includeRole = false)
        {
            IQueryable<User> query = _context.Users.Where(x => x.ParentEmail.ToLower() == parentEmail.ToLower());
            if (includeRole)
                query = query.Include(x => x.UserRoles)
                             .ThenInclude(x => x.Role);

            return await query.AsNoTracking().OrderBy(x => x.DataCadastro).ToArrayAsync();
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

        public async Task<PageList<User>> GetAllByParentEmailDateAsync(PageParams pageParams, bool includePagamentos = false)
        {
            IQueryable<User> query = _context.Users;
            if (!string.IsNullOrEmpty(pageParams.Term))
            {
                query = query.Where(x => x.ParentEmail.ToLower().Contains(pageParams.ParentEmail.ToLower())
                                        && (x.Email.ToLower().Contains(pageParams.Term.ToLower()) 
                                        || x.FirstName.ToLower().Contains(pageParams.Term.ToLower())));
            }
            else
            {
                query = query.Where(x => x.DataCadastro >= pageParams.DataIni
                                        && x.DataCadastro < pageParams.DataFim);
            }

            query = query.Include(x => x.Carteira);

            if (includePagamentos)
                query = query.Include(x => x.Pagamentos);

            query = query.OrderByDescending(x => x.DataCadastro).AsNoTracking();

            return await PageList<User>.CreateAsync(query, pageParams.PageNumber, pageParams.pageSize);
        }
    }
}
