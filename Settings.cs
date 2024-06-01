namespace LibraryAzureDevOps;

public class Settings
{
    public string LinkApi { get; init; } = default!;
    public string LinkLibrary { get; init; } = default!;
    public string UserAuthentication { get; init; } = default!;

    public string GetLinkLibrary(int id)
    {
        return string.Format("{0}{1}", LinkLibrary, id);
    }
}
