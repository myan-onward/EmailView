using System.Text.Json.Serialization;

namespace EmailView.Dtos
{
    public partial class GqlAddAction
    {
        [JsonPropertyName("data")]
        public Data Data { get; set; }
    }

    public partial class Data
    {
        [JsonPropertyName("action")]
        public ActionDto Action { get; set; }
    }
}
