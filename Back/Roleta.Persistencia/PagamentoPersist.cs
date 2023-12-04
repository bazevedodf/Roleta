using Microsoft.EntityFrameworkCore;
using Roleta.Dominio;
using Roleta.Persistencia.Interface;
using Roleta.Persistencia.Models;

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

        public async Task<PageList<Pagamento>> GetAllByParentEmailAsync(PageParams pageParams)
        {
            IQueryable<Pagamento> query = _context.Pagamentos.Include(x => x.User)
                                                  .Where(x => x.DataCadastro >= pageParams.DataIni
                                                           && x.DataCadastro < pageParams.DataFim
                                                           && x.User.ParentEmail.ToLower().Contains(pageParams.Term.ToLower()))
                                                  .OrderByDescending(x => x.DataCadastro);

            return await PageList<Pagamento>.CreateAsync(query, pageParams.PageNumber, pageParams.pageSize);
        }

        public async Task<int> GetAllAproveByParentEmailAsync(string? parentEmail = null)
        {
            var query = _context.Pagamentos.Join(_context.Users, pg => pg.UserId, us => us.Id, (pg, us) => new { pg, us });

            if (string.IsNullOrEmpty(parentEmail))
                return await query.CountAsync(_ => _.pg.Status == "APPROVED");
            else
                return await query.Where(_ => _.pg.Status == "APPROVED" && _.us.ParentEmail.ToLower() == parentEmail.ToLower())
                                  .Select(x => x.pg.UserId).Distinct().CountAsync();
        }

        public async Task<Pagamento[]> GetAllByStatusAsync(string status)
        {
            IQueryable<Pagamento> query = _context.Pagamentos.Where(x => x.Status.ToLower().Contains(status.ToLower()));

            return await query.AsNoTracking().OrderByDescending(x => x.DataCadastro).ToArrayAsync();
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
