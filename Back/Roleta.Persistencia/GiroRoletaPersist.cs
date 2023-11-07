using Microsoft.EntityFrameworkCore;
using Roleta.Dominio;
using Roleta.Persistencia.Interface;

namespace Roleta.Persistencia
{
    public class GiroRoletaPersist : GeralPersist, IGiroRoletaPersist
    {
        private readonly RoletaContext _context;

        public GiroRoletaPersist(RoletaContext context) : base(context)
        {
            _context = context;
        }

        public async Task<GiroRoleta> GetByIdAsync(int id)
        {
            IQueryable<GiroRoleta> query = _context.GirosRoleta.Where(x => x.Id == id);

            return await query.AsNoTracking().FirstOrDefaultAsync();
        }
    }
}
