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

using System.Globalization;
using DustInTheWind.ConsoleTools.Commando.MetadataModel;

namespace DustInTheWind.ConsoleTools.Commando.Commands.Help;

[HelpCommand("help", Description = "Display more details about the available commands.")]
internal class HelpCommand : IConsoleCommand<HelpViewModel>
{
    private readonly CommandMetadataCollection commandMetadataCollection;
    private readonly Application application;

    [AnonymousParameter(DisplayName = "command name", Order = 1, IsOptional = true, Description = "The name of the command for which to display detailed help information.")]
    public string CommandName { get; set; }

    public HelpCommand(CommandMetadataCollection commandMetadataCollection, Application application)
    {
        this.commandMetadataCollection = commandMetadataCollection ?? throw new ArgumentNullException(nameof(commandMetadataCollection));
        this.application = application ?? throw new ArgumentNullException(nameof(application));
    }

    public Task<HelpViewModel> Execute()
    {
        HelpViewModel viewModel = new();

        if (CommandName == null)
        {
            viewModel.CommandsOverviewInfo = GetAllCommandsOverview();
            viewModel.CultureInfo = CultureInfo.CurrentCulture;
        }
        else
        {
            viewModel.CommandFullInfo = GetCommandFullInfo(CommandName);
        }

        return Task.FromResult(viewModel);
    }

    private CommandFullInfo GetCommandFullInfo(string commandName)
    {
        CommandMetadata commandMetadata = commandMetadataCollection.GetByName(commandName);

        if (commandMetadata == null)
            throw new CommandNotFoundException(commandName);

        return new CommandFullInfo
        {
            Name = commandMetadata.Name,
            Description = commandMetadata.DescriptionLines.ToList(),
            ApplicationName = application.Name,
            OptionsInfo = commandMetadata.Parameters
                .Where(x => x.Order == null)
                .Select(x => new CommandParameterInfo(x))
                .ToList(),
            OperandsInfo = commandMetadata.Parameters
                .Where(x => x.Order != null)
                .Select(x => new CommandParameterInfo(x))
                .ToList()
        };
    }

    private CommandsOverviewInfo GetAllCommandsOverview()
    {
        return new CommandsOverviewInfo
        {
            ApplicationName = application.Name,
            NamedCommands = commandMetadataCollection.GetNamed()
                .Select(x => new CommandShortInfo(x))
                .ToList(),
            AnonymousCommands = commandMetadataCollection.GetAnonymous()
                .Select(x => new CommandShortInfo(x))
                .ToList()
        };
    }
}