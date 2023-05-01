// ConsoleTools.Commando
// Copyright (C) 2022 Dust in the Wind
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
using DustInTheWind.ConsoleTools.Commando.Commands.Empty;
using DustInTheWind.ConsoleTools.Commando.GenericCommandModel;

namespace DustInTheWind.ConsoleTools.Commando;

public class CommandRouter
{
    private readonly CommandMetadataCollection commandMetadataCollection;
    private readonly ICommandFactory commandFactory;

    public event EventHandler<CommandCreatedEventArgs> CommandCreated;

    public CommandRouter(CommandMetadataCollection commandMetadataCollection, ICommandFactory commandFactory)
    {
        this.commandMetadataCollection = commandMetadataCollection ?? throw new ArgumentNullException(nameof(commandMetadataCollection));
        this.commandFactory = commandFactory ?? throw new ArgumentNullException(nameof(commandFactory));
    }

    public async Task Execute(GenericCommand genericCommand)
    {
        ICommand command = CreateCommandToExecute(genericCommand);

        await command.Execute();

        ExecuteViewsFor(command);
    }

    private ICommand CreateCommandToExecute(GenericCommand genericCommand)
    {
        ICommand command = CreateCommandIfExists(genericCommand)
                           ?? CreateHelpCommand()
                           ?? new EmptyCommand();

        CommandCreatedEventArgs args = new()
        {
            Args = genericCommand.UnderlyingArgs,
            CommandFullName = command.GetType().FullName,
            UnusedOptions = genericCommand.EnumerateUnusedOptions().ToList(),
            UnusedOperands = genericCommand.EnumerateUnusedOperands().ToList()
        };
        OnCommandCreated(args);

        return command;
    }

    private ICommand CreateCommandIfExists(GenericCommand genericCommand)
    {
        if (string.IsNullOrEmpty(genericCommand.Verb))
            return null;

        CommandMetadata commandMetadata = commandMetadataCollection.GetByName(genericCommand.Verb);

        if (commandMetadata == null)
            throw new InvalidCommandException();

        ICommand command = commandFactory.Create(commandMetadata.Type);
        SetParameters(command, commandMetadata, genericCommand);

        return command;
    }

    private ICommand CreateHelpCommand()
    {
        CommandMetadata helpCommandMetadata = commandMetadataCollection.GetHelpCommand();

        return helpCommandMetadata == null
            ? null
            : commandFactory.Create(helpCommandMetadata.Type);
    }

    private static void SetParameters(ICommand command, CommandMetadata commandMetadata, GenericCommand genericCommand)
    {
        genericCommand.Reset();

        foreach (ParameterMetadata parameterMetadata in commandMetadata.Parameters)
            SetParameter(command, parameterMetadata, genericCommand);
    }

    private static void SetParameter(ICommand command, ParameterMetadata parameterMetadata, GenericCommand genericCommand)
    {
        GenericCommandOption option = genericCommand.GetOptionAndMarkAsUsed(parameterMetadata);

        if (option != null)
        {
            parameterMetadata.SetValue(command, option.Value);
            return;
        }

        string operand = genericCommand.GetOperandAndMarkAsUsed(parameterMetadata);

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