using System.ComponentModel.DataAnnotations;
namespace Src.Domain.Dto
{
    public class TokenDto
    {
        [Required(ErrorMessage = "El token es obligatorio")]
        public required string Token { get; set; }
    }
}
