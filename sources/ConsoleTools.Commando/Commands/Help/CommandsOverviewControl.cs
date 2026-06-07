using DustInTheWind.ConsoleTools.Controls.Tables;

namespace DustInTheWind.ConsoleTools.Commando.Commands.Help;

internal class CommandsOverviewControl
{
    public string ApplicationName { get; set; }

    public List<CommandShortInfo> NamedCommands { get; set; }

    public List<CommandShortInfo> AnonymousCommands { get; set; }

    public void Display()
    {
        if (NamedCommands?.Count > 0)
        {
            Console.WriteLine();
            CustomConsole.WriteLineEmphasized("Usage:");

            Console.WriteLine($" {ApplicationName} [command] [parameters]");
            DisplayNamedCommands();
        }

        if (AnonymousCommands?.Count > 0)
        {
            Console.WriteLine();
            CustomConsole.WriteLineEmphasized("Usage:");

            Console.WriteLine($" {ApplicationName} [parameters]");
            DisplayDefaultCommands();
        }
    }

    private void DisplayNamedCommands()
    {
        Console.WriteLine();
        Console.WriteLine("Commands:");

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
        dataGrid.Columns.Add(new Column { CellPaddingRight = 0 });

        IEnumerable<ContentRow> rows = NamedCommands.Select(CreateContentRowForNamedCommand);
        dataGrid.Rows.AddRange(rows);

        dataGrid.Display();
    }

    private static ContentRow CreateContentRowForNamedCommand(CommandShortInfo commandShortInfo)
    {
        ContentRow row = new();

        row.AddCell(commandShortInfo.Name ?? "<anonymous command>");

        row.AddCell(new ContentCell
        {
            Content = commandShortInfo.Description,
            ForegroundColor = ConsoleColor.DarkGray
        });

        return row;
    }

    private void DisplayDefaultCommands()
    {
        Console.WriteLine();
        Console.WriteLine("Anonymous Command:");

        DataGrid dataGrid = new()
        {
            IsBorderVisible = false,
            MaxWidth = 80
        };

        dataGrid.Columns.Add(new Column { CellPaddingRight = 0 });
        dataGrid.Columns.Add(new Column { CellPaddingRight = 0 });

        IEnumerable<ContentRow> rows = AnonymousCommands.Select(CreateContentRowForAnonymousCommand);
        dataGrid.Rows.AddRange(rows);

        dataGrid.Display();
    }

    private static ContentRow CreateContentRowForAnonymousCommand(CommandShortInfo commandShortInfo)
    {
        ContentRow row = new();

        row.AddCell(new ContentCell
        {
            Content = commandShortInfo.Description,
            ForegroundColor = ConsoleColor.DarkGray
        });

        return row;
    }
}