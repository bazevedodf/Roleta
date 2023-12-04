using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Roleta.Aplicacao.Dtos.Identity;
using Roleta.Aplicacao.Extensions;
using Roleta.Aplicacao.Interface;
using Roleta.Dominio.Identity;
using System.Reflection;
using System.Text;

namespace Roleta.Api.Controllers
{
    [Authorize]
    [ApiController]
    [EnableCors("BetBrazil")]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly ITokenService _tokenService;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;

        public AccountController(IAccountService accountService, 
                                 ITokenService tokenService, 
                                 IEmailService emailService,
                                 IConfiguration configuration,
                                 IMapper mapper)
        {
            _accountService = accountService;
            _tokenService = tokenService;
            _emailService = emailService;
            _configuration = configuration;
            _mapper = mapper;
        }

        [HttpGet("GetUser")]
        public async Task<IActionResult> GetUser()
        {
            try
            {
                var userName = User.GetUserName();
                var user = await _accountService.GetByUserLoginAsync(userName);
                return Ok(user);
            }
            catch (Exception ex)
            {
                return this.StatusCode(
                    StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar recuperar conta de usuário. Erro: {ex.Message}");
            }
        }

        [HttpGet("GetUserGame")]
        public async Task<IActionResult> GetUserGame(bool includeDados = false)
        {
            try
            {
                var userName = User.GetUserName();
                var user = await _accountService.GetUserGameAsync(userName, includeDados);

                return Ok(user);
            }
            catch (Exception ex)
            {
                return this.StatusCode(
                    StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar recuperar conta de usuário. Erro: {ex.Message}");
            }
        }

        [AllowAnonymous]
        [HttpGet("UserExist/{userName}")]
        public async Task<IActionResult> GetUserExist(string userName)
        {
            try
            {
                var user = await _accountService.UserExists(userName);
                return Ok(user);
            }
            catch (Exception ex)
            {
                return this.StatusCode(
                    StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar recuperar conta de usuário. Erro: {ex.Message}");
            }
        }

        [AllowAnonymous]
        [HttpGet("EmailExist/{email}")]
        public async Task<IActionResult> GetEmailExist(string email)
        {
            try
            {
                var user = await _accountService.EmailExists(email);
                return Ok(user);
            }
            catch (Exception ex)
            {
                return this.StatusCode(
                    StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar recuperar conta de usuário. Erro: {ex.Message}");
            }
        }

        [AllowAnonymous]
        [HttpPost("Register")]
        public async Task<IActionResult> Register(UserDto userDto)
        {
            try
            {
                if (await _accountService.EmailExists(userDto.Email))
                    return BadRequest("Conta de usuário já existe");

                var user = await _accountService.CreateUserAsync(userDto);
                if (user != null)
                {
                    //string urlConfirm = Url.ActionLink("ConfirmEmail", "Account", new { userName = user.UserName, token = user.Token });
                    //string urlConfirm = GetUrlAction("user", "confirmation", new { userName = user.Email, token = user.Token });
                    user.Token = _tokenService.CreateToken(user).Result;
                    user.Password = "";

                    var body = new StringBuilder();
                    body.Append($"Olá,<br>");
                    body.Append($"<br>Seja bem vindo! <br>Segue seus dados de acesso:<br>");
                    body.Append($"Login/Email: <strong>{user.Email}</strong><br>");
                    body.Append($"Password: <strong>{userDto.Password}</strong><br>");
                    body.Append($"<p>Att,<br>");
                    body.Append($"Equipe BetBrazil</p>");
                    body.Append($"Boa sorte, estamos torcendo por você!</p>");
                    try
                    {
                        await _emailService.SendMail("BetBrazil - Suporte", user.FirstName, user.Email, "Sua conta foi criada com sucesso!", body.ToString());
                    }
                    catch (Exception)
                    {
                        //BadRequest("Erro ao tentar enviar o e-mail de confirmação, contate o suporte!");
                    }
                    
                    return Ok(user);
                }

                return BadRequest("Conta de usuário não foi cadastrado, tente novamente mais tarde.");
            }
            catch (Exception ex)
            {
                return this.StatusCode(
                    StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar criar conta de usuário. Erro: {ex.Message}");
            }
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            try
            {
                var user = await _accountService.GetByUserLoginAsync(loginDto.Login);
                if (user == null) return Unauthorized("Usuário ou E-mail inválido");

                var result = await _accountService.CheckPasswordAsync(user, loginDto.Password);

                if (result.IsNotAllowed) return Unauthorized("Confirme seu cadastro no e-mail que te enviamos!");

                if (result.IsLockedOut) return Unauthorized("Você está temporariamente bloqueado, tente em 5 min!");

                if (!result.Succeeded) return Unauthorized("Senha inválida");

                var retorno = _mapper.Map<UserDto>(user);
                retorno.Token = _tokenService.CreateToken(user).Result;
                return Ok(retorno);
            }
            catch (Exception ex)
            {
                return this.StatusCode(
                    StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar autenticar conta de usuário. Erro: {ex.Message}");
            }
        }

        [AllowAnonymous]
        [HttpPost("AfiliateLogin")]
        public async Task<IActionResult> AfiliateLogin(LoginDto loginDto)
        {
            try
            {
                var user = await _accountService.GetByUserLoginAsync(loginDto.Login, true);
                if (user == null) return Unauthorized("Usuário ou E-mail inválido");

                if (await _accountService.CheckRoleAsync(user, "USER")) 
                    return BadRequest("Você não tem permissão para logar!");

                var result = await _accountService.CheckPasswordAsync(user, loginDto.Password);

                if (result.IsNotAllowed) return Unauthorized("Confirme seu cadastro no e-mail que te enviamos!");

                if (result.IsLockedOut) return Unauthorized("Você está temporariamente bloqueado, tente em 5 min!");

                if (!result.Succeeded) return Unauthorized("Senha inválida");

                var retorno = _mapper.Map<UserDashBoardDto>(user);
                retorno.Token = _tokenService.CreateToken(user).Result;
                return Ok(retorno);
            }
            catch (Exception ex)
            {
                return this.StatusCode(
                    StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar autenticar conta de usuário. Erro: {ex.Message}");
            }
        }

        [AllowAnonymous]
        [HttpGet("ConfirmEmail")]
        public async Task<ActionResult> ConfirmEmail(string userName, string token)
        {
            var user = await _accountService.GetByUserLoginAsync(userName);
            if (user == null) return NoContent();

            var result = await _accountService.ConfirmEmailAsync(user, token.Replace(' ', '+'));
            
            if (result == null) 
                return Unauthorized($"Seu cadastro já foi confirmado anteriormente, já pode fazer seu login!");

            if (result.Succeeded)
            {
                var body = new StringBuilder();
                body.Append($"<p>Seja benvindo(a) a nossa comunidade,<br>");
                body.Append($"Você já pode acessar nosso portal, e usufluir de nossos recursos!</p>");
                body.Append($"");
                body.Append($"<p>Att,<br>");
                body.Append($"Equipe BetBrazil</p>");

                try
                {
                    await _emailService.SendMail("BetBrazil - Suporte", user.FirstName, user.Email, "Seu acesso está liberado!", body.ToString());
                }
                catch (Exception)
                {
                    BadRequest("Erro ao tentar enviar o e-mail de confirmação, contate o suporte!");
                }
                return Ok(new { message = "Seus dados foram confirmados, Seja bem vindo!"});
            }
            else
            {
                return NoContent();
            }
        }

        [AllowAnonymous]
        [HttpGet("RecoverPassword/{userLogin}")]
        public async Task<IActionResult> RecoverPassword(string userLogin)
        {
            try
            {
                //https://learn.microsoft.com/pt-br/aspnet/core/security/authentication/accconfirm?view=aspnetcore-6.0&tabs=visual-studio
                var user = await _accountService.GetByUserLoginAsync(userLogin);
                if (user == null) return NoContent();

                var token = await _accountService.RecoverPasswordAsync(user);
                if (token == null) return this.StatusCode(StatusCodes.Status401Unauthorized, $"Seu cadastro ainda não foi confirmado, acesse seu e-mail");

                //string urlConfirm = Url.ActionLink("ResetPassword", "Account", new { userLogin = user.UserName, token = token });
                string urlConfirm =  GetUrlAction("user", "resetpassword", new { userName = user.Email, token = token });

                var body = new StringBuilder();
                body.Append($"Olá {user.FirstName},<br>");
                body.Append($"<p>Você solicitou a recuepração de senha, para definir uma nova senha acesse o link: <strong><a href='{urlConfirm}'>alterar senha</a></strong></p>");
                body.Append($"Seus dados:<br>");
                body.Append($"Email/Email: <strong>{user.Email}</strong></p>");
                body.Append($"<p>Att,<br>");
                body.Append($"Equipe BetBrazil</p>");
                try
                {
                    await _emailService.SendMail("BetBrazil - Suporte", user.FirstName, user.Email, "Recuperação de senha", body.ToString());
                }
                catch (Exception)
                {
                    BadRequest("Erro ao tentar enviar o e-mail de confirmação, contate o suporte!");
                }
                return Ok(new { message = "Te enviamos um e-mail com link para alteração da sua senha!"});
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao registrar Usuário, Erro: {ex.Message}");
            }
        }

        [AllowAnonymous]
        [HttpGet("ResetPassword")]
        public async Task<IActionResult> ResetPassword(string userLogin, string token, string newPassword)
        {
            try
            {
                var user = await _accountService.GetByUserLoginAsync(userLogin);
                if (user == null) return NoContent();
                var result = await _accountService.ResetPasswordAsync(user, token.Replace(' ','+'), newPassword);

                if (result.Succeeded)
                {
                    var body = new StringBuilder();
                    body.Append($"Olá {user.FirstName},<br>");
                    body.Append($"<p>Você fez a alteração da sua senha de acesso,<br>");
                    body.Append($"sua nova senha é: <strong>{newPassword}</strong></p><br>");
                    body.Append($"<p>Att,<br>");
                    body.Append($"Equipe BetBrazil</p>");
                    try
                    {
                        await _emailService.SendMail("BetBrazil - Suporte", user.FirstName, user.Email, "Sua senha foi alterada", body.ToString());
                    }
                    catch (Exception)
                    {
                        return BadRequest("Erro ao tentar enviar o e-mail de confirmação, contate o suporte!");
                    }

                    var retorno = _mapper.Map<UserDto>(user);
                    retorno.Token = _tokenService.CreateToken(user).Result;
                    return Ok(retorno);
                }
                else
                {
                    var erro = result.Errors.FirstOrDefault();
                    return BadRequest($"Não foi possível alterar sua senha. Erro: {erro.Code}, {erro.Description}");
                }
            }
            catch (Exception ex)
            {
                return this.StatusCode(StatusCodes.Status500InternalServerError,
                    $"Erro ao registrar Usuário, Erro: {ex.Message}");
            }
        }

        [HttpPut("UpdateUser")]
        public async Task<IActionResult> UpdateUser(UserDto userDto)
        {
            try
            {
                var user = await _accountService.GetByUserLoginAsync(User.GetUserName());
                if (user == null) return Unauthorized("Você não tem permissão para editar esta conta de usuário");

                var retorno = await _accountService.UpdateUserAsync(userDto);
                if (retorno == null) return NoContent();
                    
                return Ok(retorno);
            }
            catch (Exception ex)
            {
                return this.StatusCode(
                    StatusCodes.Status500InternalServerError,
                    $"Erro ao tentar atualizar conta de usuário. Erro: {ex.Message}");
            }
        }

        [NonAction]
        public string GetUrlAction(string controller, string action, object parameters)
        {
            string parametros = "";
            
            Type objType = parameters.GetType();
            PropertyInfo[] properties = objType.GetProperties(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);
            
            foreach (var item in properties)
            {
                parametros += $"{item.Name}={item.GetValue(parameters)}&";
            }

            return $"{_configuration["UrlSite"]}{controller}/{action}?{parametros.Remove(parametros.Length - 1)}";
        }
    }
}
