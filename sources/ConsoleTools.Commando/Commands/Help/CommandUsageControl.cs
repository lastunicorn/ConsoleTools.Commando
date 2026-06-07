using System.Text;
using DustInTheWind.ConsoleTools.Controls.Tables;

namespace DustInTheWind.ConsoleTools.Commando.Commands.Help;

internal class CommandUsageControl
{
    public string ApplicationName { get; set; }

    public string CommandName { get; set; }

    public List<CommandParameterInfo> NamedParameters { get; set; }

    public List<CommandParameterInfo> UnnamedParameters { get; set; }

    public string Description { get; set; }

    public void Display()
    {
        if (!string.IsNullOrEmpty(Description))
            DisplayDescription();

        DisplayUsageOverview();

        if (NamedParameters?.Count > 0)
            DisplayOptions();

        if (UnnamedParameters?.Count > 0)
            DisplayOperands();
    }

    private void DisplayDescription()
    {
        CustomConsole.WriteLine();
        CustomConsole.WriteLine(ConsoleColor.DarkGray, Description);
    }

    private void DisplayUsageOverview()
    {
        Console.WriteLine();
        CustomConsole.WriteLineEmphasized("Usage:");

        StringBuilder sb = new();
        sb.Append($" {ApplicationName} {CommandName}");

        if (NamedParameters?.Count > 0)
            sb.Append(" [Options]");

        if (UnnamedParameters?.Count > 0)
            sb.Append(" [Operands]");

        Console.WriteLine(sb.ToString());
    }

    private void DisplayOptions()
    {
        Console.WriteLine();
        CustomConsole.WriteLineEmphasized("Options (named arguments):");

        DataGrid dataGrid = new()
        {
            IsBorderVisible = false,
            MaxWidth = 80
        };

        dataGrid.Columns.Add(new Column
        {
            CellPaddingRight = 0,
            CellContentOverflow = CellContentOverflow.PreserveOverflow
        });
        dataGrid.Columns.Add(new Column
        {
            CellPaddingRight = 0,
            CellContentOverflow = CellContentOverflow.PreserveOverflow
        });
        dataGrid.Columns.Add(new Column
        {
            CellPaddingRight = 0,
            CellContentOverflow = CellContentOverflow.PreserveOverflow
        });
        dataGrid.Columns.Add(new Column
        {
            CellPaddingRight = 0,
            CellContentOverflow = CellContentOverflow.PreserveOverflow
        });
        dataGrid.Columns.Add(new Column
        {
            CellPaddingRight = 0,
            ForegroundColor = ConsoleColor.DarkGray
        });

        foreach (CommandParameterInfo parameter in NamedParameters)
        {
            string fullName = "--" + parameter.Name;
            string shortName = "-" + parameter.ShortName;
            string isOptional = parameter.IsMandatory
                ? null
                : "(?)";
            string type = parameter.ParameterType.ToUserFriendlyName();
            string description = parameter.Description;

            dataGrid.Rows.Add(fullName, shortName, isOptional, type, description);
        }
        dataGrid.Display();
    }

    private void DisplayOperands()
    {
        Console.WriteLine();
        CustomConsole.WriteLineEmphasized("Operands (anonymous arguments):");

        DataGrid dataGrid = new()
        {
            IsBorderVisible = false,
            MaxWidth = 80
        };

        dataGrid.Columns.Add(new Column
        {
            CellPaddingRight = 0,
            CellContentOverflow = CellContentOverflow.PreserveOverflow
        });
        dataGrid.Columns.Add(new Column
        {
            CellPaddingRight = 0,
            CellContentOverflow = CellContentOverflow.PreserveOverflow
        });
        dataGrid.Columns.Add(new Column
        {
            CellPaddingRight = 0,
            CellContentOverflow = CellContentOverflow.PreserveOverflow
        });
        dataGrid.Columns.Add(new Column
        {
            CellPaddingRight = 0,
            ForegroundColor = ConsoleColor.DarkGray
        });

        foreach (CommandParameterInfo parameter in UnnamedParameters)
        {
            string index = parameter.Order?.ToString();
            string isOptional = parameter.IsMandatory
                ? null
                : "(?)";
            string type = parameter.ParameterType.ToUserFriendlyName();
            string description = parameter.Description;

            dataGrid.Rows.Add(index, isOptional, type, description);
        }

        dataGrid.Display();
    }
}