using System.Collections.Generic;
using System.Threading.Tasks;
using EmailView.DataServices;
using EmailView.Dtos;
using Microsoft.AspNetCore.Components;

namespace EmailView.Pages
{
    public partial class Stacks
    {
        [Inject]
        IAlertService AlertService { get; set; }

        [Inject]
        NavigationManager NavigationManager { get; set; }

        [Inject]
        StackService MailService { get; set; }

        public List<MessageInfo> replyMsgs = new List<MessageInfo>();
        public List<MessageInfo> forwardMsgs = new List<MessageInfo>();
        public List<MessageInfo> meetMsgs = new List<MessageInfo>();
        public List<MessageInfo> reviewMsgs = new List<MessageInfo>();
        public List<MessageInfo> ccMsgs = new List<MessageInfo>();
        public List<MessageInfo> doMsgs = new List<MessageInfo>();

        public List<MessageInfo> messages = new List<MessageInfo>();

        public bool LoggedIn
        {
            get { return MailService.IsAuthenticated; }
        }

        protected override async Task OnInitializedAsync()
        {
            messages = await MailService.GetMessagesAsync();
            replyMsgs = MailService.MakeReplyStack();
            forwardMsgs = MailService.MakeForwardStack();
            meetMsgs = MailService.MakeMeetStack();
            reviewMsgs = MailService.MakeReviewStack();
            ccMsgs = MailService.MakeReviewCcStack();
            doMsgs = MailService.MakeDoStack();
        }
    }
}