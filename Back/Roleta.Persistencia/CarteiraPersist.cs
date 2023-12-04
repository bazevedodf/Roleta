using Microsoft.EntityFrameworkCore;
using Roleta.Dominio;
using Roleta.Persistencia.Interface;

namespace Roleta.Persistencia
{
    public class CarteiraPersist : GeralPersist, ICarteiraPersist
    {
        private readonly RoletaContext _context;

        public CarteiraPersist(RoletaContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Carteira> GetByIdAsync(int id, bool includeTransacoes = false)
        {
            IQueryable<Carteira> query = _context.Carteiras.Where(x => x.Id == id);

            if(includeTransacoes )
                query = query.Include(x => x.Transacoes);

            return await query.AsNoTracking().FirstOrDefaultAsync();
        }

        public async Task<Carteira> GetByUserIdAsync(Guid userId, bool includeTransacoes = false)
        {
            IQueryable<Carteira> query = _context.Carteiras.Where(x => x.UserId == userId);

            if (includeTransacoes)
                query = query.Include(x => x.Transacoes);

            return await query.AsNoTracking().FirstOrDefaultAsync();
        }

        public async Task<Carteira[]> GetAllAsync(bool includeTransacoes = false)
        {
            throw new NotImplementedException();
        }

       
    }
}
