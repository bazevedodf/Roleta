using System.ComponentModel.DataAnnotations;

namespace Roleta.Aplicacao.Dtos.Identity
{
    public class UserUpdateDto
    {
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [EmailAddress(ErrorMessage = "O {0} está inválido")]
        public string Email { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [StringLength(50, ErrorMessage = "Mínimo de 6 caracteres", MinimumLength = 6)]
        public string Password { get; set; }

        [Compare(nameof(Password), ErrorMessage = "As senhas devem ser iguais")]
        public string ConfirmPassword { get; set; }

    }
}
