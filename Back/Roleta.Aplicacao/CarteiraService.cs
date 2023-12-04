using AutoMapper;
using Roleta.Aplicacao.Dtos;
using Roleta.Aplicacao.Interface;
using Roleta.Dominio;
using Roleta.Persistencia.Interface;

namespace Roleta.Aplicacao
{
    public class CarteiraService : ICarteiraService
    {
        public readonly ICarteiraPersist _carteiraPersist;
        private readonly ITransacaoPersist _transacaoPersist;
        public readonly IMapper _mapper;

        public CarteiraService(ICarteiraPersist carteiraPersist, 
                               ITransacaoPersist transacaoPersist,
                               IMapper mapper)
        {
            _carteiraPersist = carteiraPersist;
            _transacaoPersist = transacaoPersist;
            _mapper = mapper;
        }

        public async Task<CarteiraDto> AddAsync(CarteiraDto model)
        {
            try
            {
                var carteira = _mapper.Map<Carteira>(model);
                _carteiraPersist.Add(carteira);
                if (await _carteiraPersist.SaveChangeAsync())
                {
                    var retorno = await _carteiraPersist.GetByIdAsync(carteira.Id);
                    return _mapper.Map<CarteiraDto>(retorno);
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<CarteiraDto> UpdateAsync(CarteiraDto model)
        {
            try
            {
                var carteira = await _carteiraPersist.GetByIdAsync(model.Id);
                if (carteira == null) return null;

                _mapper.Map(model, carteira);
                _carteiraPersist.Update(carteira);
                if (await _carteiraPersist.SaveChangeAsync())
                {
                    var retorno = await _carteiraPersist.GetByIdAsync(carteira.Id);
                    return _mapper.Map<CarteiraDto>(retorno);
                }

                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<Transacao> AddTransacaoAsync(Transacao model)
        {
            try
            {
                _transacaoPersist.Add(model);
                if (await _transacaoPersist.SaveChangeAsync())
                {
                    var retorno = await _transacaoPersist.GetByIdAsync(model.Id);
                    return retorno;
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<CarteiraDto> GetByIdAsync(int id, bool includeTransacoes = false)
        {
            try
            {
                var carteira = await _carteiraPersist.GetByIdAsync(id, includeTransacoes);
                if (carteira == null) return null;

                return _mapper.Map<CarteiraDto>(carteira);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<CarteiraDto> GetByUserIdAsync(Guid userId, bool includeTransacoes = false)
        {
            try
            {
                var carteira = await _carteiraPersist.GetByUserIdAsync(userId, includeTransacoes);
                if (carteira == null) return null;

                return _mapper.Map<CarteiraDto>(carteira);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<Transacao> GetTrasacaoByIdAsync(int id)
        {
            try
            {
                var transacao = await _transacaoPersist.GetByIdAsync(id);
                if (transacao == null) return null;

                return transacao;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }
}
