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

using System.Reflection;
using DustInTheWind.ConsoleTools.Commando.MetadataModel;
using DustInTheWind.ConsoleTools.Commando.RequestModel;

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
        CommandMetadata commandMetadata = GetCommandMetadata(commandRequest);

        if (commandMetadata == null)
            throw new UnknownCommandException();

        switch (commandMetadata.CommandKind)
        {
            case CommandKind.None:
                throw new UnknownCommandException();

            case CommandKind.WithoutResult:
                {
                    IConsoleCommand consoleCommand = commandFactory.Create(commandMetadata) as IConsoleCommand;

                    if (consoleCommand == null)
                        throw new UnknownCommandException();

                    SetParameters(consoleCommand, commandMetadata, commandRequest);
                    RaiseCommandCreatedEvent(commandRequest, consoleCommand);
                    await consoleCommand.Execute();
                    
                    break;
                }

            case CommandKind.WithResult:
                {
                    object consoleCommand = commandFactory.Create(commandMetadata);

                    if (consoleCommand == null)
                        throw new UnknownCommandException();

                    SetParameters(consoleCommand, commandMetadata, commandRequest);
                    RaiseCommandCreatedEvent(commandRequest, consoleCommand);

                    Type commandType = consoleCommand.GetType();
                    MethodInfo executeMemberInfo = commandType.GetMethod("Execute");

                    object viewModel = await executeMemberInfo.InvokeAsync(consoleCommand);
                    ExecuteViewsFor(viewModel);

                    break;
                }

            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void RaiseCommandCreatedEvent(CommandRequest commandRequest, object consoleCommand)
    {
        CommandCreatedEventArgs args = new()
        {
            Args = commandRequest.UnderlyingArgs,
            CommandFullName = consoleCommand.GetType().FullName,
            UnusedOptions = commandRequest.EnumerateUnusedOptions().ToList(),
            UnusedOperands = commandRequest.EnumerateUnusedOperands().ToList()
        };

        OnCommandCreated(args);
    }

    private CommandMetadata GetCommandMetadata(CommandRequest commandRequest)
    {
        if (commandRequest.IsEmpty)
            return commandMetadataCollection.GetHelpCommand();

        return string.IsNullOrEmpty(commandRequest.Verb)
            ? commandMetadataCollection.GetAnonymous().FirstOrDefault()
            : commandMetadataCollection.GetByName(commandRequest.Verb);
    }

    private static void SetParameters(object consoleCommand, CommandMetadata commandMetadata, CommandRequest commandRequest)
    {
        commandRequest.Reset();

        foreach (ParameterMetadata parameterMetadata in commandMetadata.Parameters)
            SetParameter(consoleCommand, parameterMetadata, commandRequest);
    }

    private static void SetParameter(object consoleCommand, ParameterMetadata parameterMetadata, CommandRequest commandRequest)
    {
        CommandArgument argument = commandRequest.GetOptionAndMarkAsUsed(parameterMetadata);

        if (argument != null)
        {
            parameterMetadata.SetValue(consoleCommand, argument.Value);
            return;
        }

        string operand = commandRequest.GetOperandAndMarkAsUsed(parameterMetadata);

        if (operand != null)
        {
            parameterMetadata.SetValue(consoleCommand, operand);
            return;
        }

        if (!parameterMetadata.IsOptional)
        {
            string parameterName = parameterMetadata.Name ?? parameterMetadata.DisplayName ?? parameterMetadata.Order.ToString();
            throw new ParameterMissingException(parameterName);
        }
    }

    private void ExecuteViewsFor(object viewModel)
    {
        Type commandResultType = viewModel.GetType();

        IEnumerable<Type> viewTypes = commandMetadataCollection.GetViewTypesForCommand(commandResultType);

        foreach (Type viewType in viewTypes)
        {
            object view = commandFactory.CreateView(viewType);

            MethodInfo displayMethodInfo = viewType.GetMethod(nameof(IView<IConsoleCommand>.Display));
            displayMethodInfo?.Invoke(view, new[] { viewModel });
        }
    }

    protected virtual void OnCommandCreated(CommandCreatedEventArgs e)
    {
        CommandCreated?.Invoke(this, e);
    }
}