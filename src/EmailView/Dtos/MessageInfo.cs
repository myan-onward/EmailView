using System.Linq;
using MailKit;

namespace EmailView.Dtos
{
    public class MessageInfo
    {
        public readonly IMessageSummary Summary;
        public MessageFlags Flags;
        public string FromSimple;
        public string Category;
        

        public MessageInfo(IMessageSummary summary)
        {
            Summary = summary;

            if (summary.Flags.HasValue)
                Flags = summary.Flags.Value;

            if (string.IsNullOrWhiteSpace(summary.Envelope.From.Mailboxes.FirstOrDefault().Name))
            {
                FromSimple = summary.Envelope.From.Mailboxes.FirstOrDefault().Address;
            }
            else
            {
                FromSimple = summary.Envelope.From.Mailboxes.FirstOrDefault().Name;
            }

        }
    }
}