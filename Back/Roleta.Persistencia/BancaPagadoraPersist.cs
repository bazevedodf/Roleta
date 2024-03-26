using Microsoft.EntityFrameworkCore;
using Roleta.Dominio;
using Roleta.Persistencia.Interface;

namespace Roleta.Persistencia
{
    public class BancaPagadoraPersist : GeralPersist, IBancaPagadoraPersist
    {
        private readonly RoletaContext _context;

        public  BancaPagadoraPersist(RoletaContext context) : base(context)
        {
            _context = context;
        }

        public async Task<BancaPagadora[]> GetAllByRoletaIdAsync(int roletaId)
        {
            throw new NotImplementedException();
        }

        public async Task<BancaPagadora[]> GetByDataRoletaIdAsync(int roletaId, DateTime data)
        {
            throw new NotImplementedException();
        }

        public async Task<BancaPagadora> GetByIdAsync(int id)
        {
            IQueryable<BancaPagadora> query = _context.BancasPagadoras.Where(x => x.Id == id);

            return await query.AsNoTracking().FirstOrDefaultAsync();
        }
    }
}
