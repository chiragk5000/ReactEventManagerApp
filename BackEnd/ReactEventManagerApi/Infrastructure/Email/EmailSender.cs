using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json.Linq;
using Resend;

namespace Infrastructure.Email
{
    public class EmailSender(IResend resend,EmailTemplateService emailTemplateService) : IEmailSender<User>
    {
        public async Task SendConfirmationLinkAsync(User user, string email, string confirmationLink)
        {
            var template = emailTemplateService.Get("ConfirmEmail");
            await SendMailAsync(email, template, new Dictionary<string, string>
            {
                { "UserName", user.UserName! },
                { "Link", confirmationLink }
            });
        }

        private async Task SendMailAsync(string email, EmailTemplateModel template, Dictionary<string, string> dictionary)
        {
            string subject = Replace(template.Subject, dictionary);
            string body = Replace(template.Body, dictionary);

            var message = new EmailMessage
            {
                From = "ReactvitiesCourse@resend.dev",
                Subject=template.Subject,
                HtmlBody=body,
            };

            message.To.Add(email);
            Console.WriteLine(message.HtmlBody);
            await resend.EmailSendAsync(message);
        }

        public Task SendPasswordResetCodeAsync(User user, string email, string resetCode)
        {
            throw new NotImplementedException();
        }

        public Task SendPasswordResetLinkAsync(User user, string email, string resetLink)
        {
            throw new NotImplementedException();
        }
        private static string Replace(string text, Dictionary<string, string> values)
        {
            foreach (var kv in values)
            {
                text = text.Replace($"{{{{{kv.Key}}}}}", kv.Value);
            }
            return text;
        }
    }
}
