using AutoMapper;
using Roleta.Aplicacao.Dtos;
using Roleta.Aplicacao.Interface;
using Roleta.Dominio;
using Roleta.Persistencia;
using Roleta.Persistencia.Interface;

namespace Roleta.Aplicacao
{
    public class BancaPagadoraService : IBancaPagadoraService
    {
        private readonly IBancaPagadoraPersist _bancaPersist;
        private readonly IMapper _mapper;

        public BancaPagadoraService(IBancaPagadoraPersist bancaPersist,
                                    IMapper mapper)
        {
            _bancaPersist = bancaPersist;
            _mapper = mapper;
        }

        public async Task<BancaPagadoraDto> AddAsync(BancaPagadoraDto model)
        {
            try
            {
                var banca = _mapper.Map<BancaPagadora>(model);
                _bancaPersist.Add(banca);
                if (await _bancaPersist.SaveChangeAsync())
                {
                    var retorno = await _bancaPersist.GetByIdAsync(banca.Id);
                    return _mapper.Map<BancaPagadoraDto>(retorno);
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<BancaPagadoraDto> UpdateAsync(BancaPagadoraDto model)
        {
            try
            {
                var banca = await _bancaPersist.GetByIdAsync(model.Id);
                if (banca == null) return null;

                _mapper.Map(model, banca);
                _bancaPersist.Update(banca);
                if (await _bancaPersist.SaveChangeAsync())
                {
                    var retorno = await _bancaPersist.GetByIdAsync(banca.Id);
                    return _mapper.Map<BancaPagadoraDto>(retorno);
                }

                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<BancaPagadoraDto> GetByIdAsync(int id)
        {
            try
            {
                var banca = await _bancaPersist.GetByIdAsync(id);
                if (banca == null) return null;

                return _mapper.Map<BancaPagadoraDto>(banca);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<BancaPagadoraDto[]> GetAllByRoletaIdAsync(int roletaId)
        {
            try
            {
                var banca = await _bancaPersist.GetAllByRoletaIdAsync(roletaId);
                if (banca == null) return null;

                return _mapper.Map<BancaPagadoraDto[]>(banca);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<BancaPagadoraDto> GetByDataRoletaIdAsync(int roletaId, DateTime data)
        {
            try
            {
                var banca = await _bancaPersist.GetByDataRoletaIdAsync(roletaId, data);
                if (banca == null) return null;

                return _mapper.Map<BancaPagadoraDto>(banca);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
       
    }
}
