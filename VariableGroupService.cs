using Spectre.Console;

namespace LibraryAzureDevOps;

public class VariableGroupService(Settings settings)
{
    public void WriteVariableGroups(List<VariableGroup> variableGroups)
    {
        AddEmptyLine();

        if (!variableGroups.Any())
        {
            AnsiConsole.Write("Empty");
            AddEmptyLine();
            return;
        }

        var table = CreateTable("Name", "Link");

        foreach (var variableGroup in variableGroups)
        {
            table.AddRow(variableGroup.Name, settings.GetLinkLibrary(variableGroup.Id));
            table.AddEmptyRow();
        }

        AnsiConsole.Write(table);
        AddEmptyLine();
    }

    public void WriteVariables(List<VariableGroup> variableGroups, string searchKey)
    {
        AddEmptyLine();

        WriteVariablesByVariable(variableGroups, searchKey);
        //WriteVariablesByGroup(variableGroups, searchKey);

        AddEmptyLine();
    }

    private void WriteVariablesByGroup(List<VariableGroup> variableGroups, string searchKey)
    {
        foreach (var variableGroup in variableGroups)
        {
            var keys = variableGroup.Variables.Keys.Where(x => x.Contains(searchKey));

            if (!keys.Any()) continue;

            AnsiConsole.MarkupLine($"[red]Name: {variableGroup.Name}[/] ({settings.GetLinkLibrary(variableGroup.Id)})");

            var table = CreateTable("Key", "Value");

            foreach (var key in keys)
            {
                table.AddRow(key, variableGroup.Variables[key].Value ?? "*** secret ***");
                table.AddEmptyRow();
            }

            AnsiConsole.Write(table);
            AddEmptyLine();
        }
    }

    private void WriteVariablesByVariable(List<VariableGroup> variableGroups, string searchKey)
    {
        var variables = new Dictionary<string, Dictionary<string, string>>();

        foreach (var variableGroup in variableGroups)
        {
            var keys = variableGroup.Variables.Keys.Where(x => x.Contains(searchKey));

            if (!keys.Any()) continue;

            foreach (var key in keys)
            {
                if (!variables.ContainsKey(key))
                {
                    variables[key] = new Dictionary<string, string>();
                }

                variables[key][variableGroup.Name] = variableGroup.Variables[key].Value ?? "*** secret ***";
            }
        }

        foreach (var variable in variables)
        {
            AnsiConsole.MarkupLine($"[red]Name: {variable.Key}[/]");

            var table = CreateTable("Group", "Value");

            foreach (var values in variable.Value)
            {
                table.AddRow(values.Key, Markup.Escape(values.Value));
                table.AddEmptyRow();
            }

            AnsiConsole.Write(table);
            AddEmptyLine();
        }
    }

    private void AddEmptyLine()
    {
        AnsiConsole.MarkupLine("");
    }

    private Table CreateTable(string column1, string column2)
    {
        var table = new Table().Border(TableBorder.Ascii);
        table.AddColumn(column1).Centered();
        table.AddColumn(column2);
        return table;
    }
}