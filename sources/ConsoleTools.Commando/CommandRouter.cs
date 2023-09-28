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
using DustInTheWind.ConsoleTools.Commando.CommandAnalysis;
using DustInTheWind.ConsoleTools.Commando.MetadataModel;
using DustInTheWind.ConsoleTools.Commando.RequestModel;

namespace DustInTheWind.ConsoleTools.Commando;

public class CommandRouter
{
    private readonly ExecutionMetadata executionMetadata;
    private readonly ICommandFactory commandFactory;

    public CommandRouter(ExecutionMetadata executionMetadata, ICommandFactory commandFactory)
    {
        this.executionMetadata = executionMetadata ?? throw new ArgumentNullException(nameof(executionMetadata));
        this.commandFactory = commandFactory ?? throw new ArgumentNullException(nameof(commandFactory));
    }

    public event EventHandler<CommandCreatedEventArgs> CommandCreated;

    public async Task Execute(CommandRequest commandRequest)
    {
        CommandRequestAnalysis commandRequestAnalysis = AnalyzeCommand(commandRequest);

        switch (commandRequestAnalysis.MatchedCommand.CommandKind)
        {
            case CommandKind.None:
                throw new UnknownCommandException();

            case CommandKind.WithoutResult:
                await ExecuteCommandWithoutResult(commandRequest, commandRequestAnalysis);
                break;

            case CommandKind.WithResult:
                await ExecuteCommandWithResult(commandRequest, commandRequestAnalysis);
                break;

            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private CommandRequestAnalysis AnalyzeCommand(CommandRequest commandRequest)
    {
        CommandRequestAnalysis commandRequestAnalysis = new(executionMetadata);
        commandRequestAnalysis.Analyze(commandRequest);

        switch (commandRequestAnalysis.MatchType)
        {
            case CommandMatchType.None:
                throw new UnknownCommandException();

            case CommandMatchType.Partial:
            case CommandMatchType.Full:
            case CommandMatchType.Help:
                return commandRequestAnalysis;

            case CommandMatchType.Multiple:
                throw new MultipleCommandsMatchException();

            default:
                throw new Exception("Invalid CommandMatchType. The analysis of the command failed in an unexpected way.");
        }
    }

    private async Task ExecuteCommandWithoutResult(CommandRequest commandRequest, CommandRequestAnalysis commandRequestAnalysis)
    {
        CommandMetadata commandMetadata = commandRequestAnalysis.MatchedCommand;
        IConsoleCommand consoleCommand = commandFactory.Create(commandMetadata) as IConsoleCommand;

        if (consoleCommand == null)
            throw new UnknownCommandException();

        commandRequestAnalysis.SetParameters(consoleCommand);
        RaiseCommandCreatedEvent(commandRequest, consoleCommand);
        await consoleCommand.Execute();
        ExecuteViewsFor(consoleCommand);
    }

    private async Task ExecuteCommandWithResult(CommandRequest commandRequest, CommandRequestAnalysis commandRequestAnalysis)
    {
        CommandMetadata commandMetadata = commandRequestAnalysis.MatchedCommand;
        object consoleCommand = commandFactory.Create(commandMetadata);

        if (consoleCommand == null)
            throw new UnknownCommandException();

        commandRequestAnalysis.SetParameters(consoleCommand);
        RaiseCommandCreatedEvent(commandRequest, consoleCommand);

        Type commandType = consoleCommand.GetType();
        MethodInfo executeMemberInfo = commandType.GetMethod("Execute");

        object viewModel = await executeMemberInfo.InvokeAsync(consoleCommand);
        ExecuteViewsFor(viewModel);
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

    private void ExecuteViewsFor(object viewModel)
    {
        Type commandResultType = viewModel.GetType();

        IEnumerable<Type> viewTypes = executionMetadata.Views.GetViewTypesForModel(commandResultType);

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