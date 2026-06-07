namespace DustInTheWind.ConsoleTools.Commando;

/// <summary>
/// This attribute can be used for marking a class to be a help command.
/// When this attribute is used on a command class. That command class is used as the help command
/// instead of the default one.
/// </summary>
public class HelpCommandAttribute : NamedCommandAttribute
{
    public HelpCommandAttribute(string name)
        : base(name)
    {
    }
}