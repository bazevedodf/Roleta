using AutoMapper;
using Roleta.Aplicacao.Dtos;
using Roleta.Aplicacao.Dtos.Identity;
using Roleta.Aplicacao.Interface;
using Roleta.Dominio;
using Roleta.Persistencia;
using Roleta.Persistencia.Interface;
using Roleta.Persistencia.Models;
using System.Text;

namespace Roleta.Aplicacao
{
    public class PagamentoService : IPagamentoService
    {
        private readonly IPagamentoPersist _pagamentoPersist;
        private readonly IAccountService _accountService;
        private readonly IEmailService _emailService;
        private readonly IEzzePayService _ezzePayService;
        private readonly IMapper _mapper;

        public PagamentoService(IPagamentoPersist pagamentoPersist,
                                IAccountService accountService,
                                IEzzePayService ezzePayService,
                                IEmailService emailService,
                                IMapper mapper)
        {
            _pagamentoPersist = pagamentoPersist;
            _accountService = accountService;
            _ezzePayService = ezzePayService;
            _emailService = emailService;
            _mapper = mapper;
        }
        
        public async Task<PagamentoDto> ConfirmarPagamento(string transactionId)
        {
            try
            {
                //Status dos QRCodes
                //PENDING - APPROVED - EXPIRED - RETURNED - ERROR
                var pagamento = await _pagamentoPersist.GetByTransactionIdAsync(transactionId, true);
                if (pagamento != null)
                {
                    if (pagamento.Status == "PENDING")
                    {
                        var retornoPix = await _ezzePayService.ConsultaPixAsync(transactionId);
                        if (retornoPix == null) return null;

                        if (retornoPix.Status == pagamento.Status)
                            return _mapper.Map<PagamentoDto>(pagamento);

                        pagamento.Status = retornoPix.Status;
                        pagamento.DataStatus = DateTime.Now;
                        if (await IsUpdateAsync(pagamento))
                        {
                            var retorno = _mapper.Map<PagamentoDto>(pagamento);
                            if (retornoPix.Status == "APPROVED")
                            {
                                var user = await _accountService.GetByIdAsync(pagamento.UserId);
                                if (user != null)
                                {
                                    user.SaldoDeposito += pagamento.Produto.SaldoDeposito;
                                    await _emailService.ConfirmarPagamento(user, retorno);
                                }                                
                            }

                            return retorno;
                        }
                    }

                    return _mapper.Map<PagamentoDto>(pagamento);
                }

                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public async Task<PagamentoDto> AddAsync(PagamentoDto model)
        {
            try
            {
                var pagamento = _mapper.Map<Pagamento>(model);

                _pagamentoPersist.Add(pagamento);
                if (await _pagamentoPersist.SaveChangeAsync())
                {
                    var retorno = await _pagamentoPersist.GetByIdAsync(pagamento.Id);
                    return _mapper.Map<PagamentoDto>(retorno);
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> IsUpdateAsync(Pagamento pagamento)
        {
            try
            {
                if (pagamento == null) return false;
                _pagamentoPersist.Update(pagamento);
                if (await _pagamentoPersist.SaveChangeAsync())
                {
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<PagamentoDto> UpdateAsync(PagamentoDto model)
        {
            try
            {
                var pagamento = await _pagamentoPersist.GetByIdAsync(model.Id);
                if (pagamento == null) return null;

                _mapper.Map(model, pagamento);
                _pagamentoPersist.Update(pagamento);
                if (await _pagamentoPersist.SaveChangeAsync())
                {
                    var retorno = await _pagamentoPersist.GetByIdAsync(pagamento.Id);
                    return _mapper.Map<PagamentoDto>(retorno);
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
                var pagamento = await _pagamentoPersist.GetByIdAsync(id);
                if (pagamento == null) throw new Exception("Pagamento não encontrado");

                _pagamentoPersist.Delete(pagamento);
                return await _pagamentoPersist.SaveChangeAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<PagamentoDto> GetByIdAsync(int id)
        {
            try
            {
                var pagamento = await _pagamentoPersist.GetByIdAsync(id);
                if (pagamento == null) return null;

                return _mapper.Map<PagamentoDto>(pagamento);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<PagamentoDto[]> GetAllByUserIdAsync(Guid userId)
        {
            try
            {
                var pagamentos = await _pagamentoPersist.GetAllByUserIdAsync(userId);
                if (pagamentos.Length <= 0) return null;

                return _mapper.Map<PagamentoDto[]>(pagamentos);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<PagamentoDto[]> GetAllByStatusAsync(string status)
        {
            try
            {
                var pagamentos = await _pagamentoPersist.GetAllByStatusAsync(status);
                if (pagamentos.Length <= 0) return null;

                return _mapper.Map<PagamentoDto[]>(pagamentos);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<int> GetAllAproveByParentEmailAsync(string? parentEmail = null)
        {
            try
            {
                return await _pagamentoPersist.GetAllAproveByParentEmailAsync(parentEmail);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }        

        public async Task<PagamentoDto> GetByTransactionIdAsync(string transactionId, bool includeProduto = false)
        {
            try
            {
                var pagamento = await _pagamentoPersist.GetByTransactionIdAsync(transactionId, includeProduto);
                if (pagamento == null) return null;

                return _mapper.Map<PagamentoDto>(pagamento);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<PageList<PagamentoDto>> GetAllByParentEmailAsync(DateTime dataIni, DateTime dataFim, PageParams pageParams)
        {
            try
            {
                var users = await _pagamentoPersist.GetAllByParentEmailAsync(dataIni, dataFim, pageParams);

                if (users == null) return null;

                var resultado = _mapper.Map<PageList<PagamentoDto>>(users);

                resultado.CurrentPage = users.CurrentPage;
                resultado.TotalPages = users.TotalPages;
                resultado.PageSize = users.PageSize;
                resultado.TotalCount = users.TotalCount;

                return resultado;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
