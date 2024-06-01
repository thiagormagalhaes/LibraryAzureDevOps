using LibraryAzureDevOps;
using Microsoft.Extensions.Configuration;
using Spectre.Console;
using System.Text.Json;

var builder = new ConfigurationBuilder()
    .SetBasePath(AppContext.BaseDirectory)
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

var settings = builder.Build()
    .GetSection("Settings")
    .Get<Settings>()!;

var variablesResponse = await new AzureDevOpsApi(settings.LinkApi, settings.UserAuthentication).GetLibraries();

var variableGroups = JsonSerializer.Deserialize<List<VariableGroup>>(variablesResponse);

if (variableGroups is null)
{
    Console.WriteLine("Não encontramos nada!");
    return;
}

var findGroup = AnsiConsole.Ask<string>("[blue]What's your group?[/]");

var variableGroupFinded = variableGroups.Where(x => x.Name.Contains(findGroup))
    .ToList();

AnsiConsole.MarkupLine("");

AnsiConsole.MarkupLine("[grey] Grupos encontrados:[/]");

foreach (var variableGroup in variableGroupFinded)
{
    AnsiConsole.MarkupLine("[grey] - {0} ({1})[/]", variableGroup.Name, settings.GetLinkLibrary(variableGroup.Id));
}

AnsiConsole.MarkupLine("");

var findVariable = AnsiConsole.Ask<string>("[blue]What variable are you looking for?[/]");
AnsiConsole.MarkupLine("");


foreach (var variableGroup in variableGroupFinded)
{
    var keys = variableGroup.Variables.Keys.Where(x => x.Contains(findVariable));

    if (!keys.Any())
    {
        continue;
    }

    AnsiConsole.MarkupLine("[red]Name: {0}[/] - ({1})", variableGroup.Name, settings.GetLinkLibrary(variableGroup.Id));

    foreach (var key in keys)
    {
        AnsiConsole.MarkupLine(" - {0}: [grey]{1}[/]", key, variableGroup.Variables[key].Value);
    }

    AnsiConsole.MarkupLine("");
}