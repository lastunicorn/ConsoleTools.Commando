namespace DustInTheWind.ConsoleTools.Commando.Commands.Help;

public class CommandNotFoundException : Exception
{
    public CommandNotFoundException(string commandName)
        : base(BuildMessage(commandName))
    {
    }

    private static string BuildMessage(string commandName)
    {
        return string.Format(Resources.ErrorMessage_CommandNotFound, commandName);
    }
}