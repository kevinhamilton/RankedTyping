using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Amazon;
using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;
using MailBodyPack;
using Microsoft.Extensions.Configuration;

namespace RankedTyping.Utils
{
    public class TransactionalEmailClient
    {
        private readonly IConfiguration _config;

        public TransactionalEmailClient(IConfiguration config)
        {
            _config = config;
        }
        
        public async Task<bool> SendEmail(List<string> to, string subject, string message, string link, string linkText)
        {
            var awsAccessKeyId = _config["AppSettings:AwsAccessKeyId"];
            var awsSecretAccessKey = _config["AppSettings:AwsSecretAccessKey"];
            
            var body = MailBody
                .CreateBody()
                .Paragraph("Hello,")
                .Paragraph(message)
                .Button("https://rankedtyping.com/" + link, linkText)
                .Paragraph("- RankedTyping Team")
                .ToString();
            
            using (var ses = new AmazonSimpleEmailServiceClient(awsAccessKeyId, awsSecretAccessKey, RegionEndpoint.USEast1))
            {
                var sendResult = await ses.SendEmailAsync(new SendEmailRequest
                {
                    Source = "alerts@rankedtyping.com",
                    Destination = new Destination(to),
                    Message = new Message
                    {
                        Subject = new Content(subject),
                        Body = new Body
                        {
                            Html = new Content(body)
                        }
                    }
                });
                return sendResult.HttpStatusCode == HttpStatusCode.OK;
            }
        }
    }
}