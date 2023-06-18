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

namespace DustInTheWind.ConsoleTools.Commando.Commands.Help;

internal class HelpView : ViewBase<HelpCommand>
{
    public override void Display(HelpCommand command)
    {
        if (command.CommandsOverviewInfo != null)
            DisplayCommandsOverview(command);
        else if (command.CommandFullInfo != null)
            DisplayCommandDetails(command);

        if (command.CultureInfo != null)
        {
            WriteLine();
            WriteValue("Current Culture", command.CultureInfo.Name);
            WriteNote("The current culture is determining how the arguments values are interpreted.");
        }
    }

    private static void DisplayCommandsOverview(HelpCommand command)
    {
        CommandsOverviewControl commandsOverviewControl = new()
        {
            ApplicationName = command.CommandsOverviewInfo.ApplicationName,
            NamedCommands = command.CommandsOverviewInfo.NamedCommands,
            AnonymousCommands = command.CommandsOverviewInfo.AnonymousCommands
        };

        commandsOverviewControl.Display();
    }

    private static void DisplayCommandDetails(HelpCommand command)
    {
        CommandUsageControl commandUsageControl = new()
        {
            Description = command.CommandFullInfo.Description,
            ApplicationName = command.CommandFullInfo.ApplicationName,
            CommandName = command.CommandFullInfo.Name,
            NamedParameters = command.CommandFullInfo.OptionsInfo,
            UnnamedParameters = command.CommandFullInfo.OperandsInfo
        };

        commandUsageControl.Display();
    }
}