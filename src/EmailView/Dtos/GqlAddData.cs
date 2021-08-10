using System.Text.Json.Serialization;

namespace EmailView.Dtos
{
    public partial class GqlAddData
    {
        [JsonPropertyName("data")]
        public Data Data { get; set; }
    }

    public partial class Data
    {
        [JsonPropertyName("rule")]
        public RuleDto Rule { get; set; }
    }
}
