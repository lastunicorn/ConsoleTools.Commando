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
using System.Linq;
using System.Threading.Tasks;
using DustInTheWind.ConsoleTools.Commando.CommandMetadataModel;

namespace DustInTheWind.ConsoleTools.Commando.Commands.Help;

[HelpCommand("help", ShortDescription = "Obtain more details and explanation about the available commands.", Order = int.MaxValue)]
internal class HelpCommand : ICommand
{
    private readonly CommandMetadataCollection commandMetadataCollection;
    private readonly Application application;

    [CommandParameter(DisplayName = "command name", Order = 1, IsOptional = true)]
    public string CommandName { get; set; }

    public CommandsOverviewInfo CommandsOverviewInfo { get; private set; }

    public CommandFullInfo CommandFullInfo { get; private set; }

    public HelpCommand(CommandMetadataCollection commandMetadataCollection, Application application)
    {
        this.commandMetadataCollection = commandMetadataCollection ?? throw new ArgumentNullException(nameof(commandMetadataCollection));
        this.application = application ?? throw new ArgumentNullException(nameof(application));
    }

    public Task Execute()
    {
        if (CommandName != null)
            CommandFullInfo = GetCommandFullInfo(CommandName);
        else
            CommandsOverviewInfo = GetAllCommandsOverview();

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
            Commands = commandMetadataCollection.GetAllEnabled()
                .Select(x => new CommandShortInfo(x))
                .ToList()
        };
    }
}