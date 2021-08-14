using System.Collections.Generic;
using System.Threading.Tasks;
using EmailView.Dtos;

namespace EmailView.DataServices
{
    public interface IRuleDataService
    {
        Task<List<RuleDto>> GetAllRules();
        Task<RuleDto> GetRule(int id);
        Task<RuleDto> AddRule(string Name);
        Task<ConditionDto> AddCondition(int ruleId, string sCondition, string sOperator, string onThis);

        Task<ActionDto> AddAction(int ruleId, string sAction, string directObject);
    }
}