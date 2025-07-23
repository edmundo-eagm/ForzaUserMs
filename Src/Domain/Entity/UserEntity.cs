using Microsoft.EntityFrameworkCore;

namespace Src.Domain.Entity
{
    [Index(nameof(Username), IsUnique = true)]
    public class UserEntity : BaseEntity
    {
        public required string Username { get; set; }
        public required string Password { get; set; }
    }
}
