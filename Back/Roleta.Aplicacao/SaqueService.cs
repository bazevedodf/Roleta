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
        private readonly IEzzePayService _ezzePayService;
        private readonly IUserService _userService;
        private readonly ICarteiraService _carteiraService;
        private readonly IMapper _mapper;

        public SaqueService(ISaquePersist saquePersist,
                            IEzzePayService ezzePayService,
                            IUserService userService,
                            ICarteiraService carteiraService,
                            IMapper mapper)
        {
            _saquePersist = saquePersist;
            _ezzePayService = ezzePayService;
            _userService = userService;
            _carteiraService = carteiraService;
            _mapper = mapper;
        }

        public async Task<SaqueDto> SolicitarSaquePix(UserGameDto user, decimal valor, decimal taxaSaque, string descricao)
        {
            try
            {
                user.Carteira.SaldoAtual -= valor;
                user.Carteira.DataAtualizacao = DateTime.Now;

                var saque = new SaqueDto()
                {
                    UserId = user.Id,
                    Valor = valor - taxaSaque,
                    Status = "PROCESSING",
                    Description = descricao
                };

                saque = await _ezzePayService.SaquePix(saque, user);
                if (saque != null)
                {
                    var retornoUser = await _userService.UpdateUserGame(user);
                    //var trasacaoUser = new Transacao()
                    //{
                    //    CarteiraId = user.Carteira.Id,
                    //    valor = decimal.Negate(valor),
                    //    TransactionId = saque.TransactionId,
                    //    Tipo = saque.Description,
                    //    Data = saque.DataStatus
                    //};

                    //var returnTransacao = await _carteiraService.AddTransacaoAsync(trasacaoUser);
                    return await AddAsync(saque);
                }

                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<SaqueDto> ConfirmarSaquePix(SaqueDto saque)
        {
            try
            {
               
                if (saque.Status == "ERROR")
                {
                    var user = await _userService.GetByIdAsync(saque.UserId);
                    var trasacaoUser = new Transacao()
                    {
                        CarteiraId = saque.User.Carteira.Id,
                        valor = saque.Valor,
                        TransactionId = saque.TransactionId,
                        Tipo = "Extorno Saque",
                        Data = DateTime.Now
                    };

                    var returnTransacao = await _carteiraService.AddTransacaoAsync(trasacaoUser);

                    saque.User.Carteira.SaldoAtual += saque.Valor;
                    saque.User.Carteira.DataAtualizacao = DateTime.Now;
                    await _userService.UpdateUserGame(saque.User);
                }

                var retorno = await UpdateAsync(saque);

                return retorno != null ? retorno : null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
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
