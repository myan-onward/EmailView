using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EmailView.DataServices;
using EmailView.Dtos;
using Microsoft.AspNetCore.Components;

namespace EmailView.Pages
{
    public partial class AddRule
    {
        [Inject]
        IAlertService AlertService { get; set; }

        [Inject]
        NavigationManager NavigationManager { get; set; }

        [Inject]
        IRuleDataService RuleService { get; set; }

        [Inject]
        StackService MailService { get; set; }

        private string ruleName;
        private RuleDto addedRule = new RuleDto();

        // private ConditionDto addedCondition = new ConditionDto();

        // private ActionDto addedAction = new ActionDto();

        public bool LoggedIn
        {
            get { return MailService.IsAuthenticated; }
        }

        private async void HandleValidSubmit()
        {
            Console.WriteLine("HandleValidSubmit called");

            // Process the valid form
            var result = await RuleService.AddRule(ruleName);
            if(result != null)
            {
                addedRule = result;
                // StateHasChanged();
                NavigationManager.NavigateTo($"/editrule/{result.Id}");
            }
        }
    }
}