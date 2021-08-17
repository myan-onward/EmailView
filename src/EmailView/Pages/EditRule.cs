using System;
using System.Linq;
using System.Threading.Tasks;
using EmailView.DataServices;
using EmailView.Dtos;
using Microsoft.AspNetCore.Components;

namespace EmailView.Pages
{
    public partial class EditRule
    {
        [Inject]
        IAlertService AlertService { get; set; }

        [Inject]
        NavigationManager NavigationManager { get; set; }

        [Inject]
        IRuleDataService RuleService { get; set; }

        [Inject]
        StackService MailService { get; set; }

        [Parameter]
        public string ruleId { get; set; }

        public RuleDto addedRule;

        private ConditionDto addedCondition = new ConditionDto();

        private ActionDto addedAction = new ActionDto();

        public bool LoggedIn
        {
            get { return MailService.IsAuthenticated; }
        }

        protected override async Task OnParametersSetAsync()
        {
            Int32.TryParse(ruleId, out int rid);
            var ruleResp = await RuleService.GetRule(rid);
            addedRule = ruleResp;
            Console.WriteLine($"OnParametersSetAsync:{addedRule.Id}");
        }

        private async void HandleConditionSubmit()
        {
            Console.WriteLine("HandleConditionSubmit called");

            // set condition default based on razor defaults
            if (string.IsNullOrWhiteSpace(addedCondition.Condition))
            {
                addedCondition.Condition = "PEOPLE_FROM";
            }

            // set action default based on razor defaults
            if (string.IsNullOrWhiteSpace(addedCondition.Operator))
            {
                addedCondition.Operator = "EQUALS";
            }

            // Process the valid form
            Int32.TryParse(ruleId, out int rid);
            var result = await RuleService.AddCondition(rid, addedCondition.Condition, addedCondition.Operator, addedCondition.OnThis);
            if (result != null)
            {
                Console.WriteLine($"HandleConditionSubmit: Result (Condition) Id: {result.Id}");
                addedRule.Conditions.Add(new ConditionDto()
                {
                    Id = result.Id,
                    Condition = result.Condition,
                    Operator = result.Operator,
                    OnThis = result.OnThis,
                    RuleId = result.RuleId
                });

                // addedRule = result;
                StateHasChanged();
                // NavigationManager.NavigateTo("/editrule/{result.Id}");
            }
        }

        private async void HandleConditionDelete(int conditionId)
        {
            int result = await RuleService.DeleteCondition(conditionId);
            if (result != -1)
            {
                var removedItem = addedRule.Conditions.SingleOrDefault(x => x.Id == conditionId);

                if (removedItem != null)
                {
                    addedRule.Conditions.Remove(removedItem);
                    StateHasChanged();
                }
            }
        }

        private async void HandleActionSubmit()
        {
            Console.WriteLine("HandleActionSubmit called");

            // set action default based on razor defaults
            if (string.IsNullOrWhiteSpace(addedAction.ActionType))
            {
                addedAction.ActionType = "ORGANIZE_MOVE_TO";
            }

            // Process the valid form
            Int32.TryParse(ruleId, out int rid);
            var result = await RuleService.AddAction(rid, addedAction.ActionType, addedAction.DirectObject);
            if (result != null)
            {
                Console.WriteLine($"HandleActionSubmit: Result (Action) Id: {result.Id}");
                addedRule.Actions.Add(new ActionDto()
                {
                    Id = result.Id,
                    ActionType = result.ActionType,
                    DirectObject = result.DirectObject,
                    RuleId = result.RuleId
                });

                // addedRule = result;
                StateHasChanged();
                // NavigationManager.NavigateTo("/editrule/{result.Id}");
            }
        }

        private async void HandleActionDelete(int actionId)
        {
            int result = await RuleService.DeleteAction(actionId);

            if (result != -1)
            {
                var removedItem = addedRule.Actions.SingleOrDefault(x => x.Id == actionId);

                if (removedItem != null)
                {
                    addedRule.Actions.Remove(removedItem);
                    StateHasChanged();
                }
            }
        }
    }
}