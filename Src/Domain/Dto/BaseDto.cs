using System.ComponentModel.DataAnnotations;
namespace Src.Domain.Dto
{
    public class BaseDto
    {
        [Required(ErrorMessage = "El id es obligatoria.")]
        public required int Id { get; set; }

        [Required(ErrorMessage = "La fecha de creación es obligatoria.")]
        public required DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }
    }
}
