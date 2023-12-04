using Microsoft.EntityFrameworkCore;
using Roleta.Dominio;
using Roleta.Persistencia.Interface;

namespace Roleta.Persistencia
{
    public class TransacaoRoletaPersist: GeralPersist, ITransacaoRoletaPersist
    {
        private readonly RoletaContext _context;

        public TransacaoRoletaPersist(RoletaContext context) : base(context)
        {
            _context = context;
        }

        public async Task<TransacaoRoleta> GetByIdAsync(int id)
        {
            IQueryable<TransacaoRoleta> query = _context.TransacoesRoleta.Where(x => x.Id == id);

            return await query.AsNoTracking().FirstOrDefaultAsync();
        }

        public async Task<TransacaoRoleta[]> GetAllRoletaIdAsync(int roletaId)
        {
            IQueryable<TransacaoRoleta> query = _context.TransacoesRoleta.Where(x => x.RoletaId == roletaId);

            return await query.AsNoTracking().OrderByDescending(x => x.Data).ToArrayAsync();
        }
    }
}
