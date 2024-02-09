using AutoMapper;
using Roleta.Aplicacao.Dtos;
using Roleta.Aplicacao.Interface;
using Roleta.Dominio;
using Roleta.Persistencia.Interface;
using Roleta.Persistencia.Models;

namespace Roleta.Aplicacao
{
    public class PagamentoService : IPagamentoService
    {
        private readonly IPagamentoPersist _pagamentoPersist;
        private readonly IUserService _userService;
        private readonly IRoletaService _roletaService;
        private readonly ICarteiraService _carteiraService;
        private readonly IEmailService _emailService;
        private readonly IEzzePayService _ezzePayService;
        private readonly IMapper _mapper;

        public PagamentoService(IPagamentoPersist pagamentoPersist,
                                IUserService userService,
                                IRoletaService roletaService,
                                ICarteiraService carteiraService,
                                IEzzePayService ezzePayService,
                                IEmailService emailService,
                                IMapper mapper)
        {
            _pagamentoPersist = pagamentoPersist;
            _userService = userService;
            _roletaService = roletaService;
            _carteiraService = carteiraService;
            _ezzePayService = ezzePayService;
            _emailService = emailService;
            _mapper = mapper;
        }
        
        public async Task<PagamentoDto> ConsultaDepositoPix(string transactionId)
        {
            try
            {
                //Status dos QRCodes
                //PENDING - APPROVED - EXPIRED - RETURNED - ERROR
                var pagamento = await GetByTransactionIdAsync(transactionId);
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

                        var retorno = await ConfirmarDepositoPix(pagamento);
                        if (retorno != null)
                        {
                            await _emailService.ConfirmarPagamento(retorno.User, retorno);
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

        public async Task<PagamentoDto> ConfirmarDepositoPix(PagamentoDto pagamento)
        {
            try
            {
                PagamentoDto retorno = null;

                if (pagamento.Status != "APPROVED")
                {
                    pagamento.Status = "APPROVED";
                    var user = await _userService.GetByIdAsync(pagamento.UserId);

                    if (await _roletaService.DepositoCaixa(pagamento, 1))
                    {
                        if (!string.IsNullOrEmpty(user.ParentEmail))
                        {
                            await _roletaService.ComissaoAfiliado(user.ParentEmail, pagamento.Valor, pagamento.TransactionId);
                        }

                        user.Carteira.SaldoAtual += pagamento.Valor;
                        user.Carteira.DataAtualizacao = DateTime.Now;
                        var retornoUser = await _userService.UpdateUserGame(user);
                        if (retornoUser == null) return null;

                    }
                    retorno = await UpdateAsync(pagamento);
                }

                return retorno;
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

        public async Task<PagamentoDto> GetByTransactionIdAsync(string transactionId)
        {
            try
            {
                var pagamento = await _pagamentoPersist.GetByTransactionIdAsync(transactionId);
                if (pagamento == null) return null;

                return _mapper.Map<PagamentoDto>(pagamento);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<PagamentoDto[]> GetAllAfiliateAsync(PageParams pageParams, bool somentePagos = false)
        {
            try
            {
                var users = await _pagamentoPersist.GetAllByAfiliateAsync(pageParams, somentePagos);

                if (users == null) return null;

                var resultado = _mapper.Map<PagamentoDto[]>(users);

                return resultado;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<PageList<PagamentoDto>> GetAllByParentEmailAsync(PageParams pageParams, bool somentePagos = false)
        {
            try
            {
                var users = await _pagamentoPersist.GetAllByParentEmailAsync(pageParams, somentePagos);

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
