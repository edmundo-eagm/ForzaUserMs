using Microsoft.AspNetCore.Mvc;
using Src.Infrastructure.Contexts;
using Src.Domain.Entity;
using Src.Domain.Dto;

namespace Src.Application.Service
{

    public class UserService
    {
        private readonly AppDbContext _context;

        public UserService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<UserDto> Create(UserCreateDto data)
        {
            var user = new UserEntity
            {
                Username = data.Username,
                Password = data.Password,
            };

            _context.User.Add(user);
            await _context.SaveChangesAsync();

            return new UserDto
            {
                Id = user.Id,
                CreatedAt = user.CreatedAt,
                UpdatedAt = user.UpdatedAt,
                Username = user.Username,
                Password = user.Password,
            };
        }
    }
}
