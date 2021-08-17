using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using EmailView.Dtos;

namespace EmailView.DataServices
{
    public class RuleService : IRuleDataService
    {
        private readonly HttpClient _httpclient;

        public RuleService(HttpClient httpclient)
        {
            _httpclient = httpclient;
        }

        public async Task<List<RuleDto>> GetAllRules()
        {
           var queryObject = new{
            query  = @"{rules {id name shared requireAllConditions conditions {id operator condition onThis} actions {id actionType directObject ruleId} }}",
            variables = new {}
           };

           var ruleQuery = new StringContent(
               JsonSerializer.Serialize(queryObject),
               Encoding.UTF8,
               "application/json");

            var response = await _httpclient.PostAsync("graphql", ruleQuery);

            if(response.IsSuccessStatusCode)
            {
                var gqlData = await JsonSerializer.DeserializeAsync<GqlData>  
                    (await response.Content.ReadAsStreamAsync());
                
                return gqlData.Data.Rules;
            }
            return null;
        }

        public async Task<RuleDto> GetRule(int id)
        {
            var v = new { rid = id };
            var queryObject = new {
                query = @"query ($rid: Int) { rules(where: {id: {eq: $rid}}) {id name shared requireAllConditions conditions {id operator condition onThis} actions {id actionType directObject ruleId} } }",
                variables = v
            };

            var ruleQuery = new StringContent(
               JsonSerializer.Serialize(queryObject),
               Encoding.UTF8,
               "application/json");

            var response = await _httpclient.PostAsync("graphql", ruleQuery);

            if(response.IsSuccessStatusCode)
            {
                var gqlData = await JsonSerializer.DeserializeAsync<GqlData>  
                    (await response.Content.ReadAsStreamAsync());
                
                return gqlData.Data.Rules.FirstOrDefault();
            }
            return null;
        }

        public async Task<RuleDto> AddRule(string name)
        {
            var v = new { name = name };
            var mutationObject = new {
                query = @"mutation addRule($input:AddRuleInput!){ addRule(input: $input) {rule {id name}} }",
                variables = new
                {
                    input = v
                }
            };

            var ruleMutation = new StringContent(
               JsonSerializer.Serialize(mutationObject),
               Encoding.UTF8,
               "application/json");

            var response = await _httpclient.PostAsync("graphql", ruleMutation);

            if(response.IsSuccessStatusCode)
            {
                var gqlData = await JsonSerializer.DeserializeAsync<GqlAddData>  
                    (await response.Content.ReadAsStreamAsync());
                
                return gqlData.Data.Rule;
            }
            return null;
        }

        public async Task<ConditionDto> AddCondition(int ruleId, string sCondition, string sOperator, string onThis)
        {
            // var v = new { condition = sCondition, @operator = sOperator, onThis = onThis, ruleId = ruleId };
            var v = new { specificCondition = sCondition, onThis = onThis, ruleId = ruleId };
            
            var mutationObject = new {
                query = @"mutation addCondition($input: AddConditionInput!) { addCondition(input: $input) { condition { id operator condition onThis ruleId } } }",                
                variables = new
                {
                    input = v
                }
            };

            var conditionMutation = new StringContent(
               JsonSerializer.Serialize(mutationObject),
               Encoding.UTF8,
               "application/json");

            var response = await _httpclient.PostAsync("graphql", conditionMutation);

            if(response.IsSuccessStatusCode)
            {
                var gqlData = await JsonSerializer.DeserializeAsync<GqlAddCondition>  
                    (await response.Content.ReadAsStreamAsync());
                
                return gqlData.Data.Condition;
            }
            return null;
        }

        public async Task<ActionDto> AddAction(int ruleId, string sAction, string directObject)
        {
            // var v = new { condition = sCondition, @operator = sOperator, onThis = onThis, ruleId = ruleId };
            var v = new { specificAction = sAction, directObject = directObject, ruleId = ruleId };
            
            var mutationObject = new {
                query = @"mutation addAction($input: AddActionInput!) { addAction(input: $input) { action { id actionType directObject ruleId } } }",                
                variables = new
                {
                    input = v
                }
            };

            var actionMutation = new StringContent(
               JsonSerializer.Serialize(mutationObject),
               Encoding.UTF8,
               "application/json");

            var response = await _httpclient.PostAsync("graphql", actionMutation);

            if(response.IsSuccessStatusCode)
            {
                var gqlData = await JsonSerializer.DeserializeAsync<GqlAddAction>  
                    (await response.Content.ReadAsStreamAsync());
                
                return gqlData.Data.Action;
            }
            return null;
        }
    }
}