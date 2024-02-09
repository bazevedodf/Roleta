using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Roleta.Aplicacao;
using Roleta.Aplicacao.Dtos;
using Roleta.Aplicacao.Dtos.Identity;
using Roleta.Aplicacao.Extensions;
using Roleta.Aplicacao.Interface;
using Roleta.Dominio;
using Roleta.Persistencia.Models;

namespace Roleta.Api.Controllers
{
    [Authorize(Roles = "Admin,Afiliate")]
    [ApiController]
    [EnableCors("BetBrazil")]
    [Route("api/[controller]")]
    public class DashboardController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly IUserService _userService;
        private readonly IPagamentoService _pagamentoService;
        private readonly ISaqueService _saqueService;
        private readonly IMapper _mapper;

        public DashboardController(IAccountService accountService,
                                   IUserService userService,
                                   IPagamentoService pagamentoService,
                                   ISaqueService saqueService,
                                   IMapper mapper)
        {
            _accountService = accountService;
            _userService = userService;
            _pagamentoService = pagamentoService;
            _saqueService = saqueService;
            _mapper = mapper;
        }

        [HttpGet("GetDashData")]
        public async Task<IActionResult> GetDashData()
        {
            try
            {
                double totalCadastro = 0;
                double totalPagamentos = 0;
                
                var userDto = await _accountService.GetByUserLoginAsync(User.GetUserName());
                if (userDto == null) return Unauthorized();

                if (await _accountService.CheckRoleAsync(userDto, "Admin"))
                {
                    totalCadastro = await _userService.GetCountByParentEmail();
                    totalPagamentos = await _pagamentoService.GetAllAproveByParentEmailAsync();
                }
                else
                {
                    totalCadastro = await _userService.GetCountByParentEmail(userDto.Email);
                    totalPagamentos = await _pagamentoService.GetAllAproveByParentEmailAsync(userDto.Email);
                }
                double taxaConversao = (totalPagamentos / totalCadastro) * 100;
                return Ok(new
                {
                    cadastros = totalCadastro,
                    pagamentos = totalPagamentos,
                    conversao = $"{Math.Round(taxaConversao,2)}%",
                });
            }
            catch (Exception ex)
            {
                return this.StatusCode(
                    StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar recuperar dados. Erro: {ex.Message}");
            }
        }

        [HttpGet("GetUsers")]
        public async Task<IActionResult> GetUsers([FromQuery]PageParams pageParams)
        {
            try
            {
                PageList<UserDashBoardDto>? users;
                var userDto = await _accountService.GetByUserLoginAsync(User.GetUserName());
                if (userDto == null) return Unauthorized();

                if (!await _accountService.CheckRoleAsync(userDto, "Admin"))
                {
                    pageParams.ParentEmail = userDto.Email;
                }

                users = await _userService.GetAllByParentEmailDateAsync(pageParams);
                if (users == null) return NoContent();

                Response.AddPagination("PaginationUser", users.CurrentPage, users.PageSize, users.TotalCount, users.TotalPages);

                return Ok(users);

            }
            catch (Exception ex)
            {
                return this.StatusCode(
                    StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar buscar Usuário(s). Erro: {ex.Message}");
            }
        }

        [HttpGet("GetPagamentos")]
        public async Task<IActionResult> GetPagamentos([FromQuery] PageParams pageParams) //, DateTime dataIni, DateTime dataFim
        {
            try
            {
                var userDto = await _accountService.GetByUserLoginAsync(User.GetUserName());
                if (userDto == null) return Unauthorized();

                if (!await _accountService.CheckRoleAsync(userDto, "Admin"))
                {
                    pageParams.ParentEmail = userDto.Email;
                }

                var users = await _pagamentoService.GetAllByParentEmailAsync(pageParams, true);
                if (users == null) return NoContent();

                Response.AddPagination("PaginationPag", users.CurrentPage, users.PageSize, users.TotalCount, users.TotalPages);

                return Ok(users);

            }
            catch (Exception ex)
            {
                return this.StatusCode(
                    StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar recuperar dados. Erro: {ex.Message}");
            }
        }

        
        
        [Authorize(Roles = "Admin")]
        [HttpGet("GetUser")]
        public async Task<IActionResult> GetUserByEmail(string email)
        {
            try
            {
                var user = await _userService.GetByUserLoginAsync(email, true);
                if (user == null) return BadRequest();

                return Ok(user);
            }
            catch (Exception ex)
            {
                return this.StatusCode(
                    StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar recuperar usuário. Erro: {ex.Message}");
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("PutUser")]
        public async Task<IActionResult> PutUser(UserUpdateDashDto model)
        {
            try
            {
                var user = await _userService.UpdateUserDashBoard(model);
                if (user == null) return NoContent();

                return Ok(user);
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar atualizar Usuário. Erro: {ex.Message}");
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("GetAfiliates")]
        public async Task<IActionResult> GetAfiliates([FromQuery] PageParams pageParams, bool includeBlocks)
        {
            try
            {
                var userDto = await _accountService.GetByUserLoginAsync(User.GetUserName());
                if (userDto == null) return Unauthorized();

                if (!await _accountService.CheckRoleAsync(userDto, "Admin"))
                {
                    pageParams.ParentEmail = userDto.Email;
                }

                PageList<AfiliadoDto>? afiliados;
                afiliados = await _userService.GetAllAfiliatesAsync(pageParams, includeBlocks);
                if (afiliados == null) return NoContent();

                foreach(AfiliadoDto afiliado in afiliados)
                {
                    pageParams.ParentEmail = afiliado.Email;
                    var pagamentos = await _pagamentoService.GetAllAfiliateAsync(pageParams, true);
                    afiliado.TotalDepositos = pagamentos.Count();
                    afiliado.TotalFaturamento = pagamentos.Sum(x => x.Valor);
                }
                afiliados.OrderBy(a => a.TotalDepositos).ToList();
                Response.AddPagination("PaginationUser", afiliados.CurrentPage, afiliados.PageSize, afiliados.TotalCount, afiliados.TotalPages);

                return Ok(afiliados);

            }
            catch (Exception ex)
            {
                return this.StatusCode(
                    StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar buscar Usuário(s). Erro: {ex.Message}");
            }
        }

        [HttpGet("ChangeSaldoDemo")]
        public async Task<IActionResult> ChangeSaldoDemo(decimal valor)
        {
            try
            {
                var userDto = await _accountService.GetByUserLoginAsync(User.GetUserName());
                if (userDto == null) return Unauthorized();

                var retorno = await _userService.ChangeSaldoAfiliados(valor);

                return Ok(retorno);
            }
            catch (Exception ex)
            {
                return this.StatusCode(
                    StatusCodes.Status500InternalServerError,
                    $"Erro ao atualizar saldo demo dos afilados. Erro: {ex.Message}");
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("Saque")]
        public async Task<IActionResult> Saque(string email, bool isSaque = true, decimal valor = 0)
        {
            try
            {
                var userDto = await _accountService.GetByUserLoginAsync(User.GetUserName());
                if (userDto == null) return Unauthorized();

                var user = await _userService.GetUserGameByLoginAsync(email);

                if (string.IsNullOrEmpty(user.TipoChavePix) || string.IsNullOrEmpty(user.ChavePix))
                {
                    return BadRequest($"Chave Pix Inválida.");
                }

                if (string.IsNullOrEmpty(user.CPF))
                {
                    return BadRequest($"CPF não cadastrado!");
                }

                if (user.TipoChavePix == "TELEFONE" && user.ChavePix.IndexOf("+55") <= 0)
                {
                    return BadRequest($"Chave Pix Telefone sem +55.");
                }

                string tipoPix;

                if (isSaque)
                {
                    if(user.Carteira.SaldoAtual == 0 || user.Carteira.SaldoAtual <= valor)
                        return BadRequest("Saldo insuficiente");

                    valor = user.Carteira.SaldoAtual;
                    tipoPix = "Comissão AFL";
                }
                else
                {
                    tipoPix = "Adiantamento AFL";
                }


                var retorno = await _saqueService.SolicitarSaquePix(user, valor, 0, tipoPix);

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

    }
}
