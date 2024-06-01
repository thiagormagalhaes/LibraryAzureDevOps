using System.Text.Json.Serialization;

namespace LibraryAzureDevOps;

public class Variable
{
    [JsonPropertyName("value")]
    public string Value { get; set; }

    [JsonPropertyName("isSecret")]
    public bool IsSecret { get; set; }
}
