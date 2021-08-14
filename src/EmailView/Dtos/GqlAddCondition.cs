using System.Text.Json.Serialization;

namespace EmailView.Dtos
{
    public partial class GqlAddCondition
    {
        [JsonPropertyName("data")]
        public Data Data { get; set; }
    }

    public partial class Data
    {
        [JsonPropertyName("condition")]
        public ConditionDto Condition { get; set; }
    }
}
