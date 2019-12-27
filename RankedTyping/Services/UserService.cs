using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using RankedTyping.Models;
using RankedTyping.Request;
using RankedTyping.Utils;

namespace RankedTyping.Services
{
    public interface IUserService
    {
        User Authenticate(string username, string password);
        User Register(string email, string password, string username);
        User Fetch(int userId);
        bool Delete(int userId);
        bool ChangePassword(int userId, ResetPasswordRequest request);
        bool ChangeEmail(int userId, ChangeEmailRequest request);
        bool ChangeUsername(int userId, ChangeUsernameRequest request);
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
            {
                user = _context.Users.SingleOrDefault(x => x.Username == email);
                if (user == null)
                {
                    return null;
                }
            }
            
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
                Password = BCrypt.Net.BCrypt.HashString(password),
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
            var key = Encoding.ASCII.GetBytes(_config["RankedSettings_Secret"]);
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

        /**
         * Fetch a user by user id
         */
        public User Fetch(int userId)
        {
          return _context.Users.SingleOrDefault(x => x.Id == userId);
        }

        /**
         * Delete a user by user id
         */
        public bool Delete(int userId)
        {
            var user = Fetch(userId);
            if (user == null) return false;
            
            _context.Users.Remove(user);
            _context.SaveChanges();
            
            return true;
        }

        public bool ChangePassword(int userId, ResetPasswordRequest request)
        {
            var user = Fetch(userId);
            if (user == null) return false;

            user.Password = BCrypt.Net.BCrypt.HashPassword(request.password);
            _context.Users.Update(user);
            _context.SaveChanges();

            return true;
        }

        public bool ChangeEmail(int userId, ChangeEmailRequest request)
        {
            var user = Fetch(userId);
            if (user == null) return false;
            
            //verify its not already being used
            var exists = _context.Users
                .Where(u => u.Email == request.email)
                .FirstOrDefault(e => e.Id != userId);
            
            if (exists != null) return false;
            
            var helper = new GravatarHelper();

            user.Email = request.email;
            user.EmailMd5 = helper.CalculateMD5Hash(request.email);
            
            _context.Users.Update(user);
            _context.SaveChanges();

            return true;
        }

        public bool ChangeUsername(int userId, ChangeUsernameRequest request)
        {
            var user = Fetch(userId);
            if (user == null) return false;
            
            //verify its not already being used
            var exists = _context.Users
                .Where(u => u.Username == request.username)
                .FirstOrDefault(e => e.Id != userId);
            
            if (exists != null) return false;

            user.Username = request.username;
            
            _context.Users.Update(user);
            _context.SaveChanges();

            return true;
        }
    }
}