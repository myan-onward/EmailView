using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace EmailView.Dtos
{
    public partial class RuleDto
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("shared")]
        public bool Shared { get; set; }

        [JsonPropertyName("requireAllConditions")]
        public bool RequireAllConditions { get; set; }

        [JsonPropertyName("conditions")]
        public List<ConditionDto> Conditions { get; set; }

        [JsonPropertyName("actions")]
        public List<ActionDto> Actions {get; set;}

    }

    public partial class ConditionDto
    {
        [JsonPropertyName("id")]
        public int Id {get; set;}

        [JsonPropertyName("operator")]
        public string Operator {get; set;}

        [JsonPropertyName("condition")]
        public string Condition {get; set;}

        [JsonPropertyName("onThis")]
        public string OnThis {get; set;}

        [JsonPropertyName("ruleId")]
        public int RuleId {get; set;}
    }

    public partial class ActionDto
    {
        [JsonPropertyName("id")]
        public int Id {get; set;}

        [JsonPropertyName("actionType")]
        public string ActionType {get; set;}

        [JsonPropertyName("directObject")]
        public string DirectObject {get; set;}

        [JsonPropertyName("ruleId")]
        public int RuleId {get; set;}
    }
}