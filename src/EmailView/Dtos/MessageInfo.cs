using System.Linq;
using MailKit;

namespace EmailView.Dtos
{
    public class MessageInfo
    {
        public readonly IMessageSummary Summary;
        public MessageFlags Flags;
        public string FromSimple;

        public MessageInfo(IMessageSummary summary)
        {
            Summary = summary;

            if (summary.Flags.HasValue)
                Flags = summary.Flags.Value;

            FromSimple = summary.Envelope.From.Mailboxes.FirstOrDefault().Name;
        }
    }
}