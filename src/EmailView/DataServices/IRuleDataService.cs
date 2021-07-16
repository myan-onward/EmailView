using System.Collections.Generic;
using System.Threading.Tasks;
using EmailView.Dtos;

namespace EmailView.DataServices
{
    public interface IRuleDataService
    {
        Task<List<RuleDto>> GetAllRules();
    }
}