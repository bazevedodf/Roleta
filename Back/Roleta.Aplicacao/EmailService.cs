using Roleta.Aplicacao.Dtos;
using Roleta.Aplicacao.Dtos.Identity;
using Roleta.Aplicacao.Interface;
using Roleta.Dominio.Identity;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace Roleta.Aplicacao
{
    public class EmailService : IEmailService
    {
        private readonly SmtpConfig smtpConfig;

        public EmailService(SmtpConfig smtpConfig)
        {
            this.smtpConfig = smtpConfig;
        }

        public async Task<bool> ConfirmarPagamento(UserDto userDto, PagamentoDto pagamentoDto)
        {
            var body = new StringBuilder();
            body.Append($"Olá, {userDto.FirstName}<br>");
            body.Append($"<br>Recebemos o seu pix, e seu saldo já está disponivel!<br>");
            body.Append($"<p>Att,<br>");
            body.Append($"Equipe BetBrazil</p>");
            body.Append($"Boa sorte, estamos torcendo por você!</p>");
            try
            {
                await SendMail("BetBrazil - Suporte", userDto.FirstName, userDto.Email, "Pix recebido!", body.ToString());
                return true;
            }
            catch (Exception)
            {
               
            }
            return false;
        }
        
        public async Task<bool> SendMail(string fromName, string toName, string toEmail, string subject, string body)
        {
            try
            {
                var smtpClient = new SmtpClient(smtpConfig.Host, smtpConfig.Port);
                smtpClient.Credentials = new NetworkCredential(smtpConfig.UserName, smtpConfig.Password);
                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtpClient.EnableSsl = true;

                var mail = new MailMessage();
                mail.From = new MailAddress(smtpConfig.Email, fromName);
                mail.To.Add(new MailAddress(toEmail, toName));
                mail.Subject = subject;
                mail.SubjectEncoding = Encoding.GetEncoding("ISO-8859-1");
                mail.Body = body;
                mail.BodyEncoding = Encoding.GetEncoding("ISO-8859-1");
                mail.IsBodyHtml = true;

                await smtpClient.SendMailAsync(mail);
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
