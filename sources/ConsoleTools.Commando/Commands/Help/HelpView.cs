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

internal class HelpView : ViewBase<HelpViewModel>
{
    public override void Display(HelpViewModel viewModel)
    {
        if (viewModel.CommandsOverviewInfo != null)
            DisplayCommandsOverview(viewModel);
        else if (viewModel.CommandFullInfo != null)
            DisplayCommandDetails(viewModel);

        if (viewModel.CultureInfo != null)
        {
            WriteLine();
            WriteValue("Current Culture", viewModel.CultureInfo.Name);
            WriteNote("The current culture influences the parsing of argument's values.");
        }
    }

    private static void DisplayCommandsOverview(HelpViewModel viewModel)
    {
        CommandsOverviewControl commandsOverviewControl = new()
        {
            ApplicationName = viewModel.CommandsOverviewInfo.ApplicationName,
            NamedCommands = viewModel.CommandsOverviewInfo.NamedCommands,
            AnonymousCommands = viewModel.CommandsOverviewInfo.AnonymousCommands
        };

        commandsOverviewControl.Display();
    }

    private static void DisplayCommandDetails(HelpViewModel viewModel)
    {
        CommandUsageControl commandUsageControl = new()
        {
            Description = viewModel.CommandFullInfo.Description,
            ApplicationName = viewModel.CommandFullInfo.ApplicationName,
            CommandName = viewModel.CommandFullInfo.Name,
            NamedParameters = viewModel.CommandFullInfo.OptionsInfo,
            UnnamedParameters = viewModel.CommandFullInfo.OperandsInfo
        };

        commandUsageControl.Display();
    }
}