using AutoMapper;
using Roleta.Aplicacao.Dtos;
using Roleta.Aplicacao.Interface;
using Roleta.Dominio;
using Roleta.Persistencia.Interface;

namespace Roleta.Aplicacao
{
    public class SaqueService : ISaqueService
    {
        private readonly ISaquePersist _saquePersist;
        private readonly IMapper _mapper;

        public SaqueService(ISaquePersist saquePersist, IMapper mapper)
        {
            _saquePersist = saquePersist;
            _mapper = mapper;
        }

        public async Task<SaqueDto> AddAsync(SaqueDto model)
        {
            try
            {
                var saque = _mapper.Map<Saque>(model);

                _saquePersist.Add(saque);
                if (await _saquePersist.SaveChangeAsync())
                {
                    var retorno = await _saquePersist.GetByIdAsync(saque.Id);
                    return _mapper.Map<SaqueDto>(retorno);
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<SaqueDto> UpdateAsync(SaqueDto model)
        {
            try
            {
                var saque = await _saquePersist.GetByIdAsync(model.Id);
                if (saque == null) return null;

                _mapper.Map(model, saque);
                _saquePersist.Update(saque);
                if (await _saquePersist.SaveChangeAsync())
                {
                    var retorno = await _saquePersist.GetByIdAsync(saque.Id);
                    return _mapper.Map<SaqueDto>(retorno);
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
                var saque = await _saquePersist.GetByIdAsync(id);
                if (saque == null) throw new Exception("Pagamento não encontrado");

                _saquePersist.Delete(saque);
                return await _saquePersist.SaveChangeAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<SaqueDto> GetByIdAsync(int id)
        {
            try
            {
                var saque = await _saquePersist.GetByIdAsync(id);
                if (saque == null) return null;

                return _mapper.Map<SaqueDto>(saque);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<SaqueDto[]> GetAllByUserIdAsync(Guid userId)
        {
            try
            {
                var saques = await _saquePersist.GetAllByUserIdAsync(userId);
                if (saques.Length <= 0) return null;

                return _mapper.Map<SaqueDto[]>(saques);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<SaqueDto[]> GetAllByStatusAsync(string status)
        {
            try
            {
                var saques = await _saquePersist.GetAllByStatusAsync(status);
                if (saques.Length <= 0) return null;

                return _mapper.Map<SaqueDto[]>(saques);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<SaqueDto> GetByTransactionIdAsync(string transactionId)
        {
            try
            {
                var saques = await _saquePersist.GetByTransactionIdAsync(transactionId);
                if (saques == null) return null;

                return _mapper.Map<SaqueDto>(saques);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
