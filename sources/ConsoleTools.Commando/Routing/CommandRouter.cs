using System.Reflection;
using DustInTheWind.ConsoleTools.Commando.Analysis;
using DustInTheWind.ConsoleTools.Commando.Metadata;
using DustInTheWind.ConsoleTools.Commando.Syntax;

namespace DustInTheWind.ConsoleTools.Commando.Routing;

public class CommandRouter
{
    private readonly MetadataContext metadataContext;
    private readonly ICommandFactory commandFactory;

    public CommandRouter(MetadataContext metadataContext, ICommandFactory commandFactory)
    {
        this.metadataContext = metadataContext ?? throw new ArgumentNullException(nameof(metadataContext));
        this.commandFactory = commandFactory ?? throw new ArgumentNullException(nameof(commandFactory));
    }

    public event EventHandler<CommandCreatedEventArgs> CommandCreated;

    public async Task Execute(XCommand xCommand)
    {
        RequestAnalysis requestAnalysis = AnalyzeRequest(xCommand);

        switch (requestAnalysis.MatchedCommand.CommandKind)
        {
            case CommandKind.None:
                throw new UnknownCommandException();

            case CommandKind.WithoutResult:
                await ExecuteCommandWithoutResult(xCommand, requestAnalysis);
                break;

            case CommandKind.WithResult:
                await ExecuteCommandWithResult(xCommand, requestAnalysis);
                break;

            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private RequestAnalysis AnalyzeRequest(XCommand xCommand)
    {
        RequestAnalysis requestAnalysis = new(xCommand, metadataContext);

        switch (requestAnalysis.MatchType)
        {
            case RequestMatchType.NoMatch:
                throw new UnknownCommandException();

            case RequestMatchType.Partial:
            case RequestMatchType.Full:
            case RequestMatchType.Help:
                return requestAnalysis;

            case RequestMatchType.OnlyName:
                string[] parameterNames = requestAnalysis.UnmatchedMandatoryParameters
                    .Select(x => x.Name)
                    .ToArray();
                throw new ParameterMissingException(parameterNames);

            case RequestMatchType.Multiple:
                throw new MultipleCommandsMatchException();

            default:
                throw new Exception("Invalid RequestMatchType. The analysis of the command failed in an unexpected way.");
        }
    }

    private async Task ExecuteCommandWithoutResult(XCommand xCommand, RequestAnalysis requestAnalysis)
    {
        CommandMetadata commandMetadata = requestAnalysis.MatchedCommand;
        IConsoleCommand consoleCommand = commandFactory.Create(commandMetadata) as IConsoleCommand;

        if (consoleCommand == null)
            throw new UnknownCommandException();

        requestAnalysis.SetParameters(consoleCommand);
        RaiseCommandCreatedEvent(xCommand, requestAnalysis.UnusedArguments, consoleCommand);
        await consoleCommand.Execute();
        ExecuteViewsFor(consoleCommand);
    }

    private async Task ExecuteCommandWithResult(XCommand xCommand, RequestAnalysis requestAnalysis)
    {
        try
        {
            CommandMetadata commandMetadata = requestAnalysis.MatchedCommand;
            object consoleCommand = commandFactory.Create(commandMetadata);

            if (consoleCommand == null)
                throw new UnknownCommandException();

            requestAnalysis.SetParameters(consoleCommand);
            RaiseCommandCreatedEvent(xCommand, requestAnalysis.UnusedArguments, consoleCommand);

            Type commandType = consoleCommand.GetType();
            MethodInfo executeMemberInfo = commandType.GetMethod(nameof(IConsoleCommand<object>.Execute));

            object viewModel = await executeMemberInfo.InvokeAsync(consoleCommand);
            ExecuteViewsFor(viewModel);
        }
        catch (TargetInvocationException ex)
        {
            if (ex.InnerException != null)
                throw new Exception("Command execution error. " + ex.InnerException.Message, ex);

            throw;
        }
    }

    private void RaiseCommandCreatedEvent(XCommand xCommand, UnusedArguments unusedArguments, object consoleCommand)
    {
        CommandCreatedEventArgs args = new()
        {
            Args = xCommand.UnderlyingArgs,
            CommandFullName = consoleCommand.GetType().FullName,
            UnusedOptions = unusedArguments.EnumerateOptions().ToList(),
            UnusedOperands = unusedArguments.EnumerateOperands().ToList()
        };

        OnCommandCreated(args);
    }

    private void ExecuteViewsFor(object viewModel)
    {
        Type commandResultType = viewModel.GetType();

        IEnumerable<Type> viewTypes = metadataContext.Views.GetViewTypesForModel(commandResultType);

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