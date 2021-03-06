using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EmailView.DataServices;
using EmailView.Dtos;
using Microsoft.AspNetCore.Components;

namespace EmailView.Pages
{
    public partial class Rules
    {
        [Inject]
        IAlertService AlertService { get; set; }

        [Inject]
        NavigationManager NavigationManager { get; set; }

        [Inject]
        IRuleDataService RuleService { get; set; }

        [Inject]
        StackService MailService { get; set; }

        public List<RuleDto> rules = new List<RuleDto>();

        public bool LoggedIn
        {
            get { return MailService.IsAuthenticated; }
        }

        protected override async Task OnInitializedAsync()
        {
            rules = await RuleService.GetAllRules();
        }

        private async void HandleRuleDelete(int ruleId)
        {
            int result = await RuleService.DeleteRule(ruleId);

            if (result != -1)
            {
                var removedItem = rules.SingleOrDefault(x => x.Id == ruleId);

                if (removedItem != null)
                {
                    rules.Remove(removedItem);
                    StateHasChanged();
                }
            }
        }
    }
}