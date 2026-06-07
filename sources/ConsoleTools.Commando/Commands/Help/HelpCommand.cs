using System.Globalization;
using DustInTheWind.ConsoleTools.Commando.MetadataModel;
using ExecutionContext = DustInTheWind.ConsoleTools.Commando.MetadataModel.ExecutionContext;

namespace DustInTheWind.ConsoleTools.Commando.Commands.Help;

[HelpCommand("help", Description = "Display more details about the available commands.")]
internal class HelpCommand : IConsoleCommand<HelpViewModel>
{
    private readonly ExecutionContext executionContext;
    private readonly Application application;

    [AnonymousParameter(DisplayName = "command name", Order = 1, IsMandatory = false, Description = "The name of the command for which to display detailed help information.")]
    public string CommandName { get; set; }

    public HelpCommand(ExecutionContext executionContext, Application application)
    {
        this.executionContext = executionContext ?? throw new ArgumentNullException(nameof(executionContext));
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
        CommandMetadata commandMetadata = executionContext.Commands.GetByName(commandName);

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
            NamedCommands = executionContext.Commands.GetNamed()
                .Select(x => new CommandShortInfo(x))
                .ToList(),
            AnonymousCommands = executionContext.Commands.GetAllAnonymous()
                .Select(x => new CommandShortInfo(x))
                .ToList()
        };
    }
}