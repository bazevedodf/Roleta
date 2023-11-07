using Roleta.Aplicacao.Dtos.Identity;
using Roleta.Aplicacao.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Roleta.Aplicacao.Interface
{
    public interface IEmailService
    {
        Task<bool> ConfirmarPagamento(UserDto userDto, PagamentoDto pagamentoDto);
        Task<bool> SendMail(string fromName,
                            string toName,
                            string toEmail,
                            string subject,
                            string body);
    }
}
