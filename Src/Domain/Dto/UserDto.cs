using System.ComponentModel.DataAnnotations;
namespace Src.Domain.Dto
{

    public class UserDto: BaseDto
    {
        [Required(ErrorMessage = "El nombre de usuario es obligatorio.")]
        public required string Username { get; set; }

        [Required(ErrorMessage = "La contraseña es obligatoria.")]
        public required string Password { get; set; }
    }
}
