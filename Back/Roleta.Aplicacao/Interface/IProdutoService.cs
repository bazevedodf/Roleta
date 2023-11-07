using Roleta.Aplicacao.Dtos;

namespace Roleta.Aplicacao.Interface
{
    public interface IProdutoService
    {
        Task<ProdutoDto> AddAsync(ProdutoDto model);
        Task<ProdutoDto> UpdateAsync(ProdutoDto model);
        Task<bool> DeleteAsync(int id);

        Task<ProdutoDto> GetByIdAsync(int id);
        Task<ProdutoDto[]> GetAllAsync(bool includeInativos = false);
    }
}
