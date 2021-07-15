using System.Collections.Generic;
using System.Linq;
using EmailView.Dtos;
using MailKit;

namespace EmailView.DataServices
{
    public class StackService : IMAPService
    {
        // Stacks override the default email summary fields
        // const MessageSummaryItems SummaryItems = MessageSummaryItems.UniqueId | MessageSummaryItems.Envelope | MessageSummaryItems.Flags | MessageSummaryItems.BodyStructure | MessageSummaryItems.Headers;

        public List<MessageInfo> MakeReplyStack()
        {
            List<MessageInfo> replyMsgs = new List<MessageInfo>();

            foreach (var message in this.messages)
            {
                // message body criteria such as "can you tell me?"

                // Condition: eBay Member response required
                if (message.Summary.Envelope.From.Mailboxes.Any(m => m.Address.EndsWith("@members.ebay.com")))
                {
                    replyMsgs.Add(message);
                    continue;
                }
            }

            return replyMsgs;
        }

        public List<MessageInfo> MakeForwardStack()
        {
            List<MessageInfo> forwardMsgs = new List<MessageInfo>();

            foreach (var message in this.messages)
            {
                // Condition: Order or billed message (placed here for now to action)
                if (message.Summary.Envelope.From.Mailboxes.Any(m => m.Address.StartsWith("order")))
                {
                    forwardMsgs.Add(message);
                    continue;
                }

                // Condition: Payment by eBay (TODO: put into filter list)
                if (message.Summary.Envelope.From.Mailboxes.Any(m => m.Address.StartsWith("service@paypal")))
                {
                    forwardMsgs.Add(message);
                    continue;
                }

                // Condition: Payment by eBay (TODO: put into filter list)
                if (message.Summary.Envelope.From.Mailboxes.Any(m => m.Address.StartsWith("ebay@ebay")) &&
                message.Summary.Envelope.Subject.ToLower().Contains("item") &&
                message.Summary.Envelope.Subject.ToLower().Contains("sold"))
                {
                    forwardMsgs.Add(message);
                    continue;
                }

                // Condition: eBay label ready
                if (message.Summary.Envelope.From.Mailboxes.Any(m => m.Address.StartsWith("ebay@ebay.com")) &&
                    message.Summary.Envelope.Subject.ToLower().Contains("label") &&
                message.Summary.Envelope.Subject.ToLower().Contains("ready"))
                {
                    forwardMsgs.Add(message);
                    continue;
                }

                // Condition: Apple Pay (ment)
                if (message.Summary.Envelope.From.Mailboxes.Any(m => m.Address.StartsWith("no_reply@post.applecard.apple")) &&
                    message.Summary.Envelope.Subject.ToLower().Contains("payment") &&
                message.Summary.Envelope.Subject.ToLower().Contains("received"))
                {
                    forwardMsgs.Add(message);
                    continue;
                }
            }
            return forwardMsgs;
        }

        public List<MessageInfo> MakeMeetStack()
        {
            List<MessageInfo> meetMsgs = new List<MessageInfo>();

            foreach (var message in this.messages)
            {
                // Condition: MIME body part has type Content-Type: text/calendar;
                bool hasCalendarType = false;
                foreach (var bodyPart in message.Summary.BodyParts)
                {
                    if (bodyPart.ContentType.MediaSubtype.Equals("calendar"))
                    {
                        if (bodyPart.ContentType.MediaType.Equals("text"))
                        {
                            hasCalendarType = true;
                            break;
                        }
                    }
                }
                if (hasCalendarType)
                {
                    meetMsgs.Add(message);
                    continue;
                }

                // Condition: From: calendar-notification@google.com
                if (message.Summary.Envelope.From.Mailboxes.FirstOrDefault().Address.Equals("calendar-notification@google.com"))
                {
                    meetMsgs.Add(message);
                    continue;
                }

                // Condition: Subject is "Invitation:"
                if (message.Summary.Envelope.Subject.StartsWith("Invitation:"))
                {
                    meetMsgs.Add(message);
                    continue;
                }

                // Condition: Attachment ".ics" (calendar) (.e.g invite.ecs)
                bool hasCal = false;
                foreach (var att in message.Summary.Attachments)
                {
                    if (att.FileName.EndsWith(".ics"))
                    {
                        hasCal = true;
                        break;
                    }
                }
                if (hasCal)
                {
                    meetMsgs.Add(message);
                    continue;
                }
            }

            return meetMsgs;
        }

