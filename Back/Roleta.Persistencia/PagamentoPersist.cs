using Microsoft.EntityFrameworkCore;
using Roleta.Dominio;
using Roleta.Persistencia.Interface;

namespace Roleta.Persistencia
{
    public class PagamentoPersist : GeralPersist, IPagamentoPersist
    {
        private readonly RoletaContext _context;

        public PagamentoPersist(RoletaContext context): base(context)
        {
            _context = context;
        }

        public async Task<Pagamento> GetByIdAsync(int id)
        {
            IQueryable<Pagamento> query = _context.Pagamentos.Where(x => x.Id == id);

            return await query.AsNoTracking().FirstOrDefaultAsync();
        }

        public async Task<Pagamento[]> GetAllByUserIdAsync(Guid userId)
        {
            IQueryable<Pagamento> query = _context.Pagamentos.Where(x => x.User.Id == userId);

            return await query.AsNoTracking().OrderBy(x => x.DataCadastro).ToArrayAsync();
        }

        public async Task<Pagamento[]> GetAllByStatusAsync(string status)
        {
            IQueryable<Pagamento> query = _context.Pagamentos.Where(x => x.Status.ToLower().Contains(status.ToLower()));

            return await query.AsNoTracking().OrderBy(x => x.DataCadastro).ToArrayAsync();
        }

        public async Task<Pagamento> GetByTransactionIdAsync(string transactionId, bool includeProduto = false)
        {
            IQueryable<Pagamento> query = _context.Pagamentos.Where(x => x.TransactionId == transactionId);

            if (includeProduto)
            {
                query = query.Include(x => x.Produto);
            }

            return await query.AsNoTracking().FirstOrDefaultAsync();
        }
    }
}
