using System.Collections.Generic;
using System.Threading.Tasks;
using EmailView.DataServices;
using EmailView.Dtos;
using Microsoft.AspNetCore.Components;

namespace EmailView.Pages
{
    public partial class Emails
    {
        [Inject]
        IAlertService AlertService { get; set; }

        [Inject]
        NavigationManager NavigationManager { get; set; }

        [Inject]
        IMAPService MailService { get; set; }

        public List<MessageInfo> messages = new List<MessageInfo>();

        public bool LoggedIn
        {
            get { return MailService.IsAuthenticated; }
        }

        protected override async Task OnInitializedAsync()
        {
            messages = await MailService.GetMessagesAsync();
        }
    }
}