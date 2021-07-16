using System.Collections.Generic;
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
            query  = @"{rules {id name shared requireAllConditions conditions {id onThis} actions {id directObject} }}",
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
    }
}