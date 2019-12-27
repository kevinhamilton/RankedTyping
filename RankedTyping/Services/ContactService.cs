using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using RankedTyping.Models;
using RankedTyping.Request;
using RankedTyping.Responses;
using RankedTyping.Utils;

namespace RankedTyping.Services
{
    public interface IContactService
    {
        public bool SendContactNotification(ContactRequest request);
    }

    public class ContactService : IContactService
    {
        private readonly IConfiguration _config;

        public ContactService(RankedContext context, IConfiguration config)
        {
            _config = config;
        }

        public bool SendContactNotification(ContactRequest request)
        {
            var contactEmail = _config["RankedSettings_ContactEmail"];
            var to = new List<string> { contactEmail };

            var client = new TransactionalEmailClient(_config);

            client.SendEmail(
                to,
                "Contact Form: " + request.subject,
                request.message,
                "",
                "Contact");

            return true;
        }
    }
}