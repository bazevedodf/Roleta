using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net;
using System.Text;

namespace Roleta.Aplicacao.Middleware
{
    public class AuthenticationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IPWhitelistOptions _iPWhitelistOptions;
        private readonly ILogger<AuthenticationMiddleware> _logger;

        public AuthenticationMiddleware(RequestDelegate next, 
                                        ILogger<AuthenticationMiddleware> logger ,
                                        IOptions<IPWhitelistOptions> applicationOptionsAccessor)
        {
            _next = next;
            _logger = logger;
            _iPWhitelistOptions = applicationOptionsAccessor.Value;
        }

        public async Task Invoke(HttpContext context)
        {
            if (ValidateIfIpIsInWhiteList(context))
            {
                //if (LoginUserBasicAuthentication(context))
                //{
                //    await _next.Invoke(context);
                //}
                //else
                //{
                //    context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                //    return;
                //}
                await _next.Invoke(context);
            }
            else
            {
                context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                return;
            }
        }

        private bool ValidateIfIpIsInWhiteList(HttpContext context)
        {
            var remoteIp = context.Connection.RemoteIpAddress;

            List<string> whiteListIPList = _iPWhitelistOptions.Whitelist;
            var isIPWhitelisted = whiteListIPList.Where(ip => IPAddress.Parse(ip)
                                                 .Equals(remoteIp))
                                                 .Any();
            if (!isIPWhitelisted)
            {
                _logger.LogWarning($"Solicitação de endereço IP remoto: {remoteIp} é proibida.", remoteIp);
                return false;
            }

            return true;
        }

        private bool LoginUserBasicAuthentication(HttpContext context)
        {
            string authHeader = context.Request.Headers["Authorization"];
            if (authHeader != null && authHeader.StartsWith("Basic"))
            {
                string encodedUsernamePassword = authHeader.Substring("Basic ".Length).Trim();

                Encoding encoding = Encoding.GetEncoding("iso-8859-1");
                string usernamePassword = encoding.GetString(Convert.FromBase64String(encodedUsernamePassword));

                int seperatorIndex = usernamePassword.IndexOf(':');

                var username = usernamePassword.Substring(0, seperatorIndex);
                var password = usernamePassword.Substring(seperatorIndex + 1);

                if (username == "admin" && password == "!alfa1020#")
                {
                    return true;
                }
            }

            return false;
        }
    }

    public static class AuthenticationMiddlewareExtension
    {
        /// <summary>
        /// Habilita o uso do Middleware de autenticação básica
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseIPWhitelist(this IApplicationBuilder app)
        {
            return app.UseWhen(x => (x.Request.Path.StartsWithSegments("/api", StringComparison.OrdinalIgnoreCase)),
                builder =>
                {
                    builder.UseMiddleware<AuthenticationMiddleware>();
                });
        }
    }

    public class IPWhitelistOptions
    {
        public List<string> Whitelist { get; set; }
    }
}
