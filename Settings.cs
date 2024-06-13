namespace LibraryAzureDevOps;

public class Settings
{
    public string PersonalAccessTokens { get; init; } = default!;
    public string DefaultGroup { get; init; } = "";
    public string Organization { get; init; } = default!;
    public string Project { get; init; } = default!;
    public string ApiVersion { get; init; } = default!;

    public string GetLinkLibrary(int id)
    {
        return
            $"https://{Organization}.visualstudio.com/{Project}/_library?itemType=VariableGroups&view=VariableGroupView&variableGroupId={id}";
    }
}