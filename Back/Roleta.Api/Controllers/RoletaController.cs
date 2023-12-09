using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Roleta.Aplicacao;
using Roleta.Aplicacao.Dtos;
using Roleta.Aplicacao.Extensions;
using Roleta.Aplicacao.Interface;
using Roleta.Dominio;

namespace Roleta.Api.Controllers
{
    [ApiController]
    [EnableCors("BetBrazil")]
    [Route("api/[controller]")]
    public class RoletaController : ControllerBase
    {
        private readonly IProdutoService _produtoService;
        private readonly IUserService _userService;
        private readonly IRoletaService _roletaService;
        private readonly ISaqueService _saqueService;
        private readonly IMapper _mapper;

        public RoletaController(IProdutoService produtoService, 
                                IUserService userService, 
                                IRoletaService roletaService,
                                ISaqueService saqueService,
                                IMapper mapper)
        {
            _produtoService = produtoService;
            _userService = userService;
            _roletaService = roletaService;
            _saqueService = saqueService;
            _mapper = mapper;
        }

        //[HttpGet("ItensRoleta")]
        //public async Task<IActionResult> GetItensRoleta()
        //{
        //    try
        //    {
        //        List<ItemRoletaDto> itens = new List<ItemRoletaDto>() {
        //            new ItemRoletaDto { Id = 1, Text = "0x", FillStyle = "#003399", TextFillStyle = "white", TextFontSize = "22" },
        //            new ItemRoletaDto { Id = 2, Text = "0.5x", FillStyle = "#006633", TextFillStyle = "white", TextFontSize = "22" },
        //            new ItemRoletaDto { Id = 3, Text = "1.2x", FillStyle = "#ffcc00", TextFillStyle = "white", TextFontSize = "22" },
        //            new ItemRoletaDto { Id = 4, Text = "3x", FillStyle = "#ff6600", TextFillStyle = "white", TextFontSize = "22" },
        //            new ItemRoletaDto { Id = 5, Text = "0x", FillStyle = "#003399", TextFillStyle = "white", TextFontSize = "22" },
        //            new ItemRoletaDto { Id = 6, Text = "0.5x", FillStyle = "#006633", TextFillStyle = "white", TextFontSize = "22" },
        //            new ItemRoletaDto { Id = 7, Text = "1.2x", FillStyle = "#ffcc00", TextFillStyle = "white", TextFontSize = "22" },
        //            new ItemRoletaDto { Id = 8, Text = "5x", FillStyle = "#ff6600", TextFillStyle = "white", TextFontSize = "22" },
        //            new ItemRoletaDto { Id = 9, Text = "100x", FillStyle = "#fff", TextFillStyle = "black", TextFontSize = "22" },
        //            new ItemRoletaDto { Id = 10, Text = "0x", FillStyle = "#003399", TextFillStyle = "white", TextFontSize = "22" },
        //            new ItemRoletaDto { Id = 11, Text = "0.5x", FillStyle = "#006633", TextFillStyle = "white", TextFontSize = "22" },
        //            new ItemRoletaDto { Id = 12, Text = "1.2x", FillStyle = "#ffcc00", TextFillStyle = "white", TextFontSize = "22" },
        //            new ItemRoletaDto { Id = 13, Text = "7x", FillStyle = "#ff6600", TextFillStyle = "white", TextFontSize = "22" },
        //            new ItemRoletaDto { Id = 14, Text = "0x", FillStyle = "#003399", TextFillStyle = "white", TextFontSize = "22" },
        //            new ItemRoletaDto { Id = 15, Text = "0.5x", FillStyle = "#006633", TextFillStyle = "white", TextFontSize = "22" },
        //            new ItemRoletaDto { Id = 16, Text = "1.2x", FillStyle = "#ffcc00", TextFillStyle = "white", TextFontSize = "22" },
        //            new ItemRoletaDto { Id = 17, Text = "9x", FillStyle = "#ff6600", TextFillStyle = "white", TextFontSize = "22" },
        //            new ItemRoletaDto { Id = 18, Text = "20x", FillStyle = "#fff", TextFillStyle = "black", TextFontSize = "22" }
        //        };
                

        //        return Ok(itens);
        //    }
        //    catch (Exception ex)
        //    {
        //        return this.StatusCode(
        //            StatusCodes.Status500InternalServerError,
        //            $"Erro ao tentar recuperar conta de usuário. Erro: {ex.Message}");
        //    }
        //}

        [HttpGet("SpinBet")]
        public async Task<IActionResult> SpinBet(int valorAposta, bool freeSpin = false)
        {
            try
            {
                if (freeSpin)
                {
                    var giro = await _roletaService.GirarRoleta(valorAposta, freeSpin);

                    return Ok(giro);
                }
                else
                {
                    var login = User.GetUserName();
                    if (login == null) return Unauthorized("Usuário inválido");
                    
                    var user = await _userService.GetUserGameByLoginAsync(login);
                    if (user == null) return Unauthorized("Usuário inválido");
                    
                    if (user.isBlocked) return Unauthorized("Usuário Bloqueado, procure o suporte!");

                    if (user.Carteira.SaldoAtual == 0)
                        return this.StatusCode(StatusCodes.Status406NotAcceptable, $"Você não possue saldo para jogar!");

                    if (valorAposta > user.Carteira.SaldoAtual)
                        return this.StatusCode(StatusCodes.Status406NotAcceptable, $"Sua aposta não pode ser superior ao saldo de jogo!");

                    var giro = await _roletaService.GirarRoleta(valorAposta, freeSpin, user);

                    //user.SaldoDeposito -= giro.ValorAposta;
                    //user.SaldoSaque += giro.ValorAposta * giro.Multiplicador;

                    //var retorno = await _accountService.UpdateUserAsync(user);

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

                var roleta = await _roletaService.GetByIdAsync(1);

                if (valor < roleta.ValorSaque)
                    return this.StatusCode(StatusCodes.Status403Forbidden, 
                            $"O valor mínimo de saque é de {roleta.ValorSaque.ToString("C")}.");

                if (user.Carteira.SaldoAtual < valor) 
                    return this.StatusCode(StatusCodes.Status403Forbidden,
                                $"Saldo insuficiente.");

                var retorno = await _saqueService.SolicitarSaquePix(user, valor);

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

                return Ok(new { valorSaque = roleta.ValorSaque });
            }
            catch (Exception ex)
            {
                return this.StatusCode(
                    StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar recuperar dados da Roleta. Erro: {ex.Message}");
            }
        }

        //[HttpGet("GetOferta/{id:int}")]
        //public async Task<IActionResult> GetOferta(int id)
        //{
        //    try
        //    {
        //        var oferta = await _produtoService.GetByIdAsync(id);
        //        if (oferta == null) return NoContent();

        //        return Ok(oferta);
        //    }
        //    catch (Exception ex)
        //    {
        //        return this.StatusCode(
        //            StatusCodes.Status500InternalServerError,
        //            $"Erro ao tentar recuperar conta de usuário. Erro: {ex.Message}");
        //    }
        //}

    }
}
