using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EmailView.DataServices;
using EmailView.Dtos;
using Microsoft.AspNetCore.Components;

namespace EmailView.Pages
{
    public partial class PreviewRule
    {
        [Inject]
        IAlertService AlertService { get; set; }

        [Inject]
        NavigationManager NavigationManager { get; set; }

        [Inject]
        StackService MailService { get; set; }
        
        [Inject]
        IRuleDataService RuleService { get; set; }

        [Parameter]
        public string ruleId { get; set; }

        public List<MessageInfo> previewMsgs = new List<MessageInfo>();

        public List<MessageInfo> messages = new List<MessageInfo>();

        public RuleDto previewRule;

        public bool LoggedIn
        {
            get { return MailService.IsAuthenticated; }
        }

        protected override async Task OnInitializedAsync()
        {
            Int32.TryParse(ruleId, out int rid);
            var ruleResp = await RuleService.GetRule(rid);
            previewRule = ruleResp;

            messages = await MailService.GetMessagesAsync();
            previewMsgs = MailService.MakeStack(previewRule.Conditions);
        }       
    }
}