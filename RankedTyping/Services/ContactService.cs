using System;
using Microsoft.Extensions.Caching.Memory;
using RankedTyping.Models;
using RankedTyping.Request;

namespace RankedTyping.Services
{
    public interface IContactService
    {
        public bool SendContactNotification(ContactRequest request);
    }

    public class ContactService : IContactService
    {
        private readonly RankedContext _context;

        public ContactService(RankedContext context, IMemoryCache cache)
        {
            _context = context;
        }

        public bool SendContactNotification(ContactRequest request)
        {
            //todo send email
            
            return true;
        }
    }
}