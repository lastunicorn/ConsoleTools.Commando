namespace DustInTheWind.ConsoleTools.Commando.Commands.Help;

public class CommandsOverviewInfo
{
    public string ApplicationName { get; set; }

    public List<CommandShortInfo> NamedCommands { get; set; }

    public List<CommandShortInfo> AnonymousCommands { get; set; }
}