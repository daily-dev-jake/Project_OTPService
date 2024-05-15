using MimeKit;

namespace OTPService.Models
{
    public class Mail
    {
        public Mail(IEnumerable<string> receivers, string sender, string subject, string body)
        {
            Receivers = new List<MailboxAddress>();
            Receivers.AddRange(receivers.Select(x => new MailboxAddress("email",x)));
            Sender = sender;
            Subject = subject;
            Body = body;
        }

        public List<MailboxAddress> Receivers { get; }
        public string Sender { get; }
        public string Subject { get; }
        public string Body { get; }
    }
}
