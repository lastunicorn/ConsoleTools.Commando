// ConsoleTools.Commando
// Copyright (C) 2022-2024 Dust in the Wind
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