        public List<MessageInfo> MakeReviewStack()
        {
            List<MessageInfo> reviewMsgs = new List<MessageInfo>();

            bool hasUnsub = false;
            foreach (var message in this.messages)
            {
                // Condition: Header: List-Unsubscribe
                foreach (var header in message.Summary.Headers)
                {
                    if (header.Id.Equals(MimeKit.HeaderId.ListUnsubscribe))
                    {
                        hasUnsub = true;
                        break;
                    }

                    if (header.Id.Equals(MimeKit.HeaderId.ListUnsubscribePost))
                    {
                        hasUnsub = true;
                        break;
                    }
                }
                if (hasUnsub)
                {
                    reviewMsgs.Add(message);
                    continue;
                }
            }

            return reviewMsgs;
        }

        public List<MessageInfo> MakeReviewCcStack()
        {
            List<MessageInfo> ccMsgs = new List<MessageInfo>();

            foreach (var message in this.messages)
            {
                // Condition: you are CC'd
                if (message.Summary.Envelope.Cc.Mailboxes.Any(m => m.Address.Equals(_email, System.StringComparison.OrdinalIgnoreCase)))
                {
                    ccMsgs.Add(message);
                    continue;
                }

                // Condition: you are BCC'd
                if (message.Summary.Envelope.Bcc.Mailboxes.Any(m => m.Address.Equals(_email, System.StringComparison.OrdinalIgnoreCase)))
                {
                    ccMsgs.Add(message);
                    continue;
                }
            }

            return ccMsgs;
        }

        public List<MessageInfo> MakeDoStack()
        {
            List<MessageInfo> doMsgs = new List<MessageInfo>();

            foreach (var message in this.messages)
            {
                // Condition: Google critical security warning
                if (message.Summary.Envelope.From.Mailboxes.Any(m => m.Address.StartsWith("no-reply@accounts.google.com")))
                {
                    doMsgs.Add(message);
                    continue;
                }

                // Condition: Yahoo critical security warning
                if (message.Summary.Envelope.From.Mailboxes.Any(m => m.Address.StartsWith("no-reply@cc.yahoo-inc.com")))
                {
                    doMsgs.Add(message);
                    continue;
                }

                // Condition: Coinbase reminder, verify
                if (message.Summary.Envelope.From.Mailboxes.Any(m => m.Address.StartsWith("no-reply@coinbase.com")) &&
                message.Summary.Envelope.Subject.ToLower().Contains("reminder") &&
                message.Summary.Envelope.Subject.ToLower().Contains("verify"))
                {
                    doMsgs.Add(message);
                    continue;
                }

                // Condition: Coinbase requesting, access
                if (message.Summary.Envelope.From.Mailboxes.Any(m => m.Address.StartsWith("no-reply@coinbase.com")) &&
                message.Summary.Envelope.Subject.ToLower().Contains("requesting") &&
                message.Summary.Envelope.Subject.ToLower().Contains("access"))
                {
                    doMsgs.Add(message);
                    continue;
                }

                // Condition: eBay payment, respond with shipping
                if (message.Summary.Envelope.From.Mailboxes.Any(m => m.Address.StartsWith("ebay@ebay.com")) &&
                    message.Summary.Envelope.Subject.ToLower().Contains("payment") &&
                message.Summary.Envelope.Subject.ToLower().Contains("confirmed"))
                {
                    doMsgs.Add(message);
                    continue;
                }
            }

            return doMsgs;
        }
    }
}