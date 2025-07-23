using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Src.Infrastructure.Contexts;
using Src.Domain.Entity;
using Src.Domain.Dto;

namespace Src.Application.Service
{

    public class UserService
    {
        private readonly AppDbContext _context;
        private readonly PasswordService _passwordService;

        public UserService(
            AppDbContext context,
            PasswordService passwordService
        ) {
            _context = context;
            _passwordService = passwordService;
        }

        public async Task<bool> Signin(UserSigninDto data)
        {
            data.Password = _passwordService.HashPassword(data.Username, data.Password);
            data.Username = data.Username.ToLower();
            var user = new UserEntity
            {
                Username = data.Username,
                Password = data.Password,
            };

            _context.User.Add(user);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> Login(UserLoginDto data)
        {
            var userFound =  await _context.User
                .FirstOrDefaultAsync(u => u.Username == data.Username.ToLower());
            // .Where(p => p.Username == data.Username.ToLower()).FirstAsync();
            if(userFound == null) return false;

            var isCorrect = _passwordService.VerifyPassword(data.Username, userFound.Password, data.Password);
            if(isCorrect == false) return false;
            return true;
        }
    }
}
