using LibraryAzureDevOps;
using Microsoft.Extensions.Configuration;
using Spectre.Console;
using System.Text.Json;

var builder = new ConfigurationBuilder()
    .SetBasePath(AppContext.BaseDirectory)
    .AddJsonFile("appsettings.Development.json", optional: false, reloadOnChange: true);

var settings = builder.Build()
    .GetSection("Settings")
    .Get<Settings>()!;

var variablesResponse = await new AzureDevOpsApi(settings.LinkApi, settings.UserAuthentication).GetLibraries();

var variableGroupService = new VariableGroupService(settings);

// ============================

var variableGroups = JsonSerializer.Deserialize<List<VariableGroup>>(variablesResponse);

var searchGroupName = AnsiConsole.Prompt(new TextPrompt<string>("[blue]What's your group?[/]")
    .DefaultValue(settings.DefaultGroup));

var variableGroupFound = variableGroups.Where(x => x.Name.Contains(searchGroupName))
    .ToList();

variableGroupService.WriteVariableGroups(variableGroupFound);

if (!variableGroupFound.Any())
{
    return;
}

AnsiConsole.MarkupLine("");

var searchKey = AnsiConsole.Ask<string>("[blue]What variable are you looking for?[/]");

variableGroupService.WriteVariables(variableGroupFound, searchKey);