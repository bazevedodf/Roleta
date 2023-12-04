using Microsoft.EntityFrameworkCore;
using Roleta.Dominio;
using Roleta.Persistencia.Interface;

namespace Roleta.Persistencia
{
    public class TransacaoPersist : GeralPersist, ITransacaoPersist
    {
        private readonly RoletaContext _context;

        public TransacaoPersist(RoletaContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Transacao> GetByIdAsync(int id)
        {
            IQueryable<Transacao> query = _context.Transacoes.Where(x => x.Id == id);

            return await query.AsNoTracking().FirstOrDefaultAsync();
        }
    }
}
