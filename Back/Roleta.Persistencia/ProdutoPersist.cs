using Microsoft.EntityFrameworkCore;
using Roleta.Dominio;
using Roleta.Persistencia.Interface;

namespace Roleta.Persistencia
{
    public class ProdutoPersist : GeralPersist, IProdutoPersist
    {
        private readonly RoletaContext _context;

        public ProdutoPersist(RoletaContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Produto[]> GetAllAsync(bool includeInativos = false)
        {
            IQueryable<Produto> query = _context.Produtos;

            if (!includeInativos)
                query = query.Where(x => x.Ativo == true);

            return await query.AsNoTracking().OrderBy(x => x.Id).ToArrayAsync();
        }

        public async Task<Produto> GetByIdAsync(int id)
        {
            IQueryable<Produto> query = _context.Produtos.Where(x => x.Id == id);

            return await query.AsNoTracking().FirstOrDefaultAsync();
        }
    }
}
