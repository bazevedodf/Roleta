using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using Roleta.Aplicacao.Dtos;
using Roleta.Aplicacao.Extensions;
using Roleta.Aplicacao.Interface;
using Roleta.Dominio;

namespace Roleta.Api.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentController : ControllerBase
    {
        //private readonly IAccountService _accountService;
        private readonly IUserService _userService;
        private readonly IPagamentoService _pagamentoService;
        private readonly ISaqueService _saqueService;
        private readonly IEzzePayService _ezzePayService;
        private readonly IEmailService _emailService;
        private readonly IMapper _mapper;
        

        public PaymentController(IUserService userService,
                                 IPagamentoService pagamentoService,
                                 ISaqueService saqueService,
                                 IEzzePayService ezzePayService,
                                 IEmailService emailService,
                                 IMapper mapper)
        {
            _userService = userService;
            _pagamentoService = pagamentoService;
            _saqueService = saqueService;
            _ezzePayService = ezzePayService;
            _emailService = emailService;
            _mapper = mapper;
            
        }

        [EnableCors("BetBrazil")]
        [HttpPost("GetDepositPix")]
        public async Task<IActionResult> GerarQRCodePix(DepositoDto deposito)
        {
            try
            {
                var userId = Guid.Parse(User.GetUserId());
                //var user = await _accountService.GetByUserLoginAsync(userName);
                //if (user == null)
                //    return BadRequest("Você não possue permissão para essa operação");

                if (deposito.Valor < 10) return BadRequest("Não foi possivel gerar o pix!");

                DadosPixDto dados = new DadosPixDto()
                {
                    Amount = deposito.Valor,
                    PayerQuestion = "Roleta Pro",
                    PayerName = deposito.Nome,
                    PayerDocument = deposito.CPF
                };
                var retornoPix = await _ezzePayService.GetPixAsync(dados);
                if (retornoPix.Status == "error") return BadRequest("Não foi possivel gerar o código Pix");

                PagamentoDto pagamento = new PagamentoDto()
                {
                    Nome = deposito.Nome,
                    CPF = deposito.CPF,
                    TransactionId = retornoPix.TransactionId,
                    QrCode = retornoPix.QRCode, //@"data:image/jpeg;base64, iVBORw0KGgoAAAANSUhEUgAAAjoAAAI6AQMAAAAUhnCDAAAABlBMVEX///8AAABVwtN+AAAACXBIWXMAAA7EAAAOxAGVKw4bAAAEKklEQVR4nO2aQY6lMAxEI/0DcCSuniNxACT3ELucAD1SL2Ykt/S8oIEkL72gkrLzWyMIgiAIgiAI4v+ERZyt7cfHWtvMxp31q7W36268+9PF7LjebRrUAQEqC/KH7eo1aFf/no/tAo3hdk2zj+HbeRsLCFBJ0BZt8dVvQywDPkTwCcVMbk4DCNBvAI1eQyxxabH4q+GCHdEFEKBfA7KIATKb7kVwk48BBOg3gLJt3O12Ll3Dv5tJRU8BAQJUFZTCcJ386KJBgACVBa1IZZzjborF73aN/MtwQIBKgeTBzX25d7UcmTEeo59MDyBAhUE+clHHR4NG5dCyJv7mvv0RIECFQKoN7s4I4yI5PHxMzBX+HRCgqqAY7j7GYlGPSrjU0dUv/M4hUXVAgKqC4tP/QQJ6dXax5NlmAwSoLMh/YiKvHueYehdy8DijYXU+gADVBUXuqQ5nLO8OmtVxDV/E0gEBKgsyaSK8zSGuW/LpciQWHQi9qzWAABUC9ZYSMbPb4j8bTk2oyqFHBwSoLGgMT2Ss9q4YXVo27F5feTAAASoI6lkltCyeeAVlOxeJzCXfH2f1BRCgoiA5dK+bbPNIXjtA1s69Ju4Xe1XYAQEqBJrlcC3+wVimaUnLhrdEAAGqBWp3hqWZmUnpUn0ZXexchwECVBEk95IMex3Oa2uY28DF2B4SAQSoFGiu4stIL7Jssum9TTNjq9UBBKgsSIw144wa4tTJdOj+7kzFAAJUFKSDS0lktO1meY4Z1cRl8R+dQ0qAAJUF2SwLTtC0NSrBzIMe7/dy/oAAFQOFj2nP0Bp/U1FkoSEgQIDKghZz7tysoGSyOeZKKc3aOSBAdUHKM9OzpDCaDnpWsVi4HP1QBRCgqiAvfdstu1x6eWaac+lMX/8EIEBFQeo1u2pkFF6ubaDFkY+y0G93EUCAioGuq3twM4E0wxiZyFCHmx57pqKAAFUCXbFKpOWJ5khPlXb6ur8fepc7ACBAJUHLkt/01Q9u2HQLOXjnrou3dkCA6oLcrnxiPfcsVLmnZWE8Ki3KR5WtAgJUFBTJZtLsuF36DamS4urfAQGqCdqPpg9+CzPjj7t2AKnjc3foz0MiQIDKgWTJzyZhhCWfI8PqtDmDfZfTAgJUBmQSRl9yz5l2BvwmpW9tDSBAlUCWx5VRSPRI/z6nWQyOogMCVBW0IL3SMsLrK5phhG8Shyotcy8ABKgiSF97rudbGJzF6vTk7kMY+a4DAlQW5A+uhE/+ZCrKKLPB1/0cvuoJEKCSoM10OC+nonU/tKMlP1b70NSz5gIIUFFQfPrLZT3RXB2NmanWCAhQeZCv9lFL0aFO3EX4iDgQyo0DEKCSoPFHSpgS0RnPRMauEO9eqSggQKVAEedyFzY9k9Lx6Iv/lp1fEgEEqBKIIAiCIAiCIIh/GV8kQUs8NzKVTQAAAABJRU5ErkJggg==",
                    QrCodeText = retornoPix.QRCodeText, //@"00020101021226790014br.gov.bcb.pix2557brcode.starkinfra.com/v2/2bc98df7201643f8a203d9615e5ab17b5204000053039865802BR5925Ezzepay Solucoes de Pagam6008Brasilia62070503***6304CA83",
                    Valor = deposito.Valor,
                    Status = retornoPix.Status,
                    DataStatus = DateTime.Now,
                    DataCadastro = DateTime.Now,
                    UserId = userId,
                };

                var retornoPg = await _pagamentoService.AddAsync(pagamento);
                if (retornoPg == null) return BadRequest("Erro ao solicitar o Pix");

                //Status dos QRCodes
                //PENDING - APPROVED - EXPIRED - RETURNED
                return Ok(new
                {
                    status = "PENDING",
                    value = retornoPg.Valor,
                    qrCode = retornoPg.QrCode,
                    qrCodeText = retornoPg.QrCodeText,
                });

            }
            catch (Exception ex)
            {
                return this.StatusCode(
                    StatusCodes.Status500InternalServerError,
                    $"Ocorreu um erro ao gerar o QR Code: {ex.Message}");
            }
        }

        [EnableCors("AcessoLivre")]
        [HttpGet("ConsultaQRCode")]
        public async Task<IActionResult> ConsultaQRCode(string transactionId)
        {
            try
            {
                //var userDto = await _accountService.GetByUserLoginAsync(User.GetUserName());
                var userDto = await _userService.GetByUserLoginAsync(User.GetUserName());
                if (userDto == null) return Unauthorized("Usuário inválido");

                var pagamento = await _pagamentoService.ConsultaDepositoPix(transactionId);
                if (pagamento == null) return BadRequest("Transação não encontrada");

                return Ok(pagamento);
            }
            catch (Exception ex)
            {
                return this.StatusCode(
                    StatusCodes.Status500InternalServerError,
                    $"Ocorreu um erro ao consultar pagamento: {ex.Message}");
            }
        }

        [AllowAnonymous]
        [EnableCors("AcessoLivre")]
        [HttpPost("Status")]
        public async Task<IActionResult> PaymentReceiveWebhook()
        {
            if (Request.Headers.TryGetValue("Verify-Signature", out StringValues headerValue))
            {
                string[] headerParts = headerValue.ToString().Split(',');
                string reqTimestamp = "";
                string reqSignature = "";

                foreach (string part in headerParts)
                {
                    if (part.StartsWith("t=")) reqTimestamp = part.Replace("t=", "");

                    if (part.StartsWith("vsign=")) reqSignature = part.Replace("vsign=", "");
                }

                if (!string.IsNullOrWhiteSpace(reqTimestamp) && !string.IsNullOrWhiteSpace(reqSignature))
                {
                    using (StreamReader reader = new StreamReader(Request.Body))
                    {
                        string requestPayload = await reader.ReadToEndAsync();
                        if (_ezzePayService.ValidaAssinaturaWebHook(reqTimestamp, requestPayload, reqSignature))
                        {
                            var json = JsonConvert.DeserializeObject<dynamic>(requestPayload);
                            if (json != null)
                            {
                                string transactionType = json.requestBody.transactionType;
                                string transactionId = json.requestBody.transactionId;
                                
                                //Depositos via PIX
                                if (transactionType == "EXPIRED" || transactionType == "RECEIVEPIX")
                                {
                                    //Status dos QRCodes
                                    //PENDING - APPROVED - EXPIRED - RETURNED - ERROR
                                    var pagamento = await _pagamentoService.GetByTransactionIdAsync(transactionId);
                                    if (pagamento != null)
                                    {
                                        if (transactionType == "EXPIRED")
                                        {
                                            pagamento.Status = "EXPIRED";
                                            pagamento.DataStatus = DateTime.Now;
                                            var retorno = await _pagamentoService.UpdateAsync(pagamento);
                                        }
                                        if (transactionType == "RECEIVEPIX")
                                        {
                                            pagamento.Status = "APPROVED";
                                            pagamento.DataStatus = DateTime.Now;
                                            var retorno = await _pagamentoService.ConfirmarDepositoPix(pagamento);
                                            if (retorno != null)
                                            {
                                                await _emailService.ConfirmarPagamento(retorno.User, retorno);
                                            }
                                        }
                                    }
                                }

                                //Saques via PIX
                                if (transactionType == "PAYMENT_CANCELLED" || transactionType == "PAYMENT")
                                {
                                    var saque = await _saqueService.GetByTransactionIdAsync(transactionId);
                                    if (saque != null)
                                    {
                                        //Status dos Saques
                                        //PROCESSING - PEND_CONFIRM - CONFIRMED - ERROR - CANCELED - RETURNED
                                        if (transactionType == "PAYMENT_CANCELLED")
                                        {
                                            saque.Status = "PAYMENT_CANCELLED";
                                            var retorno = await _saqueService.UpdateAsync(saque);
                                        }

                                        if (transactionType == "PAYMENT")
                                        {
                                            string statusId = json.requestBody.statusCode.statusId;
                                            if (statusId == "3")
                                            {
                                                saque.Status = "ERROR";
                                                saque.DataStatus = DateTime.Now;
                                            }

                                            if (statusId == "2")
                                            {
                                                saque.Status = "PAYMENT";
                                                saque.DataStatus = DateTime.Now;
                                            }

                                            var retorno = await _saqueService.ConfirmarSaquePix(saque);
                                        }
                                    }                                    
                                }
                            }

                            return Ok(new
                            {
                                reqTimestamp,
                                reqSignature,
                                valid = true
                            });
                        }
                    }
                }

            }
            return Ok();
        }
    }
}
