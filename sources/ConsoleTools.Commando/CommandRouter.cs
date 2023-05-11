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
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using DustInTheWind.ConsoleTools.Commando.CommandMetadataModel;
using DustInTheWind.ConsoleTools.Commando.CommandRequestModel;
using DustInTheWind.ConsoleTools.Commando.Commands.Empty;

namespace DustInTheWind.ConsoleTools.Commando;

public class CommandRouter
{
    private readonly CommandMetadataCollection commandMetadataCollection;
    private readonly ICommandFactory commandFactory;

    public CommandRouter(CommandMetadataCollection commandMetadataCollection, ICommandFactory commandFactory)
    {
        this.commandMetadataCollection = commandMetadataCollection ?? throw new ArgumentNullException(nameof(commandMetadataCollection));
        this.commandFactory = commandFactory ?? throw new ArgumentNullException(nameof(commandFactory));
    }

    public event EventHandler<CommandCreatedEventArgs> CommandCreated;

    public async Task Execute(CommandRequest commandRequest)
    {
        ICommand command = CreateCommandToExecute(commandRequest);

        await command.Execute();

        ExecuteViewsFor(command);
    }

    private ICommand CreateCommandToExecute(CommandRequest commandRequest)
    {
        ICommand command = CreateCommandIfExists(commandRequest)
                           ?? CreateHelpCommand()
                           ?? new EmptyCommand();

        CommandCreatedEventArgs args = new()
        {
            Args = commandRequest.UnderlyingArgs,
            CommandFullName = command.GetType().FullName,
            UnusedOptions = commandRequest.EnumerateUnusedOptions().ToList(),
            UnusedOperands = commandRequest.EnumerateUnusedOperands().ToList()
        };
        OnCommandCreated(args);

        return command;
    }

    private ICommand CreateCommandIfExists(CommandRequest commandRequest)
    {
        CommandMetadata commandMetadata = GetCommandMetadata(commandRequest);

        if (commandMetadata == null)
            throw new InvalidCommandException();

        ICommand command = commandFactory.Create(commandMetadata.Type);
        SetParameters(command, commandMetadata, commandRequest);

        return command;
    }

    private CommandMetadata GetCommandMetadata(CommandRequest commandRequest)
    {
        if (commandRequest.IsEmpty)
            return commandMetadataCollection.GetHelpCommand();

        return string.IsNullOrEmpty(commandRequest.Verb)
            ? commandMetadataCollection.GetAnonymous().FirstOrDefault()
            : commandMetadataCollection.GetByName(commandRequest.Verb);
    }

    private ICommand CreateHelpCommand()
    {
        CommandMetadata helpCommandMetadata = commandMetadataCollection.GetHelpCommand();

        return helpCommandMetadata == null
            ? null
            : commandFactory.Create(helpCommandMetadata.Type);
    }

    private static void SetParameters(ICommand command, CommandMetadata commandMetadata, CommandRequest commandRequest)
    {
        commandRequest.Reset();

        foreach (ParameterMetadata parameterMetadata in commandMetadata.Parameters)
            SetParameter(command, parameterMetadata, commandRequest);
    }

    private static void SetParameter(ICommand command, ParameterMetadata parameterMetadata, CommandRequest commandRequest)
    {
        CommandOption option = commandRequest.GetOptionAndMarkAsUsed(parameterMetadata);

        if (option != null)
        {
            parameterMetadata.SetValue(command, option.Value);
            return;
        }

        string operand = commandRequest.GetOperandAndMarkAsUsed(parameterMetadata);

        if (operand != null)
        {
            parameterMetadata.SetValue(command, operand);
            return;
        }

        if (!parameterMetadata.IsOptional)
        {
            string parameterName = parameterMetadata.Name ?? parameterMetadata.Order.ToString();
            throw new ParameterMissingException(parameterName);
        }
    }

    private void ExecuteViewsFor(ICommand command)
    {
        Type commandType = command.GetType();

        IEnumerable<Type> viewTypes = commandMetadataCollection.GetViewTypesForCommand(commandType);

        foreach (Type viewType in viewTypes)
        {
            object view = commandFactory.CreateView(viewType);

            MethodInfo displayMethodInfo = viewType.GetMethod(nameof(IView<ICommand>.Display));
            displayMethodInfo?.Invoke(view, new object[] { command });
        }
    }

    protected virtual void OnCommandCreated(CommandCreatedEventArgs e)
    {
        CommandCreated?.Invoke(this, e);
    }
}