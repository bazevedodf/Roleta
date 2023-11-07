using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Roleta.Aplicacao.Dtos;
using Roleta.Aplicacao.Extensions;
using Roleta.Aplicacao.Interface;

namespace Roleta.Api.Controllers
{
    [ApiController]
    [EnableCors("BetBrazil")]
    [Route("api/[controller]")]
    public class RoletaController : ControllerBase
    {
        private readonly IProdutoService _produtoService;
        private readonly IAccountService _accountService;
        private readonly IRoletaService _roletaService;
        private readonly ISaqueService _saqueService;
        private readonly IMapper _mapper;

        public RoletaController(IProdutoService produtoService, 
                                IAccountService accountService, 
                                IRoletaService roletaService,
                                ISaqueService saqueService,
                                IMapper mapper)
        {
            _produtoService = produtoService;
            _accountService = accountService;
            _roletaService = roletaService;
            _saqueService = saqueService;
            _mapper = mapper;
        }

        [HttpGet("ItensRoleta")]
        public async Task<IActionResult> GetItensRoleta()
        {
            try
            {
                List<ItemRoletaDto> itens = new List<ItemRoletaDto>() {
                    new ItemRoletaDto { Id = 1, Text = "0.5x", FillStyle = "#003399", TextFillStyle =  "white", TextFontSize =  "22" },
                    new ItemRoletaDto { Id = 2, Text = "1x", FillStyle = "#006633", TextFillStyle =  "white", TextFontSize =  "22" },
                    new ItemRoletaDto { Id = 3, Text = "1.2x", FillStyle = "#ffcc00", TextFillStyle =  "white", TextFontSize =  "22" },
                    new ItemRoletaDto { Id = 4, Text = "3x", FillStyle = "#ff6600", TextFillStyle =  "white", TextFontSize =  "22" },
                    new ItemRoletaDto { Id = 5, Text = "0.5", FillStyle = "#003399", TextFillStyle =  "white", TextFontSize =  "22" },
                    new ItemRoletaDto { Id = 6, Text = "1x", FillStyle = "#006633", TextFillStyle =  "white", TextFontSize =  "22"},
                    new ItemRoletaDto { Id = 7, Text = "1.2x", FillStyle = "#ffcc00", TextFillStyle =  "white", TextFontSize =  "22"},
                    new ItemRoletaDto { Id = 8, Text = "5x", FillStyle = "#ff6600", TextFillStyle =  "white", TextFontSize =  "22"},
                    new ItemRoletaDto { Id = 9, Text = "100x", FillStyle = "#fff", TextFillStyle =  "black", TextFontSize =  "22"},
                    new ItemRoletaDto { Id = 10, Text = "0.5", FillStyle = "#003399", TextFillStyle =  "white", TextFontSize =  "22"},
                    new ItemRoletaDto { Id = 11, Text = "1x", FillStyle = "#006633", TextFillStyle =  "white", TextFontSize =  "22"},
                    new ItemRoletaDto { Id = 12, Text = "1.2x", FillStyle = "#ffcc00", TextFillStyle =  "white", TextFontSize =  "22"},
                    new ItemRoletaDto { Id = 13, Text = "7x", FillStyle = "#ff6600", TextFillStyle =  "white", TextFontSize =  "22"},
                    new ItemRoletaDto { Id = 14, Text = "0.5x", FillStyle = "#003399", TextFillStyle =  "white", TextFontSize =  "22"},
                    new ItemRoletaDto { Id = 15, Text = "1x", FillStyle = "#006633", TextFillStyle =  "white", TextFontSize =  "22"},
                    new ItemRoletaDto { Id = 16, Text = "1.2x", FillStyle = "#ffcc00", TextFillStyle =  "white", TextFontSize =  "22"},
                    new ItemRoletaDto { Id = 17, Text = "9x", FillStyle = "#ff6600", TextFillStyle =  "white", TextFontSize =  "22"},
                    new ItemRoletaDto { Id = 18, Text = "30x", FillStyle = "#fff", TextFillStyle =  "black", TextFontSize =  "22"}
                };
                

                return Ok(itens);
            }
            catch (Exception ex)
            {
                return this.StatusCode(
                    StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar recuperar conta de usuário. Erro: {ex.Message}");
            }
        }

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
                    var user = await _accountService.GetByUserLoginAsync(login);
                    if (user == null) return Unauthorized("Usuário inválido");

                    if (user.SaldoDeposito == 0)
                        return this.StatusCode(StatusCodes.Status406NotAcceptable, $"Você não possue saldo para jogar!");
                    if (valorAposta > user.SaldoDeposito)
                        return this.StatusCode(StatusCodes.Status406NotAcceptable, $"Sua aposta não pode ser superior ao saldo de jogo!");

                    var giro = await _roletaService.GirarRoleta(valorAposta, freeSpin, user);

                    user.SaldoDeposito -= giro.ValorAposta;
                    user.SaldoSaque += giro.ValorAposta * giro.Multiplicador;

                    var retorno = await _accountService.UpdateUserAsync(user);

                    return Ok(giro);
                }
            }
            catch (Exception ex)
            {
                return this.StatusCode(
                    StatusCodes.Status500InternalServerError,
                    $"Ocorreu um erro ao processar o Giro: {ex.Message}");
            }
        }

        [Authorize]
        [HttpGet("SaldoReverse")]
        public async Task<IActionResult> SaldoReverse()
        {
            try
            {
                var login = User.GetUserName();
                var userDto = await _accountService.GetByUserLoginAsync(login);
                if (userDto == null) return Unauthorized("Usuário inválido");

                if (userDto.SaldoSaque > 0)
                {
                    userDto.SaldoDeposito += userDto.SaldoSaque;
                    var retorno = await _accountService.UpdateUserAsync(userDto);

                    return Ok(_mapper.Map<UserGameDto>(retorno));
                }

                return Ok(_mapper.Map<UserGameDto>(userDto));
            }
            catch (Exception ex)
            {
                return this.StatusCode(
                    StatusCodes.Status500InternalServerError,
                    $"Ocorreu um erro ao processar o Giro: {ex.Message}");
            }
        }

        [Authorize]
        [HttpGet("Saque")]
        public async Task<IActionResult> Saque(decimal valor)
        {
            try
            {
                if (valor < 100)
                    return this.StatusCode(StatusCodes.Status403Forbidden, $"O valor mínimo de saque é de R$ 100,00.");

                var login = User.GetUserName();
                var userDto = await _accountService.GetByUserLoginAsync(login);
                if (userDto == null) return Unauthorized("Usuário inválido");

                if (userDto.SaldoSaque < 100) 
                    return this.StatusCode(StatusCodes.Status403Forbidden,
                                $"O seu saldo de saque é inferior ao valor mínimo de R$ 100,00.");

                SaqueDto saque = new SaqueDto()
                {
                    UserId = userDto.Id,
                    Valor = valor,
                    Status = "Pendente"
                };

                var retorno = await _saqueService.AddAsync(saque);
                if (retorno == null) return BadRequest("Erro ao solicitar o Saque");

                userDto.SaldoSaque -= valor;
                var userRetorno = await _accountService.UpdateUserAsync(userDto);
                if (userRetorno == null)
                {
                    await _saqueService.DeleteAsync(retorno.Id);
                    return BadRequest("Erro ao atualizar saldo, para finalizar o saque.");
                }

                return Ok(retorno);
            }
            catch (Exception ex)
            {
                return this.StatusCode(
                    StatusCodes.Status500InternalServerError,
                    $"Ocorreu um erro ao processar o Giro: {ex.Message}");
            }
        }

        [HttpGet("GetOfertas")]
        public async Task<IActionResult> GetOfertas()
        {
            try
            {
                var ofertas = await _produtoService.GetAllAsync();
                if (ofertas.Length == 0) return NoContent();
                
                return Ok(ofertas);
            }
            catch (Exception ex)
            {
                return this.StatusCode(
                    StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar recuperar conta de usuário. Erro: {ex.Message}");
            }
        }

        [HttpGet("GetOferta/{id:int}")]
        public async Task<IActionResult> GetOferta(int id)
        {
            try
            {
                var oferta = await _produtoService.GetByIdAsync(id);
                if (oferta == null) return NoContent();

                return Ok(oferta);
            }
            catch (Exception ex)
            {
                return this.StatusCode(
                    StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar recuperar conta de usuário. Erro: {ex.Message}");
            }
        }

    }
}
