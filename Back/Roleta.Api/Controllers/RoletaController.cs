using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Roleta.Aplicacao.Extensions;
using Roleta.Aplicacao.Interface;

namespace Roleta.Api.Controllers
{
    [ApiController]
    [EnableCors("BetBrazil")]
    [Route("api/[controller]")]
    public class RoletaController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IRoletaService _roletaService;
        private readonly ISaqueService _saqueService;
        private readonly IMapper _mapper;

        public RoletaController(IUserService userService, 
                                IRoletaService roletaService,
                                ISaqueService saqueService,
                                IMapper mapper)
        {
            _userService = userService;
            _roletaService = roletaService;
            _saqueService = saqueService;
            _mapper = mapper;
        }

        [HttpGet("SpinBet")]
        public async Task<IActionResult> SpinBet(int valorAposta, bool freeSpin = false)
        {
            try
            {
                var login = User.GetUserName();
                if (login == null) return Unauthorized("Usuário inválido");

                var user = await _userService.GetUserGameByLoginAsync(login);
                if (user == null) return Unauthorized("Usuário inválido");

                if (user.isBlocked) return Unauthorized("Usuário Bloqueado, procure o suporte!");

                if (freeSpin)
                {
                    var giro = await _roletaService.GirarRoleta(valorAposta, freeSpin);

                    return Ok(giro);
                }
                else
                {
                    if (user.DemoAcount)
                    {
                        if (user.Carteira.SaldoDemo == 0)
                            return this.StatusCode(StatusCodes.Status406NotAcceptable, $"Você não possue saldo para jogar!");

                        if (valorAposta > user.Carteira.SaldoDemo)
                            return this.StatusCode(StatusCodes.Status406NotAcceptable, $"Sua aposta não pode ser superior ao saldo de jogo!");
                    }
                    else
                    {
                        if (user.Carteira.SaldoAtual == 0)
                            return this.StatusCode(StatusCodes.Status406NotAcceptable, $"Você não possue saldo para jogar!");

                        if (valorAposta > user.Carteira.SaldoAtual)
                            return this.StatusCode(StatusCodes.Status406NotAcceptable, $"Sua aposta não pode ser superior ao saldo de jogo!");
                    }
                    
                    var giro = await _roletaService.GirarRoleta(valorAposta, freeSpin, user);

                    return Ok(giro);
                }
            }
            catch (Exception ex)
            {
                return this.StatusCode(
                    StatusCodes.Status500InternalServerError,
                    $"Ocorreu um erro ao processar o Giro");
            }
        }

        [Authorize]
        [HttpGet("Saque")]
        public async Task<IActionResult> Saque(decimal valor)
        {
            try
            {
                var login = User.GetUserName();
                if (login == null) return Unauthorized("Usuário inválido");

                var user = await _userService.GetUserGameByLoginAsync(login);
                if (user == null) return Unauthorized("Usuário inválido");

                if (user.DemoAcount) return Unauthorized("Não é possivel fazer saques nessa conta!");

                if (user.isAfiliate) return Unauthorized("Não é possivel fazer saques nessa conta!");

                var roleta = await _roletaService.GetByIdAsync(1, true);

                if (valor < roleta.ValorMinimoSaque)
                    return this.StatusCode(StatusCodes.Status403Forbidden, 
                            $"O valor mínimo de saque é de {roleta.ValorMinimoSaque.ToString("C")}.");

                if (valor > roleta.ValorMaximoSaque)
                    return this.StatusCode(StatusCodes.Status403Forbidden,
                            $"O valor diário máximo para saque é de {roleta.ValorMaximoSaque.ToString("C")}.");

                if (user.Carteira.SaldoAtual < valor) 
                    return this.StatusCode(StatusCodes.Status403Forbidden,
                                $"Saldo insuficiente.");

                if (string.IsNullOrEmpty(user.TipoChavePix) || string.IsNullOrEmpty(user.ChavePix))
                {
                    return this.StatusCode(StatusCodes.Status403Forbidden,
                                $"Chave Pix Inválida.");
                }
                
                var retorno = await _saqueService.SolicitarSaquePix(user, valor, roleta.TaxaSaque, "Saque Game");
                if (retorno == null) return BadRequest("Erro ao solicitar o Saque");

                return Ok(retorno);
            }
            catch (Exception ex)
            {
                return this.StatusCode(
                    StatusCodes.Status500InternalServerError,
                    $"Ocorreu um erro ao processar o saque: {ex.Message}");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetValorSaque()
        {
            try
            {
                var roleta = await _roletaService.GetByIdAsync(1);
                if (roleta == null) return NoContent();

                return Ok(new { 
                    valorMinimoSaque = roleta.ValorMinimoSaque, 
                    valorMaximoSaque = roleta.ValorMaximoSaque 
                });
            }
            catch (Exception ex)
            {
                return this.StatusCode(
                    StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar recuperar dados da Roleta. Erro: {ex.Message}");
            }
        }

    }
}
