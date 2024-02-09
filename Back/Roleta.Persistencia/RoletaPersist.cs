using Microsoft.EntityFrameworkCore;
using Roleta.Dominio;
using Roleta.Persistencia.Interface;

namespace Roleta.Persistencia
{
    public class RoletaPersist : GeralPersist, IRoletaPersist
    {
        private readonly RoletaContext _context;

        public RoletaPersist(RoletaContext context) : base(context)
        {
            _context = context;
        }

        public async Task<RoletaSorte> GetByIdAsync(int id, bool includeBancaDia = false, bool includeTransacoes = false)
        {
            IQueryable<RoletaSorte> query = _context.Roletas.Where(x => x.Id == id);

            if (includeBancaDia)
                query = query.Include(x => x.BancasPagadoras.Where(x => x.DataBanca.Date > DateTime.Now.Date));

            if (includeTransacoes)
                query = query.Include(x => x.TransacoesRoleta);

            return await query.AsNoTracking().FirstOrDefaultAsync();
        }
    }
}
