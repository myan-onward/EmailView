using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace EmailView.Dtos
{
    public partial class GqlData
    {
        [JsonPropertyName("data")]
        public Data Data { get; set; }
    }

    public partial class Data
    {
        [JsonPropertyName("rules")]
        public List<RuleDto> Rules { get; set; }
    }
}
