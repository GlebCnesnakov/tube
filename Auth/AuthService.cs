using Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;

namespace Auth
{
    public class AuthService(Data.AppContext appContext, JwtService jwtService) : IAuthService
    {
        public async Task Register(string login, string password)
        {
            User user = new User()
            {
                Login = login
            };
            var hashedPass = new PasswordHasher<User>().HashPassword(user, password);
            user.Password = hashedPass;
            await appContext.Users.AddAsync(user);
            
            await appContext.SaveChangesAsync();
        }

        public async Task<string> Authenticate(string login, string password)
        {
            var user = await appContext.Users.FirstOrDefaultAsync(x => x.Login == login);
            var authenticated = new PasswordHasher<User>().VerifyHashedPassword(user, user.Password, password);
            if (authenticated == PasswordVerificationResult.Success)
            {
                return jwtService.GetJwt(user);
            }
            else throw new Exception("Unauthorized");
        }
    }
}
