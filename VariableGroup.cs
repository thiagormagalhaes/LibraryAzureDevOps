using System.Text.Json.Serialization;

namespace LibraryAzureDevOps;

public class VariableGroup
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; } = default!;

    [JsonPropertyName("variables")]
    public Dictionary<string, Variable> Variables { get; set; } = new();
}
