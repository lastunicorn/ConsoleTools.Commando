using System.Reflection;
using DustInTheWind.ConsoleTools.Commando.Analysis;
using DustInTheWind.ConsoleTools.Commando.Metadata;
using DustInTheWind.ConsoleTools.Commando.RequestModel;

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

    public async Task Execute(CommandRequest commandRequest)
    {
        RequestAnalysis requestAnalysis = AnalyzeRequest(commandRequest);

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

    private RequestAnalysis AnalyzeRequest(CommandRequest commandRequest)
    {
        RequestAnalysis requestAnalysis = new(commandRequest, metadataContext);

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
        try
        {
            CommandMetadata commandMetadata = requestAnalysis.MatchedCommand;
            object consoleCommand = commandFactory.Create(commandMetadata);

            if (consoleCommand == null)
                throw new UnknownCommandException();

            requestAnalysis.SetParameters(consoleCommand);
            RaiseCommandCreatedEvent(commandRequest, consoleCommand);

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