using Microsoft.AspNetCore.Identity;

namespace Src.Application.Service
{

    public class PasswordService
    {
        private readonly PasswordHasher<string> _passwordHasher = new();

        public string HashPassword(string username, string plainPassword)
        {
            return _passwordHasher.HashPassword(username, plainPassword);
        }

        public bool VerifyPassword(string username, string hashedPassword, string plainPassword)
        {
            var result = _passwordHasher.VerifyHashedPassword(username, hashedPassword, plainPassword);
            return result == PasswordVerificationResult.Success;
        }
    }
}