using System.ComponentModel.DataAnnotations;

namespace Roleta.Aplicacao.Dtos.Identity
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? UserName { get; set; }
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [EmailAddress(ErrorMessage = "O {0} está inválido")]
        public string Email { get; set; }
        public string? PhoneNumber { get; set; }

        //[RegularExpression(@"^(?=.*[a-z])(?=.*?[0-9]).{6,}$")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public string? Description { get; set; }
        public string? ImagemUrl { get; set; }
        public int FreeSpin { get; set; } = 0;
        public decimal SaldoDeposito { get; set; } = 0;
        public decimal SaldoSaque { get; set; } = 0;
        public bool Verified { get; set; }
        public string? Token { get; set; }
    }
}
