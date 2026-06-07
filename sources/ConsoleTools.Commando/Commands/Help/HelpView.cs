namespace DustInTheWind.ConsoleTools.Commando.Commands.Help;

internal class HelpView : ViewBase<HelpViewModel>
{
    public override void Display(HelpViewModel viewModel)
    {
        if (viewModel.CommandsOverviewInfo != null)
            DisplayCommandsOverview(viewModel);
        else if (viewModel.CommandFullInfo != null)
            DisplayCommandDetails(viewModel);

        if (viewModel.CultureInfo != null)
        {
            WriteLine();
            WriteValue("Current Culture", viewModel.CultureInfo.Name);
            WriteNote("The current culture influences the parsing of argument's values.");
        }
    }

    private static void DisplayCommandsOverview(HelpViewModel viewModel)
    {
        CommandsOverviewControl commandsOverviewControl = new()
        {
            ApplicationName = viewModel.CommandsOverviewInfo.ApplicationName,
            NamedCommands = viewModel.CommandsOverviewInfo.NamedCommands,
            AnonymousCommands = viewModel.CommandsOverviewInfo.AnonymousCommands
        };

        commandsOverviewControl.Display();
    }

    private static void DisplayCommandDetails(HelpViewModel viewModel)
    {
        CommandUsageControl commandUsageControl = new()
        {
            Description = viewModel.CommandFullInfo.Description,
            ApplicationName = viewModel.CommandFullInfo.ApplicationName,
            CommandName = viewModel.CommandFullInfo.Name,
            NamedParameters = viewModel.CommandFullInfo.OptionsInfo,
            UnnamedParameters = viewModel.CommandFullInfo.OperandsInfo
        };

        commandUsageControl.Display();
    }
}