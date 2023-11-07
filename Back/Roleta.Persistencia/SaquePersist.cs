using Microsoft.EntityFrameworkCore;
using Roleta.Dominio;
using Roleta.Persistencia.Interface;

namespace Roleta.Persistencia
{
    public class SaquePersist : GeralPersist, ISaquePersist
    {
        private readonly RoletaContext _context;

        public SaquePersist(RoletaContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Saque> GetByIdAsync(int id)
        {
            IQueryable<Saque> query = _context.Saques.Where(x => x.Id == id);

            return await query.AsNoTracking().FirstOrDefaultAsync();
        }

        public async Task<Saque[]> GetAllByUserIdAsync(Guid userId)
        {
            IQueryable<Saque> query = _context.Saques.Where(x => x.User.Id == userId);

            return await query.AsNoTracking().OrderBy(x => x.DataCadastro).ToArrayAsync();
        }

        public async Task<Saque[]> GetAllByStatusAsync(string status)
        {
            IQueryable<Saque> query = _context.Saques.Where(x => x.Status.ToLower().Contains(status.ToLower()));

            return await query.AsNoTracking().OrderBy(x => x.DataCadastro).ToArrayAsync();
        }

        public async Task<Saque> GetByTransactionIdAsync(string transactionId)
        {
            IQueryable<Saque> query = _context.Saques.Where(x => x.TransactionId == transactionId);

            return await query.AsNoTracking().FirstOrDefaultAsync();
        }
    }
}
