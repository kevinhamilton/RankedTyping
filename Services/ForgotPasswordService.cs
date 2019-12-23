using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using RankedTyping.Models;
using RankedTyping.Request;
using RankedTyping.Utils;

namespace RankedTyping.Services
{
    public interface IForgotPasswordService
    {
        public Task<bool> SendResetEmail(SendResetEmailRequest request);
        public bool ChangePassword(ChangePasswordRequest request);
    }

    public class ForgotPasswordService : IForgotPasswordService
    {
        private readonly RankedContext _context;
        private readonly IConfiguration _config;

        public ForgotPasswordService(RankedContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        public async Task<bool> SendResetEmail(SendResetEmailRequest request)
        {
            var user = _context.Users
                .FirstOrDefault(u => u.Email == request.email);
            
            if (user == null) 
                return false;
            
            //Generate token
            var reset = new ForgotPassword();
            reset.Email = user.Email;
            reset.Token = Guid.NewGuid().ToString("N");
            reset.CreatedAt = DateTime.Now;
            reset.UpdatedAt = DateTime.Now;

            //Persist changes to db
            _context.ForgotPasswords.Add(reset);
            _context.SaveChanges();

            //Send email
            var to = new List<string> { user.Email };
            var client = new TransactionalEmailClient(_config);
            
            await client.SendEmail(
                to,
                "A request to reset your password was initiated",
                "If it was you who requested to reset your password, click the link below to reset your password. If you did not request this change, then you can ignore this email.",
                "/recover/" + reset.Token,
                "Click Here to Reset Your Password");

            return true;
        }

        public bool ChangePassword(ChangePasswordRequest request)
        {
            //Verify token
            var reset = _context.ForgotPasswords
                .Where(f => f.Email == request.email)
                .FirstOrDefault(t => t.Token == request.token);

            if (reset == null)
                return false;
            
            var user = _context.Users
                .FirstOrDefault(u => u.Email == request.email);

            if (user == null)
                return false;
            
            //Change password
            user.Password = BCrypt.Net.BCrypt.HashPassword(request.password);
            
            //Remove forgot password
            _context.ForgotPasswords.Remove(reset);
            
            //Persist changes
            _context.Users.Update(user);
            _context.SaveChanges();

            return true;
        }
    }
}