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
using DustInTheWind.ConsoleTools.Commando.CommandMetadataModel;

namespace DustInTheWind.ConsoleTools.Commando.Commands.Help;

[HelpCommand("help", Description = "Display more details about the available commands.")]
internal class HelpCommand : IConsoleCommand
{
    private readonly CommandMetadataCollection commandMetadataCollection;
    private readonly Application application;

    [AnonymousParameter(DisplayName = "command name", Order = 1, IsOptional = true, Description = "The name of the command for which to display detailed help information.")]
    public string CommandName { get; set; }

    public CommandsOverviewInfo CommandsOverviewInfo { get; private set; }

    public CommandFullInfo CommandFullInfo { get; private set; }

    public CultureInfo CultureInfo { get; private set; }

    public HelpCommand(CommandMetadataCollection commandMetadataCollection, Application application)
    {
        this.commandMetadataCollection = commandMetadataCollection ?? throw new ArgumentNullException(nameof(commandMetadataCollection));
        this.application = application ?? throw new ArgumentNullException(nameof(application));
    }

    public Task Execute()
    {
        if (CommandName == null)
        {
            CommandsOverviewInfo = GetAllCommandsOverview();
            CultureInfo = CultureInfo.CurrentCulture;
        }
        else
        {
            CommandFullInfo = GetCommandFullInfo(CommandName);
        }

        return Task.CompletedTask;
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