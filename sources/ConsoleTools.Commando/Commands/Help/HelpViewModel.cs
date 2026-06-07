using System.Globalization;

namespace DustInTheWind.ConsoleTools.Commando.Commands.Help;

internal class HelpViewModel
{
    public CommandsOverviewInfo CommandsOverviewInfo { get; set; }

    public CommandFullInfo CommandFullInfo { get; set; }

    public CultureInfo CultureInfo { get; set; }
}