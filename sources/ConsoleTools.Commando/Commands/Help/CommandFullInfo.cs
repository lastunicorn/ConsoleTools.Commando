using DustInTheWind.ConsoleTools.Controls;

namespace DustInTheWind.ConsoleTools.Commando.Commands.Help;

public class CommandFullInfo
{
    public string Name { get; set; }

    public MultilineText Description { get; init; }

    public string ApplicationName { get; set; }

    public List<CommandParameterInfo> OptionsInfo { get; set; }

    public List<CommandParameterInfo> OperandsInfo { get; set; }
}