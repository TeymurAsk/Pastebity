using Microsoft.EntityFrameworkCore;
using Pastebin_api.Controllers;
using Pastebin_api.Data;

namespace Pastebin_api.Services
{
    public class AuthService
    {
        private readonly HashGenerator _hashGenerator;
        private readonly JWTProvider _jwtProvider;
        private readonly PastebinDbContext _dbContext;
        public AuthService(HashGenerator hashGenerator, JWTProvider jWTProvider, PastebinDbContext dbContext)
        {
            _hashGenerator = hashGenerator;
            _jwtProvider = jWTProvider;
            _dbContext = dbContext;
        }
        public void Login(string email, string password, HttpContext context)
        {
            var token = CreateJWT(email, password);
            context.Response.Cookies.Append("tasty-users-cookies", token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true
            });
        }
        public string CreateJWT(string email, string password)
        {
            var user = _dbContext.Users.SingleOrDefault(u => u.Email == email);

            var result = _hashGenerator.Verify(password, user.PasswordHash);
            if (result == false)
            {
                throw new Exception("Login credetials are false!");
            }
            var token = _jwtProvider.GenerateToken(user);
            return token;
        }
    }
}
