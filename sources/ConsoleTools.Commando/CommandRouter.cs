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
using DustInTheWind.ConsoleTools.Commando.CommandAnalyzing;
using DustInTheWind.ConsoleTools.Commando.MetadataModel;
using DustInTheWind.ConsoleTools.Commando.RequestModel;
using ExecutionContext = DustInTheWind.ConsoleTools.Commando.MetadataModel.ExecutionContext;

namespace DustInTheWind.ConsoleTools.Commando;

public class CommandRouter
{
    private readonly ExecutionContext executionContext;
    private readonly ICommandFactory commandFactory;

    public CommandRouter(ExecutionContext executionContext, ICommandFactory commandFactory)
    {
        this.executionContext = executionContext ?? throw new ArgumentNullException(nameof(executionContext));
        this.commandFactory = commandFactory ?? throw new ArgumentNullException(nameof(commandFactory));
    }

    public event EventHandler<CommandCreatedEventArgs> CommandCreated;

    public async Task Execute(CommandRequest commandRequest)
    {
        RequestAnalysis requestAnalysis = AnalyzeCommand(commandRequest);

        switch (requestAnalysis.MatchedCommand.CommandKind)
        {
            case CommandKind.None:
                throw new UnknownCommandException();

            case CommandKind.WithoutResult:
                await ExecuteCommandWithoutResult(commandRequest, requestAnalysis);
                break;

            case CommandKind.WithResult:
                await ExecuteCommandWithResult(commandRequest, requestAnalysis);
                break;

            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private RequestAnalysis AnalyzeCommand(CommandRequest commandRequest)
    {
        RequestAnalysis requestAnalysis = new(executionContext);
        requestAnalysis.Analyze(commandRequest);

        switch (requestAnalysis.MatchType)
        {
            case RequestMatchType.NoMatch:
                throw new UnknownCommandException();

            case RequestMatchType.Partial:
            case RequestMatchType.Full:
            case RequestMatchType.Help:
                return requestAnalysis;

            case RequestMatchType.Multiple:
                throw new MultipleCommandsMatchException();

            default:
                throw new Exception("Invalid RequestMatchType. The analysis of the command failed in an unexpected way.");
        }
    }

    private async Task ExecuteCommandWithoutResult(CommandRequest commandRequest, RequestAnalysis requestAnalysis)
    {
        CommandMetadata commandMetadata = requestAnalysis.MatchedCommand;
        IConsoleCommand consoleCommand = commandFactory.Create(commandMetadata) as IConsoleCommand;

        if (consoleCommand == null)
            throw new UnknownCommandException();

        requestAnalysis.SetParameters(consoleCommand);
        RaiseCommandCreatedEvent(commandRequest, consoleCommand);
        await consoleCommand.Execute();
        ExecuteViewsFor(consoleCommand);
    }

    private async Task ExecuteCommandWithResult(CommandRequest commandRequest, RequestAnalysis requestAnalysis)
    {
        CommandMetadata commandMetadata = requestAnalysis.MatchedCommand;
        object consoleCommand = commandFactory.Create(commandMetadata);

        if (consoleCommand == null)
            throw new UnknownCommandException();

        requestAnalysis.SetParameters(consoleCommand);
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

        IEnumerable<Type> viewTypes = executionContext.Views.GetViewTypesForModel(commandResultType);

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