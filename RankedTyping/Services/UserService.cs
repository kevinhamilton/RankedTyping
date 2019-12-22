using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using RankedTyping.Models;
using RankedTyping.Utils;

namespace RankedTyping.Services
{
    public interface IUserService
    {
        User Authenticate(string username, string password);
        User Register(string email, string password, string username);
    }

    public class UserService : IUserService
    {
        private readonly IConfiguration _config;

        private readonly RankedContext _context;

        public UserService(RankedContext context, IConfiguration config)
        {
            _config = config;
            _context = context;
        }

        /**
         * Authenticate a user, returning the JWT token.
         */
        public User Authenticate(string email, string password)
        {
            var user = _context.Users.SingleOrDefault(x => x.Email == email);

            if (user == null)
                return null;
            
            if ( ! BCrypt.Net.BCrypt.Verify(password, user.Password)) 
                return null;

            return AppendToken(user);
        }

        /**
         * Create a new account.
         */
        public User Register(string email, string password, string username)
        {
            var gravatar = new GravatarHelper();

            var user = new User
            {
                Email = email,
                Username = username,
                Password = password,
                EmailMd5 = gravatar.CalculateMD5Hash(email),
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
            };

            _context.Users.Add(user);
            _context.SaveChanges();

            return AppendToken(user);
        }

        /**
         * Appends a JWT token to the user's information.
         */
        private User AppendToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_config["AppSettings:Secret"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString())
                }),
                Expires = DateTime.Now.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            user.Token = tokenHandler.WriteToken(token);
            return user;
        }
    }
}