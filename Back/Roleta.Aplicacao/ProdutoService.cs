using AutoMapper;
using Roleta.Aplicacao.Dtos;
using Roleta.Aplicacao.Interface;
using Roleta.Dominio;
using Roleta.Persistencia.Interface;

namespace Roleta.Aplicacao
{
    public class ProdutoService : IProdutoService
    {
        public readonly IProdutoPersist _produtoPersist;
        public readonly IMapper _mapper;

        public ProdutoService(IProdutoPersist produtoPersist, IMapper mapper)
        {
            _produtoPersist = produtoPersist;
            _mapper = mapper;
        }

        public async Task<ProdutoDto> AddAsync(ProdutoDto model)
        {
            try
            {
                var produto = _mapper.Map<Produto>(model);
                _produtoPersist.Add(produto);
                if (await _produtoPersist.SaveChangeAsync())
                {
                    var retorno = await _produtoPersist.GetByIdAsync(produto.Id);
                    return _mapper.Map<ProdutoDto>(retorno);
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ProdutoDto> UpdateAsync(ProdutoDto model)
        {
            try
            {
                var produto = await _produtoPersist.GetByIdAsync(model.Id);
                if (produto == null) return null;

                _mapper.Map(model, produto);
                _produtoPersist.Update(produto);
                if (await _produtoPersist.SaveChangeAsync())
                {
                    var retorno = await _produtoPersist.GetByIdAsync(produto.Id);
                    return _mapper.Map<ProdutoDto>(retorno);
                }

                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var produto = await _produtoPersist.GetByIdAsync(id);
                if (produto == null) throw new Exception("Produto não encontrado");

                _produtoPersist.Delete(produto);
                return await _produtoPersist.SaveChangeAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ProdutoDto[]> GetAllAsync(bool includeInativos = false)
        {
            try
            {
                var produtos = await _produtoPersist.GetAllAsync(includeInativos);
                if (produtos.Length == 0) return null;

                return _mapper.Map<ProdutoDto[]>(produtos);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ProdutoDto> GetByIdAsync(int id)
        {
            try
            {
                var produto = await _produtoPersist.GetByIdAsync(id);
                if (produto == null) return null;

                return _mapper.Map<ProdutoDto>(produto);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
