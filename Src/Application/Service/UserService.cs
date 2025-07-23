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
        private readonly JwtService _jwtService;

        public UserService(
            AppDbContext context,
            PasswordService passwordService,
            JwtService jwtService
        ) {
            _context = context;
            _passwordService = passwordService;
            _jwtService = jwtService;
        }

        public async Task<TokenDto> Signin(UserSigninDto data)
        {
            try{
                data.Password = _passwordService.HashPassword(data.Username, data.Password);
                data.Username = data.Username.ToLower();
                var user = new UserEntity
                {
                    Username = data.Username,
                    Password = data.Password,
                };

                _context.User.Add(user);
                await _context.SaveChangesAsync();

                var userFound =  await _context.User
                    .FirstOrDefaultAsync(u => u.Username == data.Username.ToLower());
                if(userFound == null) return null;

                var token = _jwtService.GenerateToken(userFound.Id.ToString());
                return new TokenDto
                {
                    Token = token
                };
            }
            // catch (DbUpdateException ex) when (ex.InnerException is SqlException sqlEx && sqlEx.Number == 2601)
            catch (Exception)
            {
                return null;
            }
        }

        public async Task<TokenDto> Login(UserLoginDto data)
        {
            var userFound =  await _context.User
                .FirstOrDefaultAsync(u => u.Username == data.Username.ToLower());

            if(userFound == null) return null;

            var isCorrect = _passwordService.VerifyPassword(data.Username, userFound.Password, data.Password);
            if(isCorrect == false) return null;

            var token = _jwtService.GenerateToken(userFound.Id.ToString());
            return new TokenDto
            {
                Token = token
            };
        }
    }
}
