using DustInTheWind.ConsoleTools.Commando.RequestModel;

namespace DustInTheWind.ConsoleTools.Commando;

public class CommandCreatedEventArgs : EventArgs
{
    public string CommandFullName { get; init; }

    public string[] Args { get; init; }

    public List<CommandArgument> UnusedOptions { get; init; }

    public List<string> UnusedOperands { get; init; }
}