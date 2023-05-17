// ConsoleTools.Commando
// Copyright (C) 2022-2023 Dust in the Wind
// 
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.

using DustInTheWind.ConsoleTools.Controls.Tables;

namespace DustInTheWind.ConsoleTools.Commando.Commands.Help;

internal class CommandsOverviewControl
{
    public string ApplicationName { get; set; }

    public List<CommandShortInfo> NamedCommands { get; set; }

    public List<CommandShortInfo> DefaultCommands { get; set; }

    public void Display()
    {
        Console.WriteLine();
        CustomConsole.WriteLineEmphasized("Usage:");

        if (NamedCommands?.Count > 0)
            Console.WriteLine($" {ApplicationName} [command] [parameters]");

        if (DefaultCommands?.Count > 0)
            Console.WriteLine($" {ApplicationName} [parameters]");

        if (NamedCommands?.Count > 0)
            DisplayNamedCommands();

        if (DefaultCommands?.Count > 0)
            DisplayDefaultCommands();
    }

    private void DisplayNamedCommands()
    {
        Console.WriteLine();
        Console.WriteLine("Commands:");

        DataGrid dataGrid = new()
        {
            Border = { IsVisible = false },
            MaxWidth = 80
        };

        dataGrid.Columns.Add(new Column { CellPaddingRight = 0 });
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
            Border = { IsVisible = false },
            MaxWidth = 80
        };

        dataGrid.Columns.Add(new Column { CellPaddingRight = 0 });
        dataGrid.Columns.Add(new Column { CellPaddingRight = 0 });

        IEnumerable<ContentRow> rows = DefaultCommands.Select(CreateContentRowForAnonymousCommand);
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