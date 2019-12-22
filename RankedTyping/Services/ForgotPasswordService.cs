using System;
using System.Linq;
using RankedTyping.Models;
using RankedTyping.Request;

namespace RankedTyping.Services
{
    public interface IForgotPasswordService
    {
        public bool SendResetEmail(SendResetEmailRequest request);
        public bool ChangePassword(ChangePasswordRequest request);
    }

    public class ForgotPasswordService : IForgotPasswordService
    {
        private readonly RankedContext _context;

        public ForgotPasswordService(RankedContext context)
        {
            _context = context;
        }

        public bool SendResetEmail(SendResetEmailRequest request)
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

            //Todo: Send email

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