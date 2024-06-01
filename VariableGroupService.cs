using Spectre.Console;

namespace LibraryAzureDevOps;

public class VariableGroupService
{
    private Settings _settings;

    public VariableGroupService(Settings settings)
    {
        _settings = settings;
    }

    public void WriteVariableGroups(List<VariableGroup> variableGroups)
    {
        AnsiConsole.MarkupLine("");

        if (!variableGroups.Any())
        {
            AnsiConsole.Write("Empty");

            AnsiConsole.MarkupLine("");

            return;
        }

        var table = new Table();
        table.Border(TableBorder.Ascii);

        table.AddColumn("Name").Centered();
        table.AddColumn("Link");

        foreach (var variableGroup in variableGroups)
        {
            table.AddRow(variableGroup.Name, _settings.GetLinkLibrary(variableGroup.Id));
            table.AddEmptyRow();
        }

        AnsiConsole.Write(table);

        AnsiConsole.MarkupLine("");
    }

    public void WriteVariables(List<VariableGroup> variableGroups, string searchKey)
    {
        foreach (var variableGroup in variableGroups)
        {
            var keys = variableGroup.Variables.Keys.Where(x => x.Contains(searchKey));

            if (!keys.Any())
            {
                continue;
            }

            AnsiConsole.MarkupLine("");

            AnsiConsole.MarkupLine("[red]Name: {0}[/] ({1})", variableGroup.Name, _settings.GetLinkLibrary(variableGroup.Id));

            var table = new Table();
            table.Border(TableBorder.Ascii);

            table.AddColumn("Key").Centered();
            table.AddColumn("Value");

            foreach (var key in keys)
            {
                table.AddRow(key, variableGroup.Variables[key].Value ?? "*** secret ***");
                table.AddEmptyRow();
            }

            AnsiConsole.Write(table);

            AnsiConsole.MarkupLine("");
        }
    }
}
