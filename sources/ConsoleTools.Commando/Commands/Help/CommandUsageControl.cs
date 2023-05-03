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

using System;
using System.Collections.Generic;
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
        Console.WriteLine();
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
        CustomConsole.WriteLineEmphasized("Options:");

        DataGrid dataGrid = new()
        {
            Border = { IsVisible = false }
        };
        foreach (CommandParameterInfo parameter in NamedParameters)
        {
            string fullName = "-" + parameter.Name;
            string parameterShortName = "-" + parameter.ShortName;
            string isOptional = parameter.IsOptional ? "(optional)" : string.Empty;
            string type = SerializeValueDescription(parameter);
            string description = parameter.Description;

            dataGrid.Columns.Add(new Column());
            dataGrid.Columns.Add(new Column());
            dataGrid.Columns.Add(new Column());
            dataGrid.Columns.Add(new Column());
            dataGrid.Columns.Add(new Column { ForegroundColor = ConsoleColor.DarkGray });

            dataGrid.Rows.Add(fullName, parameterShortName, isOptional, type, description);
        }
        dataGrid.Display();
    }

    private void DisplayOperands()
    {
        Console.WriteLine();
        CustomConsole.WriteLineEmphasized("Operands:");

        DataGrid dataGrid = new()
        {
            Border = { IsVisible = false }
        };

        foreach (CommandParameterInfo parameter in UnnamedParameters)
        {
            string index = parameter.Order == null
                ? string.Empty
                : parameter.Order.Value.ToString();
            string fullName = parameter.DisplayName.Replace(' ', '-');
            string parameterShortName = string.Empty;
            string isOptional = parameter.IsOptional
                ? "(optional)"
                : string.Empty;
            string description = SerializeValueDescription(parameter);

            dataGrid.Rows.Add(index, fullName, parameterShortName, isOptional, description);
        }

        dataGrid.Display();
    }

    private static string SerializeValueDescription(CommandParameterInfo parameter)
    {
        if (parameter.ParameterType.IsText())
            return "text";

        if (parameter.ParameterType.IsNumber())
            return "number";

        if (parameter.ParameterType.IsListOfNumbers())
            return "list-of-numbers";

        if (parameter.ParameterType.IsListOfTexts())
            return "list-of-texts";

        if (parameter.ParameterType.IsBoolean())
            return "flag";

        if (parameter.ParameterType.IsCharacter())
            return "character";

        return parameter.ParameterType.Name;
    }
}