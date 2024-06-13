using System.Text.Json;
using LibraryAzureDevOps;
using Microsoft.Extensions.Configuration;
using Spectre.Console;

await Main(args);
return;

static async Task Main(string[] args)
{
    var settings = LoadSettings();
    var variablesResponse = await FetchVariablesResponseAsync(settings);

    var variableGroups = DeserializeVariableGroups(variablesResponse);
    var variableGroupService = new VariableGroupService(settings);

    var searchGroupName = PromptForGroupName(settings);
    var variableGroupFound = FilterVariableGroups(variableGroups, searchGroupName);

    variableGroupService.WriteVariableGroups(variableGroupFound);

    if (variableGroupFound.Any())
    {
        var searchKey = AnsiConsole.Ask<string>("[blue]What variable are you looking for?[/]");
        variableGroupService.WriteVariables(variableGroupFound, searchKey);
    }
}

static Settings LoadSettings()
{
    var builder = new ConfigurationBuilder()
        .SetBasePath(AppContext.BaseDirectory)
        .AddJsonFile("appsettings.Development.json", optional: false, reloadOnChange: true);

    return builder.Build()
        .GetSection("Settings")
        .Get<Settings>()!;
}

static async Task<string> FetchVariablesResponseAsync(Settings settings)
{
    var api = new AzureDevOpsApi(settings);
    return await api.GetLibraries();
}

static List<VariableGroup> DeserializeVariableGroups(string variablesResponse)
{
    return JsonSerializer.Deserialize<List<VariableGroup>>(variablesResponse)
           ?? new List<VariableGroup>();
}

static string PromptForGroupName(Settings settings)
{
    return AnsiConsole.Prompt(
        new TextPrompt<string>("[blue]What's your group?[/]")
            .DefaultValue(settings.DefaultGroup)
    );
}

static List<VariableGroup> FilterVariableGroups(List<VariableGroup> variableGroups, string searchGroupName)
{
    return variableGroups
        .Where(x => x.Name.Contains(searchGroupName, StringComparison.OrdinalIgnoreCase))
        .ToList();
}