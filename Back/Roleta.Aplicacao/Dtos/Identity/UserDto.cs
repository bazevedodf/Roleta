using System.ComponentModel.DataAnnotations;

namespace Roleta.Aplicacao.Dtos.Identity
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? UserName { get; set; }
        public string Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string Password { get; set; }
        public string? Description { get; set; }
        public string? ImagemUrl { get; set; }
        public bool Verified { get; set; }
        public bool DemoAcount { get; set; } = false;
        public string? ParentEmail { get; set; }
        public string? AfiliateCode { get; set; }
        public DateTime DataCadastro { get; set; } = DateTime.Now;
        public string? Token { get; set; }
    }
}
