using System.Text.Json.Serialization;

namespace EmailView.Dtos
{
    public partial class GqlDeleteData
    {
        [JsonPropertyName("ruleId")]
        public int RuleId { get; set; }

        [JsonPropertyName("conditionId")]
        public int ConditionId { get; set; }

        [JsonPropertyName("actionId")]
        public int ActionId { get; set; }

        [JsonPropertyName("data")]
        public Data Data { get; set; }
    }

    public partial class Data
    {
        [JsonPropertyName("ruleId")]
        public int RuleId { get; set; }

        [JsonPropertyName("conditionId")]
        public int ConditionId { get; set; }

        [JsonPropertyName("actionId")]
        public int ActionId { get; set; }
    }
}
