using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Roleta.Aplicacao.Dtos.Identity;
using Roleta.Aplicacao.Extensions;
using Roleta.Aplicacao.Interface;
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
        private readonly IMapper _mapper;

        public DashboardController(IAccountService accountService,
                                   IUserService userService,
                                   IPagamentoService pagamentoService,
                                   IMapper mapper)
        {
            _accountService = accountService;
            _userService = userService;
            _pagamentoService = pagamentoService;
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
                var userDto = await _accountService.GetByUserLoginAsync(User.GetUserName());
                if (userDto == null) return Unauthorized();

                if (!await _accountService.CheckRoleAsync(userDto, "Admin"))
                {
                    pageParams.ParentEmail = userDto.Email;
                }

                var users = await _userService.GetAllByParentEmailDateAsync(pageParams);
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
                    pageParams.Term = userDto.Email;
                }

                var users = await _pagamentoService.GetAllByParentEmailAsync(pageParams);
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
    }
}
